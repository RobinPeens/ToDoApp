namespace ToDoApp
{
    public static class DateExtensions
    {
        public static string ToNiceString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
