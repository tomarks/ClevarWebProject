using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;

namespace ClevarWeb.Pages.Admin.Publications
{
    public class CreateModel : PageModel
    {
        private readonly ClevarDbContext db;

        public CreateModel(ClevarDbContext context)
        {
            db = context;
        }

        public IActionResult OnGet()
        {
            publication = Publication.CreatePublication();
            return Page();
        }

        [BindProperty]
        public Publication publication { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.Publications.Add(publication);
            await db.SaveChangesAsync();

            return RedirectToPage("./Edit", new { id = publication.Id });
        }
    }
}
