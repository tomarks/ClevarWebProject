using System.Collections.Generic;

namespace ClevarWeb
{
    public static class ListExtensions
    {
        public static bool IsEmpty<T>(this List<T> list) => list == null || list.Count == 0;

        public static bool HasItems<T>(this List<T> list) => list != null && list.Count > 0;
    }
}
