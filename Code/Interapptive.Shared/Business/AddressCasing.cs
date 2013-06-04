using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Helps convert a string to proper address casing
    /// </summary>
    public static class AddressCasing
    {
        static Regex regex = new Regex(@"(\w+)", RegexOptions.Compiled);

        /// <summary>
        /// Apply Address Casing to the given text
        /// </summary>
        public static string Apply(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return regex.Replace(text, new MatchEvaluator(ProperCasingEvaluator));
        }

        /// <summary>
        /// Callback for ProperCasing regex.
        /// </summary>
        private static string ProperCasingEvaluator(Match match)
        {
            // First convert to lower case
            string lower = match.Value.ToLower();

            // Now caps the first letter
            string result = char.ToUpper(lower[0]) + ((lower.Length > 1) ? lower.Substring(1) : "");

            return result;
        }
    }
}
