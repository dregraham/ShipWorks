using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Utility class for working with query strings
    /// </summary>
    public static class QueryStringUtility
    {
        static Regex hexEscapeRegex = new Regex("%[0-9a-zA-Z][0-9a-zA-Z]", RegexOptions.Compiled);

        /// <summary>
        /// Generate a query string of the given variables.  No leading ? or & is included.
        /// </summary>
        public static string GetQueryString(IEnumerable<HttpVariable> variables)
        {
            return GetQueryString(variables, QueryStringEncodingCasing.Default);
        }

        /// <summary>
        /// Generate a query string of the given variables.  No leading ? or & is included.
        /// </summary>
        public static string GetQueryString(IEnumerable<HttpVariable> variables, QueryStringEncodingCasing encodingCasing)
        {
            StringBuilder query = new StringBuilder();

            // Add each variable
            foreach (HttpVariable variable in variables)
            {
                if (query.Length > 0)
                {
                    query.Append("&");
                }

                // Add the name, if present.
                if (variable.Name.Length > 0)
                {
                    query.Append(variable.Name);
                    query.Append("=");
                }

                // Add the value
                query.Append(UrlEncode(variable.Value, encodingCasing));
            }

            return query.ToString();
        }

        /// <summary>
        /// UrlEncode the given value with the specified casing option
        /// </summary>
        private static string UrlEncode(string value, QueryStringEncodingCasing encodingCasing)
        {
            string encoded = HttpUtility.UrlEncode(value);

            if (encodingCasing == QueryStringEncodingCasing.Upper)
            {
                encoded = hexEscapeRegex.Replace(encoded, m => m.Value.ToUpper());
            }

            return encoded;
        }
    }
}
