using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClevarWeb.Data.Models.Internals
{
    public abstract class ClevarTableBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime ModifiedDateTime { get; set; } = DateTime.Now;

        public string DisplayCreatedDate { get { return CreatedDateTime.ToString("d-MMM-yyyy"); } }
        public string DisplayCreatedTime { get { return CreatedDateTime.ToString("hh:mm tt"); } }
        public string DisplayCreatedDateTime { get { return DisplayCreatedDate + " " + DisplayCreatedTime; } }

        public string DisplayModifiedDate { get { return ModifiedDateTime.ToString("d-MMM-yyyy"); } }
        public string DisplayModifiedTime { get { return ModifiedDateTime.ToString("hh:mm tt"); } }
        public string DisplayModifiedDateTime { get { return DisplayModifiedDate + " " + DisplayModifiedTime; } }
    }
}
