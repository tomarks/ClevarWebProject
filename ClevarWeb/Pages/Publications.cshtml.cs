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
    public class PublicationsModel : PageModel
    {
        private readonly ClevarDbContext db;

        public PublicationsModel(ClevarDbContext context)
        {
            db = context;
        }

        public IList<Publication> Publications { get;set; }

        public async Task OnGetAsync()
        {
            
                Publications = await db.Publications
                    .Include(x => x.PrimaryDocument)
                    .Include(x => x.Author)
                    .ThenInclude(x => x.PrimaryDocument)
                    //.Include(x => x.PublicationUrl)
                    .OrderByDescending(x => x.PublishedDateTime)
                    .Take(10)
                    .ToListAsync();

        }
    }
}
