using ClevarWeb.Data.Models.Internals;
using ClevarWeb.Data.SampleData;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace ClevarWeb.Data.Models
{
    /// <summary>
    /// Defines a clevar employeed for display on the website
    /// </summary>
    public class Person: ClevarTableBase
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        [Display(Name = "Clevar Person Name", Prompt = "Enter the clevar persons Name")]
        public string Name { get; set; }

        [MaxLength(255)]
        [Display(Name = "Title", Prompt = "Enter the CLEVAR persons Title, eg.. Grand Poobah")]
        public string Title { get; set; }

        [MaxLength(255)]
        [Display(Name = "Sub Title", Prompt = "Enter the CLEVAR persons Sub Title, eg.. Master of the Virtual")]
        public string SubTitle { get; set; }

        [InverseProperty("Person")]
        public ICollection<CartoonImage> CartoonImages { get; set; } = new List<CartoonImage>();

        public Document PrimaryDocument { get; set; }

        [Display(Name = "Bio Content", Prompt = "Write a short Bio for this person...")]
        public string BioHtml { get; set; }

        [Display(Name = "Is Person Active?", Description = "Is this person displayed on the public website?")]
        public bool IsActive { get; set; } = false;

        [InverseProperty("Author")]
        public ICollection<Project> AuthoredProjects { get; set; } = new List<Project>();
        
        [InverseProperty("Supervisor")]
        public ICollection<Project> SupervisedProjects { get; set; } = new List<Project>();

        [InverseProperty("Author")]
        public ICollection<Publication> AuthoredPublications { get; set; } = new List<Publication>();

        #region Methods
        
        public SelectListItem AsSelectListItem => new SelectListItem(this.Name, this.Id.ToString());

        public static Person FromSelectListItem<dbset>(dbset db, SelectListItem selectListItem) where dbset : ClevarDbContext
        {
            return db.People.Where(x => x.Id == int.Parse(selectListItem.Value)).FirstOrDefault();
        }

        public static Person CreatePerson()
        {
            return new Person()
            {
                CreatedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now
            };
        }

        public static void DeletePerson<dbset>(int id, dbset db) where dbset : ClevarDbContext
        {
            var person = db.People
                .Include(x => x.CartoonImages)
                .Include(x => x.AuthoredProjects)
                .Include(x => x.SupervisedProjects)
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (person == null)
                return;

            if(person.CartoonImages.Count > 0)
            {
                db.CartoonImages.RemoveRange(person.CartoonImages);
            }

            foreach (var project in person.AuthoredProjects)
                project.Author = null;

            foreach (var project in person.SupervisedProjects)
                project.Supervisor = null;

            db.People.Remove(person);

            db.SaveChanges();
        }

        #endregion
    }
}
