namespace ClevarWeb
{
    public static class DateTimeExtensions
    {
        public static string ToString(this System.DateTime? dt, string format) => !dt.HasValue ? null : dt.Value.ToString(format);
    }
}
