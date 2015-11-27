using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Utility;
using System.Collections.ObjectModel;
using System.Collections;

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
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            return SplitIntoChunksOfImplementation(source, size);
        }
        /// <summary>
        /// Breaks a collection up into chunks of the selected size
        /// </summary>
        /// <typeparam name="T">Type of data in the collection</typeparam>
        /// <param name="source">Collection that will be broken into chunks</param>
        /// <param name="size">Size of each chunk</param>
        /// <returns>Collection of chunks of the specified type</returns>
        private static IEnumerable<IEnumerable<T>> SplitIntoChunksOfImplementation<T>(IEnumerable<T> source, int size)
        {
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
        /// Breaks a collection up into chunks where the sum of the specified value is less than the maxValue
        /// </summary>
        /// <typeparam name="TSource">Type of data in the collection</typeparam>
        /// <param name="source">Collection that will be broken into chunks</param>
        /// <param name="size">Max value for each chunk</param>
        /// <returns>Collection of chunks of the specified type</returns>
        public static IEnumerable<IEnumerable<TSource>> SplitIntoChunks<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double maxValue)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(selector, nameof(selector));

            // Add a little extra to handle the fact that doubles aren't exactly accurate
            return SplitIntoChunksImplementation(source, selector, maxValue + 0.01);
        }

        /// <summary>
        /// Breaks a collection up into chunks where the sum of the specified value is less than the maxValue
        /// </summary>
        /// <typeparam name="TSource">Type of data in the collection</typeparam>
        /// <param name="source">Collection that will be broken into chunks</param>
        /// <param name="size">Max value for each chunk</param>
        /// <returns>Collection of chunks of the specified type</returns>
        public static IEnumerable<IEnumerable<TSource>> SplitIntoChunksImplementation<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double maxValue)
        {
            using (IEnumerator<TSource> iter = source.GetEnumerator())
            {
                double currentValue = 0;
                List<TSource> items = new List<TSource>();

                while (iter.MoveNext())
                {
                    TSource item = iter.Current;
                    double selectedValue = selector(item);

                    if (items.Any())
                    {
                        if (currentValue + selectedValue < maxValue)
                        {
                            items.Add(item);
                            currentValue += selectedValue;
                        }
                        else
                        {
                            yield return items;
                            items = new List<TSource>() { item };
                            currentValue = selectedValue;
                        }
                    }
                    else
                    {
                        items.Add(item);
                        currentValue += selectedValue;
                    }
                }

                yield return items;
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
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));

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

        /// <summary>
        /// Are there no items in the collection
        /// </summary>
        public static bool None<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }

        /// <summary>
        /// Are there no items in the collection that match the given predicate
        /// </summary>
        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return !source.Any(predicate);
        }

        /// <summary>
        /// Create a ReadOnlyCollection from the given enumerable
        /// </summary>
        public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> source) =>
            new ReadOnlyCollection<T>(source?.ToList() ?? new List<T>());

        /// <summary>
        /// Returns whether the collection has more, less, or equal to the specified count
        /// </summary>
        public static int HasMoreOrLessThanCount<T>(this IEnumerable<T> source, int count)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));

            ICollection<T> collection = source as ICollection<T>;
            if (collection != null)
            {
                return collection.Count.CompareTo(count);
            }

            return HasMoreOrLessThanCount(source as IEnumerable, count);
        }

        /// <summary>
        /// Returns whether the collection has more, less, or equal to the specified count
        /// </summary>
        public static int HasMoreOrLessThanCount(this IEnumerable source, int count)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));

            ICollection collection = source as ICollection;
            if (collection != null)
            {
                return collection.Count.CompareTo(count);
            }

            // It's not a collection, so enumerate only as long as necessary
            int num = 0;
            checked
            {
                IEnumerator enumerator = null;

                try
                {
                    enumerator = source.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        num++;
                        if (num > count)
                        {
                            return 1;
                        }
                    }
                }
                finally
                {
                    (enumerator as IDisposable)?.Dispose();
                }
            }

            return num == count ? 0 : -1;
        }
    }
}
