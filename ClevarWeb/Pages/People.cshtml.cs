using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;
using ClevarWeb.Data.SampleData;

namespace ClevarWeb
{
    public class PeoplePageModel : PageModel
    {
        private readonly ClevarDbContext db;

        public PeoplePageModel(ClevarDbContext context)
        {
            db = context;
        }

        public IList<Person> People { get;set; }
        public string JsonData { get; set; }

        public async Task OnGetAsync()
        {
            People = await db.People
                .Where(x => x.IsActive == true)
                .Include(x => x.PrimaryDocument)
                .Include(x => x.SupervisedProjects)
                .Include(x => x.AuthoredProjects)
                .Include(x => x.CartoonImages)
                .ThenInclude(x => x.Document)
                .ToListAsync();
        }
    }
}
