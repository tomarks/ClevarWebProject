using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClevarWeb.Data.Helpers;

namespace ClevarWeb
{
    public class ProjectsModel : PageModel
    {
        private readonly ClevarDbContext db;

        [BindProperty]
        public int AuthorId { get; set; }

        [BindProperty]
        public int SupervisorId { get; set; }

        public List<SelectListItem> PeopleSelectListItems;

        public ProjectsModel(ClevarDbContext context)
        {
            db = context;
            PeopleSelectListItems = DropdownHelpers.GetPeopleData(this.db);
            PeopleSelectListItems[0] = new SelectListItem()
            {
                Text = "All"
            };
        }

        public IList<Project> Projects { get; set; }

        public async Task OnGetAsync(int? authorId, int? supervisorId)
        {
            var ProjectsQuery = db.Projects
                .Include(x => x.PrimaryDocument)
                .Include(x => x.Author)
                .Include(x => x.Supervisor)
                .OrderByDescending(x => x.PublishedDateTime)
                .Where(x => x.IsActive);

            if (authorId != null)
            {
                AuthorId = authorId.Value;
                ProjectsQuery = ProjectsQuery.Where(x => x.Author.Id == authorId);
            }
            if (supervisorId != null)
            {
                SupervisorId = supervisorId.Value;
                ProjectsQuery = ProjectsQuery.Where(x => x.Supervisor.Id == supervisorId);
            }

            Projects = await ProjectsQuery.ToListAsync();
        }
    }
}
