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

namespace ClevarWeb.Pages.Admin.People
{
    [BindProperties]
    public class EditPeopleModel : PageModel
    {
        private readonly ClevarDbContext db;
        private readonly IMapper mapper;

        public EditPeopleModel(ClevarDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        /// <summary>
        /// Model Properties
        /// </summary>
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        [Display(Name = "Clevar Person Name", Prompt = "Enter the clevar persons Name")]
        public string Name { get; set; }

        [MaxLength(255)]
        [Display(Name = "Title", Prompt = "Enter the CLEVAR persons Title, eg.. Grand Poobah")]
        public string Title { get; set; }

        [MaxLength(255)]
        [Display(Name = "Sub Title", Prompt = "Enter the CLEVAR persons Sub Title, eg.. Master of the Virtual")]
        public string SubTitle { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Bio Content", Prompt = "Write a short Bio for this person...")]
        public string BioHtml { get; set; }

        public string PrimaryDocumentPath { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Setup Model
            if (id == null)
                return NotFound();

            var person = await db.People
                .Include(x => x.PrimaryDocument)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
                return NotFound();

            // Gets the primary document image or sets up a default image.
            if (person.PrimaryDocument != null)
                PrimaryDocumentPath = person.PrimaryDocument.GetFilePath();
            else
                PrimaryDocumentPath = DocumentService.ResolveDocumentRelativePath("DefaultPersonImage.png");

            // Automap matching properties
            mapper.Map(person, this);

            // Return
            return Page();
        }

        [Display(Name = "Upload New Bio Image")]
        public IFormFile UploadedFile { get; set; }
        public async Task<JsonResult> OnPostUploadFileAsync()
        {
            if (UploadedFile == null || this.Id == 0)
                return new JsonResult(new { error = "Unable to match ID or find an Uploaded File." });

            var ms = new MemoryStream();
            UploadedFile.OpenReadStream().CopyTo(ms);
            var person = db.People.Where(x => x.Id == this.Id).First();

            var newDocument = Document.CreateDocument(UploadedFile.FileName, ms.ToArray());
            db.Documents.Add(newDocument);
            person.PrimaryDocument = newDocument;
            await db.SaveChangesAsync();

            return new JsonResult(
                new
                {
                    name = newDocument.FileName,
                    path = newDocument.GetFilePath()
                });
        }


        public async Task<IActionResult> OnPostSaveAsync(string SaveAndContinue)
        {
            var person = db.People.Where(x => x.Id == this.Id).First();
            mapper.Map(this, person);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.Attach(person).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(person.Id))
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
                case "EditCartoon":
                    return RedirectToPage("/Admin/People/EditCartoon", new { id = person.Id });
                case "Preview":
                    return RedirectToPage("/Admin/People/Details", new { id = person.Id });
                default:
                    return RedirectToPage("./Index");
            }

        }

        private bool PersonExists(int id) => db.People.Any(x => x.Id == id);

    }
}
