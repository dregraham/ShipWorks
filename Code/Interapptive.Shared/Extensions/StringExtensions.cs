namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// String type extension methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks whether the string value is numeric
        /// </summary>
        public static bool IsNumeric(this string input)
        {
            long value;
            return long.TryParse(input, out value);
        }

        /// <summary>
        /// Returns null if the input is an empty string
        /// </summary>
        public static string NullIfEmpty(this string input) =>
            input == string.Empty ? null : input;
    }
}