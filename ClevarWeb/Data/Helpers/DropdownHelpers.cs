using ClevarWeb.Data.SampleData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClevarWeb.Data.Helpers
{
    public static class DropdownHelpers
    {

        #region GetDropdownLists

        public static List<SelectListItem> GetPeopleData<dbset>(dbset db) where dbset : ClevarDbContext
        {
            // Get list of available people for use in selectlist
            var people = db.People.ToList();

            // Setup Select List
            var selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem("", "0"));
            foreach (var person in people)
            {
                selectList.Add(person.AsSelectListItem);
            }

            return selectList;
        }

        #endregion
    }
}
