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
    public class DetailsModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;

        public DetailsModel(ClevarWeb.Data.ClevarDbContext context)
        {
            db = context;
        }

        public Project Project { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Project = await db.Projects.Include(x => x.PrimaryDocument).FirstOrDefaultAsync(m => m.Id == id);

            if (Project == null)
                return NotFound();

            return Page();
        }
    }
}
