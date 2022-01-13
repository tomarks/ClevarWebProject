using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;

namespace ClevarWeb.Pages.Admin.Projects
{
    public class IndexModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;

        public IndexModel(ClevarWeb.Data.ClevarDbContext context)
        {
            db = context;
        }

        public int PersonId { get; set; }
        public IList<Project> Projects { get;set; }

        public async Task OnGetAsync(int personId)
        {
            PersonId = personId;

            Projects = await db.Projects
                .Include(x => x.Author)
                .ThenInclude(x => x.PrimaryDocument)
                .Include(x => x.PrimaryDocument)
                .ToListAsync();
        }
    }
}
