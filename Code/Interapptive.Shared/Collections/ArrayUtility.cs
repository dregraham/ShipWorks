using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Utility functions for working with arrays
    /// </summary>
    public static class ArrayUtility
    {
        /// <summary>
        /// Extract the list of int values from the given string list
        /// </summary>
        public static T[] ParseCommaSeparatedList<T>(string list)
        {
            if (string.IsNullOrEmpty(list))
            {
                return new T[] { };
            }

            return list.Split(',').Select(s => (T) Convert.ChangeType(s, typeof(T))).ToArray();
        }

        /// <summary>
        /// Format the given array as a comma seperated list
        /// </summary>
        public static string FormatCommaSeparatedList<T>(T[] array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T value in array)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }

                sb.Append(value.ToString());
            }

            return sb.ToString();
        }
    }
}
