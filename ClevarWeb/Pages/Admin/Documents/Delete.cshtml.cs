using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;

namespace ClevarWeb.Pages.Admin.Documents
{
    public class DeleteModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;

        public DeleteModel(ClevarWeb.Data.ClevarDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public Document document { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            document = await db.Documents.FirstOrDefaultAsync(m => m.Id == id);

            if (document == null)
                return NotFound();

            return Page();
        }

        // Delete Document
        public IActionResult OnPost(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            Document.DeleteDocument(id.Value, db);

            return RedirectToPage("./Index");
        }
    }
}
