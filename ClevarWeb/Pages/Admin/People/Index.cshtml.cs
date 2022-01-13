using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;

namespace ClevarWeb.Pages.Admin.People
{
    public class IndexModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;

        public IndexModel(ClevarWeb.Data.ClevarDbContext context)
        {
            db = context;
        }

        public IList<Person> People { get; set; }

        public async Task OnGetAsync()
        {
            People = await db.People
                .Include(x => x.PrimaryDocument)
                .Include(x => x.AuthoredProjects)
                .Include(x => x.SupervisedProjects)
                .Include(x => x.AuthoredPublications)
                .ToListAsync();
        }
    }
}
