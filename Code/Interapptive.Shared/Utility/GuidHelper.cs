using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Helper class for parsing guids
    /// </summary>
    public static class GuidHelper
    {
        static Regex format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$",
                RegexOptions.Compiled);

        /// <summary>
        /// Converts the string representation of a Guid to its Guid 
        /// equivalent. A return value indicates whether the operation 
        /// succeeded. 
        /// </summary>
        public static bool TryParse(string value, out Guid guid)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Match match = format.Match(value);

            if (match.Success)
            {
                guid = new Guid(value);
                return true;
            }
            else
            {
                guid = Guid.Empty;
                return false;
            }
        }

        /// <summary>
        /// Indicates if the given string is a valid representation of a guid.
        /// </summary>
        public static bool IsGuid(string previous)
        {
            Guid guid;
            return TryParse(previous, out guid);
        }
    }
}
