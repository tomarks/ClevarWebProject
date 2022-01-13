using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;

namespace ClevarWeb.Pages.Admin.People
{
    public class CreateModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;

        public CreateModel(ClevarWeb.Data.ClevarDbContext context)
        {
            db = context;
        }

        public IActionResult OnGet()
        {
            Person = Person.CreatePerson();
            return Page();
        }

        [BindProperty]
        public Person Person { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.People.Add(Person);
            await db.SaveChangesAsync();

            return RedirectToPage("./Edit", new { id = Person.Id });
        }
    }
}
