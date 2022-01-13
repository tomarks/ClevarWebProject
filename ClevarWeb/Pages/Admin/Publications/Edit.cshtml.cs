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

namespace ClevarWeb.Pages.Admin.Publications
{
    [BindProperties]
    public class EditPublicationModel : PageModel
    {
        private readonly ClevarDbContext db;
        private readonly IMapper mapper;
        public List<SelectListItem> PeopleSelectListItems;

        public EditPublicationModel(ClevarDbContext context, IMapper mapper)
        {
            this.db = context;
            this.mapper = mapper;
            PeopleSelectListItems = DropdownHelpers.GetPeopleData(this.db);
        }

        [Required] 
        public int? AuthorID { get; set; }
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Publication Name", Prompt = "Enter the publication name...")]
        public string PublicationName { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Publication URL", Prompt = "Enter the publications URL...")]
        [RegularExpression(@"^https?:\/\/.*", ErrorMessage = "Invalid URL - Must start with http:// or https://")]
        public string PublicationUrl { get; set; }

        [Required]
        [Display(Name = "Publish Date")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}")]
        public DateTime PublishedDateTime { get; set; }

        public string PrimaryDocumentPath { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Setup Model
            if (id == null)
                return NotFound();

            var publication = await db.Publications
                .Include(x => x.PrimaryDocument)
                .Include(x => x.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (publication == null)
                return NotFound();

            // Gets the primary document image or sets up a default image.
            if (publication.PrimaryDocument != null)
                PrimaryDocumentPath = publication.PrimaryDocument.GetFilePath();
            else
                PrimaryDocumentPath = DocumentService.ResolveDocumentRelativePath("DefaultProjectImage.png");

            // Automap Properties Database Model => EditProjectModel
            mapper.Map(publication, this);

            // Setup Dropdown Props
            if (publication.Author != null)
                AuthorID = int.Parse(publication.Author.AsSelectListItem.Value);

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
            var publication = db.Publications.Where(x => x.Id == this.Id).First();

            var newDocument = Document.CreateDocument(UploadedFile.FileName, ms.ToArray());
            db.Documents.Add(newDocument);
            publication.PrimaryDocument = newDocument;
            db.SaveChanges();

            return new JsonResult(
                new
                {
                    name = newDocument.FileName,
                    path = newDocument.GetFilePath()
                });
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            // Check Model State
            if (!ModelState.IsValid)
                return Page();
            
            if (!PublicationExists(Id))
                return NotFound();

            var publication = db.Publications.Where(x => x.Id == Id).First();

            // Automap Properties EditProjectModel => Database Model
            mapper.Map(this, publication);

            // Process Custom Dropdowns
            if (AuthorID > 0)
                publication.Author = db.People.Where(x => x.Id == AuthorID).FirstOrDefault();
            else
                publication.Author = null;

            db.Attach(publication).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicationExists(publication.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PublicationExists(int id)
        {
            return db.Publications.Any(e => e.Id == id);
        }
    }
}
