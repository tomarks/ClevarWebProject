using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;
using ClevarWeb.Utility;

namespace ClevarWeb.Pages.Admin
{
    public class SystemSettingsModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;

        public SystemSettingsModel(ClevarWeb.Data.ClevarDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public SystemSetting SystemSetting { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SystemSetting = await db.SystemSettings.FirstOrDefaultAsync();

            // Setup Default Site Settings
            if (SystemSetting == null)
            {
                var defaultSettings = new SystemSetting()
                {
                    SiteTitle = "CLEVAR",
                    SiteKeywords = "virtual, reality, augmented, vr, ar, research",
                    PublicEmailAddress = "admin@clevargroup.com.au",
                    SocialFacebookURL = @"http://www.facebook.com/clevargroup",
                    SocialInstagramURL = @"http://www.instagram.com/clevargroup",
                    SocialLinkedinURL = @"http://www.linkedin.com/clevargroup",
                    SocialTwitterURL = @"http://www.twitter.com/clevargroup"
                };

                db.SystemSettings.Add(defaultSettings);
                db.SaveChanges();

                SystemSetting = db.SystemSettings.First();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.Attach(SystemSetting).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                SystemProperties.Instance.SystemSettings = db.SystemSettings.First();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemSettingExists(SystemSetting.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Page();
        }

        private bool SystemSettingExists(int id) => db.SystemSettings.Any(e => e.Id == id);

    }
}
