using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;

namespace ClevarWeb.Pages.Admin.Publications
{
    public class PublicationDeleteModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;

        public PublicationDeleteModel(ClevarDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public Publication publication { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            publication = await db.Publications.FirstOrDefaultAsync(x => x.Id == id);

            if (publication == null)
                return NotFound();

            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            Publication.DeletePublication(id.Value, db);

            return RedirectToPage("./Index");
        }
    }
}
