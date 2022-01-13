using System.Text.RegularExpressions;
using System.Text;

namespace ClevarWeb
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (value.Length > length)
                value = value.Substring(0, length);

            return value;
        }

        public static bool HasValue(this string value) => !string.IsNullOrWhiteSpace(value);

        public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);

        public static string AsSlug(this string phrase, int maxLength = 255)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
