using Interapptive.Shared.Utility;
using Newtonsoft.Json;

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

        /// <summary>
        /// Try deserializing JSON to an object
        /// </summary>
        public static bool TryParseJson<T>(this string input, out T result)
        {
            if (input.IsNullOrWhiteSpace())
            {
                result = default(T);
                return false;
            }

            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(input, settings);
            return success && result != null;
        }
    }
}