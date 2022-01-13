using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;

namespace ClevarWeb
{
    public class PeopleDetailsModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;

        public PeopleDetailsModel(ClevarWeb.Data.ClevarDbContext context)
        {
            db = context;
        }

        public Person Person { get; set; }
        public List<Person> People { get; set; } = new List<Person>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Person = await db.People
                .Include(x => x.PrimaryDocument)
                .Include(x => x.AuthoredProjects)
                .Include(x => x.SupervisedProjects)
                .Include(x => x.CartoonImages)
                .ThenInclude(x => x.Document)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Person == null)
                return NotFound();

            People.Add(Person);

            return Page();
        }

        public async Task<IActionResult> OnPostPublishPersonAsync(int? PersonId)
        {
            if (PersonId == null)
                return NotFound();

            Person = await db.People.FirstOrDefaultAsync(m => m.Id == PersonId);

            if (Person == null)
                return NotFound();

            Person.IsActive = true;
            await db.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
