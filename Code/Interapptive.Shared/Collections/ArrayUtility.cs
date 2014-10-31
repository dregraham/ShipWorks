using System;
using System.Collections.Generic;
using System.Globalization;
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

            return list.Split(',')
                .Select(s => ChangeType(s, typeof(T)))
                .OfType<T>()
                .ToArray();
        }

        /// <summary>
        /// Try to change the type of an object, returning null if the change fails
        /// </summary>
        private static object ChangeType(object fromValue, Type toType)
        {
            try
            {
                return Convert.ChangeType(fromValue, toType, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return null;
            }
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

                sb.Append(value);
            }

            return sb.ToString();
        }
    }
}
