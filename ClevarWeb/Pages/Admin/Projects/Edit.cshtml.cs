using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;
using ClevarWeb.Data.SampleData;
using ClevarWeb.Utility;
using ClevarWeb.Data.Helpers;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using static ClevarWeb.Data.Models.Document;

namespace ClevarWeb.Pages.Admin.Projects
{
    [BindProperties]
    public class EditProjectModel : PageModel
    {
        private readonly ClevarDbContext db;
        private readonly IMapper mapper;
        public List<SelectListItem> PeopleSelectListItems;

        public EditProjectModel(ClevarDbContext context, IMapper mapper)
        {
            this.db = context;
            this.mapper = mapper;
            PeopleSelectListItems = DropdownHelpers.GetPeopleData(this.db);
        }

        [Required] 
        public int? AuthorID { get; set; }
        public int? SupervisorID { get; set; }
        public int Id { get; set; }

        [Required] 
        [MaxLength(255)]
        [Display(Name = "Project Name", Prompt = "Enter a Name for this Project...")]
        public string Title { get; set; }

        [Display(Name = "Is Project Active?", Description = "Is this project displayed on the public website?")]
        public bool IsActive { get; set; }

        [Required] 
        [MaxLength(4000)]
        [Display(Name = "Short Description", Prompt = "Enter a short Description describing a quick overview of the Project...")]
        public string Description { get; set; }
        
        [Required]
        [Display(Name = "Publish Date")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}")]
        public DateTime PublishedDateTime { get; set; }

        [Display(Name = "Project Content",Prompt = "Write about your project here...")]
        public string HTMLContent { get; set; }

        public string PrimaryDocumentPath { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Setup Model
            if (id == null)
                return NotFound();

            var Project = await db.Projects
                .Include(x => x.PrimaryDocument)
                .Include(x => x.Author)
                .Include(x => x.Supervisor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Project == null)
                return NotFound();

            // Gets the primary document image or sets up a default image.
            if (Project.PrimaryDocument != null)
                PrimaryDocumentPath = Project.PrimaryDocument.GetFilePath();
            else
                PrimaryDocumentPath = DocumentService.ResolveDocumentRelativePath("DefaultProjectImage.png");

            // Automap Properties Database Model => EditProjectModel
            mapper.Map(Project, this);

            // Setup Dropdown Props
            if (Project.Author != null)
                AuthorID = int.Parse(Project.Author.AsSelectListItem.Value);

            if (Project.Supervisor != null)
                SupervisorID = int.Parse(Project.Supervisor.AsSelectListItem.Value);

            // Return
            return Page();
        }

        [Display(Name = "Upload New Image")]
        public IFormFile UploadedFile { get; set; }
        public JsonResult OnPostUploadFile()
        {
            if (UploadedFile == null || this.Id == 0)
                return new JsonResult(new { error = "Unable to match ID or find an Uploaded File." });

            var ms = new MemoryStream();
            UploadedFile.OpenReadStream().CopyTo(ms);
            var project = db.Projects.Where(x => x.Id == this.Id).First();

            var newDocument = Document.CreateDocument(UploadedFile.FileName, ms.ToArray());
            db.Documents.Add(newDocument);
            project.PrimaryDocument = newDocument;
            db.SaveChanges();

            return new JsonResult(
                new
                {
                    name = newDocument.FileName,
                    path = newDocument.GetFilePath()
                });
        }

        public async Task<IActionResult> OnPostSaveAsync(string SaveAndContinue)
        {
            // Check Model State
            if (!ModelState.IsValid)
                return Page();
            
            if (!ProjectExists(Id))
                return NotFound();

            var Project = db.Projects.Where(x => x.Id == Id).First();

            // Automap Properties EditProjectModel => Database Model
            mapper.Map(this, Project);

            // Process Custom Dropdowns
            if (AuthorID > 0)
                Project.Author = db.People.Where(x => x.Id == AuthorID).FirstOrDefault();
            else
                Project.Author = null;

            if (SupervisorID > 0)
                Project.Supervisor = db.People.Where(x => x.Id == SupervisorID).FirstOrDefault();
            else
                Project.Supervisor = null;

            db.Attach(Project).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(Project.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            switch (SaveAndContinue)
            {
                case "AddPerson":
                    return RedirectToPage("/Admin/People/Create");
                case "Preview":
                    return RedirectToPage("/Admin/Projects/Details", new { id = Project.Id });
                default:
                    return RedirectToPage("./Index");
            }
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Any(e => e.Id == id);
        }
    }
}
