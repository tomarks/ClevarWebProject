using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClevarWeb.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClevarWeb
{
    public class AdminIndexModel : PageModel
    {
        private readonly ClevarWeb.Data.ClevarDbContext db;


        public AdminIndexModel(ClevarWeb.Data.ClevarDbContext context)
        {
            db = context;
        }

        public void OnGet()
        {
            SystemProperties.Instance.GetIsDbConnected(db);
        }
    }
}