using ClevarWeb.Data.Models.Internals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClevarWeb.Data.Models
{
    public class Publication: ClevarTableBase
    {
        [Required]
        [MaxLength(255)]
        [Display(Name = "Publication Name", Prompt = "Enter the publication name...")]
        public string PublicationName { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Publication URL", Prompt = "Enter the publications URL...")]
        [RegularExpression(@"^https?:\/\/.*", ErrorMessage = "Invalid URL - Must start with http:// or https://")]
        public string PublicationUrl { get; set; }

        [Display(Name = "Publication Author")]
        public virtual Person Author { get; set; }

        [Required]
        [Display(Name = "Publish Date")]
        public DateTime PublishedDateTime { get; set; } = DateTime.Now;

        public Document PrimaryDocument { get; set; }

        #region Methods

        public string DisplayAuthorName { get { return this.Author != null ? this.Author.Name : ""; } }
        public string DisplayPublishDate { get { return PublishedDateTime.ToString("d-MMM-yyyy"); } }

        public static Publication CreatePublication()
        {
            return new Publication()
            {
                PublishedDateTime = DateTime.Now
            };
        }

        public static void DeletePublication<dbcontext>(int id, dbcontext db) where dbcontext : ClevarDbContext
        {
            var publication = db.Publications
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (publication == null)
                return;

            // Remove Publication
            db.Publications.Remove(publication);

            // Commit
            db.SaveChanges();
        }

        #endregion
    }
}
