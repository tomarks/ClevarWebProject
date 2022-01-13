using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;
using ClevarWeb.Data.SampleData;
using ClevarWeb.Utility;
using ClevarWeb.Data.Helpers;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ClevarWeb.Pages.Admin.People
{
    [BindProperties]
    public class EditCartoonModel : PageModel
    {
        private readonly ClevarDbContext db;
        private readonly IMapper mapper;

        public List<SelectListItem> StylesSelectListItems;

        public EditCartoonModel(ClevarDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;

            StylesSelectListItems = new List<SelectListItem>();

            StylesSelectListItems.Add(new SelectListItem() { Text = "", Value = "" });
            StylesSelectListItems.Add(new SelectListItem() { Text = "Float", Value = "clevar-animate-float" });
            StylesSelectListItems.Add(new SelectListItem() { Text = "Glow", Value = "clevar-static-glow" });
        }

        /// <summary>
        /// Page Model
        /// </summary>
        public List<CartoonImage> CartoonImages { get; set; }
        public int personId { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Setup Model
            if (id == null)
                return NotFound();


            var person = await db.People.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
                return NotFound();

            personId = person.Id;

            var cartoonImages = await db.CartoonImages.Where(x => x.Person.Id == person.Id).Include(x => x.Document).ToListAsync();

            //If cartoon Images is empty then we create a new list in the page model with 1 item in it and a default Character.
            if (cartoonImages.Count == 0)
            {
                var newCartoonImage = CartoonImage.CreateCartoonImage(1);
                newCartoonImage.Person = person;
                db.CartoonImages.Add(newCartoonImage);
                cartoonImages.Add(newCartoonImage);
                db.SaveChanges();
            }

            CartoonImages = cartoonImages;

            // Automap matching properties
            //mapper.Map(person, this);

            // Return
            return Page();
        }

        [Display(Name = "Upload New Bio Image")]
        public IFormFile UploadedFile { get; set; }
        public async Task<JsonResult> OnPostUploadFileAsync()
        {
            if (UploadedFile == null || this.personId == 0)
                return new JsonResult(new { error = "Unable to match ID or find an Uploaded File." });

            var ms = new MemoryStream();
            UploadedFile.OpenReadStream().CopyTo(ms);
            var person = db.People
                .Include(x => x.CartoonImages)
                .Where(x => x.Id == this.personId).First();
            var newDocument = Document.CreateDocument(UploadedFile.FileName, ms.ToArray());

            db.Documents.Add(newDocument);

            var lastLayerNumber = 1;
            if (person.CartoonImages.Any())
                lastLayerNumber = person.CartoonImages.Select(x => x.LayerNumber).Max() + 1;

            var newCartoonImage = CartoonImage.CreateCartoonImage(lastLayerNumber);
            newCartoonImage.Document = newDocument;
            newCartoonImage.Person = person;

            db.CartoonImages.Add(newCartoonImage);
            await db.SaveChangesAsync();

            return new JsonResult(
                new
                {
                    name = newDocument.FileName,
                    path = newDocument.GetFilePath()
                });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int CartoonID)
        {
            var cartoon = db.CartoonImages.Include(x => x.Person).Where(x => x.Id == CartoonID).FirstOrDefault();
            if (cartoon == null)
                return Page();

            var personid = cartoon.Person.Id;
            db.CartoonImages.Remove(cartoon);
            await db.SaveChangesAsync();

            var remainingCartoonsToReOrder = db.CartoonImages.Where(x => x.Person.Id == personid).ToList();

            if (remainingCartoonsToReOrder.Count > 0)
            {
                for (int i = 0; i < remainingCartoonsToReOrder.Count; i++)
                {
                    remainingCartoonsToReOrder[i].LayerNumber = i + 1;
                }

                await db.SaveChangesAsync();
            }

            return RedirectToPage("/Admin/People/EditCartoon", new { id = personid });
        }


        public async Task<IActionResult> OnPostSaveAsync(string SaveAndContinue)
        {
            var person = db.People.Where(x => x.Id == this.personId).First();

            person.CartoonImages = this.CartoonImages;


            //mapper.Map(this, person);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.Attach(person).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(person.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            switch (SaveAndContinue)
            {
                case "Save":
                    return RedirectToPage("/Admin/People/EditCartoon", new { id = person.Id });
                case "EditBio":
                    return RedirectToPage("/Admin/People/Edit", new { id = person.Id });
                case "Preview":
                    return RedirectToPage("/Admin/People/Details", new { id = person.Id });
                default:
                    return RedirectToPage("./Index");
            }

        }

        private bool PersonExists(int id) => db.People.Any(x => x.Id == id);

    }
    public class OrderingAction
    {
        public string action { get; set; }
        public int? cartoonid { get; set; }
        public int? personid { get; set; }
    }
}
