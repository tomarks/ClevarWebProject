using ClevarWeb.Data.Models.Internals;
using ClevarWeb.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ClevarWeb.Data.Models
{
    public class Project : ClevarTableBase
    {
        /// <TODO>
        /// • Build page slugs from project name
        /// • Ensure these are unique when inserting into database
        /// • When prompting user to enter a new project, ensure database name is created before moving onto editing the content.
        /// </TODO>
        [Required]
        [MaxLength(255)]
        [Display(Name = "Project Title", Prompt = "Enter a Name for this Project...")]
        public string Title { get; set; }

        [Required]
        [MaxLength(4000)]
        [Display(Name = "Short Description", Prompt = "Enter a short Description describing a quick overview of the Project...")]
        public string Description { get; set; }

        [Display(Name = "Project Author")]
        public virtual Person Author { get; set; }

        [Display(Name = "Project Supervisor")]
        public virtual Person Supervisor { get; set; }

        [MaxLength(255, ErrorMessage = "Maximum Character Limit - 255")]
        [Display(Name = "Project Tagline", Prompt = "Enter a tagline for the poject...")]
        public string Tagline { get; set; }

        
        public string Slug { get; set; }

        public string HTMLContent { get; set; }

        [Required]
        [Display(Name = "Publish Date")]
        public DateTime PublishedDateTime { get; set; } = DateTime.Now;

        public Document PrimaryDocument { get; set; }

        //Methods
        public string GetShortDescription
        {
            get
            {
                return Description.HasValue() ? new string(Description.Take(280).ToArray()) + "..." : "";
            }
        }

        public string GetPrimaryDocumentFilePath()
        {
            if (PrimaryDocument == null)
                return "";
            else
                return PrimaryDocument.GetFilePath();
        }

        public string DisplayPublishDate { get { return PublishedDateTime.ToString("d-MMM-yyyy"); } }
        public string DisplayAuthorName { get { return this.Author != null ? this.Author.Name : ""; } }
        public string DisplaySupervisorName { get { return this.Supervisor != null ? this.Supervisor.Name : ""; } }

        public string Slugify() => Slug = Title.AsSlug();

        [Display(Name = "Is Project Active?", Description = "Is this project displayed on the public website?")]
        public bool IsActive { get; set; } = false;

        public static Project CreateProject()
        {
            return new Project()
            {
                PublishedDateTime = DateTime.Now
            };
        }

        public static void DeleteProject<dbcontext>(int id, dbcontext db) where dbcontext : ClevarDbContext
        {
            var project = db.Projects
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (project == null)
                return;

            // Remove Document
            db.Projects.Remove(project);

            // Commit
            db.SaveChanges();
        }
    }
}
