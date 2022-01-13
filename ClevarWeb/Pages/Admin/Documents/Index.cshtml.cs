using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;
using ClevarWeb.Utility;
using static ClevarWeb.Data.Models.Document;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ClevarWeb.Pages.Admin.Documents
{
    public class IndexModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;

        public IndexModel(ClevarWeb.Data.ClevarDbContext context)
        {
            db = context;
        }

        public IList<Document> Documents { get;set; }

        public async Task OnGetAsync()
        {
            Documents = await db.Documents
                .Where(x => x.DocumentType == DocumentTypeEnum.Uploaded.ToString())
                .OrderByDescending(x => x.CreatedDateTime)
                .ToListAsync();
        }

        [Display(Name = "Upload New Bio Image")]
        [BindProperty]
        public IFormFile UploadedFile { get; set; }
        public async Task<JsonResult> OnPostUploadFileAsync()
        {
            var ms = new MemoryStream();
            UploadedFile.OpenReadStream().CopyTo(ms);

            var newDocument = Document.CreateDocument(UploadedFile.FileName, ms.ToArray());
            db.Documents.Add(newDocument);
            await db.SaveChangesAsync();

            return new JsonResult(
                new
                {
                    name = newDocument.FileName,
                    path = newDocument.GetFilePath()
                });
        }

        public IActionResult OnPostAddOrRemoveImageOnHomepage(int id)
        {
            var documentToChange = db.Documents.FirstOrDefault(x => x.Id == id);

            if (documentToChange == null)
                return NotFound();

            documentToChange.IsDisplayOnHomePage = !documentToChange.IsDisplayOnHomePage;
            db.SaveChanges();

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostRebuildDatabaseDocuments()
        {
            DocumentService.RebuildDatabaseDocuments(db);

            return RedirectToPage("./Index");
        }
    }
}
