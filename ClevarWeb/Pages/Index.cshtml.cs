using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;
using ClevarWeb.Data.SampleData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClevarWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ClevarDbContext db;

        public IndexModel(ClevarDbContext dbContext)
        {
            db = dbContext;
        }

        public IList<Project> Projects { get; set; }
        public IList<Document> Documents { get; set; }

        public async Task OnGetAsync()
        {
            db.SystemSettings.Any();

            try
            {
                Documents = await db.Documents
                    .Where(x => x.IsDisplayOnHomePage)
                    .OrderByDescending(x => x.ModifiedDateTime)
                    .ToListAsync();

                Projects = await db.Projects
                    .Include(x => x.PrimaryDocument)
                    .Include(x => x.Author)
                    .Include(x => x.Supervisor)
                    .OrderByDescending(x => x.PublishedDateTime)
                    .Where(x => x.IsActive)
                    .Take(10)
                    .ToListAsync();

                if (Projects.Count == 0)
                    Projects = ProjectSamples.AsList();
            }
            catch (Exception)
            {
                Projects = ProjectSamples.AsList();
            }
        }
    }
}
