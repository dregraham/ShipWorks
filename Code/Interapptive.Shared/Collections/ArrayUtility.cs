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

        /// <summary>
        /// Finds the indexes of an array within another array.
        /// </summary>
        /// <param name="arrayToSearch">The array in which to search.</param>
        /// <param name="pattern">The array to find.</param>
        /// <param name="startIndex">Initial start position in arrayToSearch.</param>
        /// <returns>List of indexes where pattern starts in arrayToSearch.</returns>
        public static List<int> IndexOfSequence(byte[] arrayToSearch, byte[] pattern, int startIndex)
        {
            List<int> positions = new List<int>();
            int i = Array.IndexOf<byte>(arrayToSearch, pattern[0], startIndex);
            while (i >= 0 && i <= arrayToSearch.Length - 1)
            {
                byte[] segment = new byte[pattern.Length];
                Buffer.BlockCopy(arrayToSearch, i, segment, 0, pattern.Length);
                if (segment.SequenceEqual<byte>(pattern))
                    positions.Add(i);
                i = Array.IndexOf<byte>(arrayToSearch, pattern[0], i + 1);
            }
            return positions;
        }

        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        public static T[] Slice<T>(T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }
    }
}
