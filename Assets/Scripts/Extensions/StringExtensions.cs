namespace Extensions
{
    public static class StringExtensions
    {
        
        public static string StripQuotes(this string str)
        {
            return str.Replace("\"", "");
        }
    }
}