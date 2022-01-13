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
    public class PublicationIndexModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;

        public PublicationIndexModel(ClevarWeb.Data.ClevarDbContext context)
        {
            db = context;
        }

        public IList<Publication> Publications { get;set; }

        public async Task OnGetAsync()
        {
            Publications = await db.Publications
                .Include(x => x.Author)
                .ThenInclude(x => x.PrimaryDocument)
                .Include(x => x.PrimaryDocument)
                .ToListAsync();
        }
    }
}
