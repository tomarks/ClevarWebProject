using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClevarWeb.Utility
{
    public sealed class General
    {
        private static readonly Lazy<General> lazy = new Lazy<General>(() => new General());
        public static General Instance { get { return lazy.Value; } }
        private General()
        {
        }

        public string wwwRootPath { get; set; }

        public void WriteToLog(string text, LogLevel level = LogLevel.Information)
        {
            var msg = $"[{level.ToString()}] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $"\n{text}\n\n";
            try
            {
                Console.Write(msg);
                Trace.Write(msg);
                File.AppendAllTextAsync(Path.Combine(General.Instance.wwwRootPath, "log.txt"), msg);
            }
            catch (Exception) {}
        }

        public void WriteToLog(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var body = reader.ReadToEndAsync();
                Instance.WriteToLog(body.Result);
            }
        }
    }
}
