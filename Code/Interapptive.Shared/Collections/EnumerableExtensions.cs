using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace Interapptive.Shared.Collections
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int count)
        {
            if (null == source)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return RepeatIterator(source, count);
        }

        static IEnumerable<T> RepeatIterator<T>(this IEnumerable<T> source, int count)
        {
            while (0 < count--)
                foreach (var item in source)
                    yield return item;
        }

        /// <summary>
        /// Combines a collection of strings
        /// </summary>
        /// <param name="source">Collection of strings to Combine</param>
        /// <returns>Combined string</returns>
        public static string Combine(this IEnumerable<string> source)
        {
            return Combine(source, string.Empty);
        }

        /// <summary>
        /// Combines a collection of strings using the specified separator
        /// </summary>
        /// <param name="source">Collection of strings to Combine</param>
        /// <param name="separator">String that will be used between each string in the collection</param>
        /// <returns></returns>
        public static string Combine(this IEnumerable<string> source, string separator)
        {
            return source == null || !source.Any() ? string.Empty : source.Aggregate((x, y) => x + separator + y);
        }

        /// <summary>
        /// Combines a collection of chars
        /// </summary>
        /// <param name="source">Collection of chars to Combine</param>
        /// <returns>Combined string</returns>
        public static string Combine(this IEnumerable<char> source)
        {
            return Combine(source, string.Empty);
        }

        /// <summary>
        /// Combines a collection of chars using the specified separator
        /// </summary>
        /// <param name="source">Collection of chars to Combine</param>
        /// <param name="separator">String that will be used between each string in the collection</param>
        /// <returns></returns>
        public static string Combine(this IEnumerable<char> source, string separator)
        {
            return Combine(source.Select(x => x.ToString(CultureInfo.InvariantCulture)), separator);
        }
    }
}
