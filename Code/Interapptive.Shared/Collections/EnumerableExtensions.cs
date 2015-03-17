using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Extension methods for the Enumerable type
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Breaks a collection up into chunks of the selected size
        /// </summary>
        /// <typeparam name="T">Type of data in the collection</typeparam>
        /// <param name="source">Collection that will be broken into chunks</param>
        /// <param name="size">Size of each chunk</param>
        /// <returns>Collection of chunks of the specified type</returns>
        public static IEnumerable<IEnumerable<T>> SplitIntoChunksOf<T>(this IEnumerable<T> source, int size)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            using (IEnumerator<T> iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    var chunk = new List<T>(size) { iter.Current };

                    for (int i = 1; i < size && iter.MoveNext(); i++)
                    {
                        chunk.Add(iter.Current);
                    }
                    
                    yield return chunk;
                }
            }
        }

        /// <summary>
        /// Repeats the items in the collection
        /// </summary>
        /// <typeparam name="T">Type of object in the collection</typeparam>
        /// <param name="source">Collection to repeat</param>
        /// <param name="count">Number of times the collection will be repeated</param>
        /// <returns></returns>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int count)
        {
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }
                
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            
            return RepeatIterator(source, count);
        }

        /// <summary>
        /// Repeats the items in the collection
        /// </summary>
        /// <typeparam name="T">Type of object in the collection</typeparam>
        /// <param name="source">Collection to repeat</param>
        /// <param name="count">Number of times the collection will be repeated</param>
        /// <returns></returns>
        private static IEnumerable<T> RepeatIterator<T>(this IEnumerable<T> source, int count)
        {
            while (0 < count--)
            {
                foreach (var item in source)
                {
                    yield return item;        
                }
            }
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

        /// <summary>
        /// Excludes the other collection from the source, using the specified property for comparison
        /// </summary>
        public static IEnumerable<T> Except<T, TProp>(this IEnumerable<T> source, IEnumerable<T> other, Func<T, TProp> propertyAccessor) where T : class
        {
            return source.Except(other, new GenericPropertyEqualityComparer<T, TProp>(propertyAccessor));
        }
    }
}
