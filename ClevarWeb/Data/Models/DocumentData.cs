using ClevarWeb.Data.Models.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClevarWeb.Data.Models
{
    public class DocumentData : ClevarTableBase
    {
        public byte[] FileData { get; set; }
    }
}
