namespace Interapptive.Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNumeric(this string input)
        {
            long value;
            return long.TryParse(input, out value);
        }
    }
}