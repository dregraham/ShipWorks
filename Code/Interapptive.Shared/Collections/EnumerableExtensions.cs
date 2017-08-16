using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.UI;
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
        [SuppressMessage("ShipWorks", "SW0002",
            Justification = "nameof is only being used for error information, not binding")]
        public static IEnumerable<IEnumerable<T>> SplitIntoChunksOf<T>(this IEnumerable<T> source, int size)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
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
        [SuppressMessage("ShipWorks", "SW0002",
            Justification = "nameof is only being used for error information, not binding")]
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int count)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
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
        /// Is the count of the collection greater than the specified amount
        /// </summary>
        public static bool IsCountGreaterThan<T>(this IEnumerable<T> source, int count) =>
            source.CompareCountTo(count) == ComparisonResult.More;

        /// <summary>
        /// Is the count of the collection equal to the specified amount
        /// </summary>
        public static bool IsCountEqualTo<T>(this IEnumerable<T> source, int count) =>
            source.CompareCountTo(count) == ComparisonResult.Equal;

        /// <summary>
        /// Returns whether the collection has more, less, or equal to the specified count
        /// </summary>
        public static ComparisonResult CompareCountTo<T>(this IEnumerable<T> source, int count)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));

            ICollection<T> collection = source as ICollection<T>;
            if (collection != null)
            {
                return ConvertIntToComparisonResult(collection.Count.CompareTo(count));
            }

            return CompareCountTo(source as IEnumerable, count);
        }

        /// <summary>
        /// Returns whether the collection has more, less, or equal to the specified count
        /// </summary>
        public static ComparisonResult CompareCountTo(this IEnumerable source, int count)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));

            ICollection collection = source as ICollection;
            if (collection != null)
            {
                return ConvertIntToComparisonResult(collection.Count.CompareTo(count));
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
                            return ComparisonResult.More;
                        }
                    }
                }
                finally
                {
                    (enumerator as IDisposable)?.Dispose();
                }
            }

            return num == count ? ComparisonResult.Equal : ComparisonResult.Less;
        }

        /// <summary>
        /// Performs the specified action for each item in source
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(action, nameof(action));

            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Convert an int to a comparison result
        /// </summary>
        private static ComparisonResult ConvertIntToComparisonResult(int result)
        {
            return result < 0 ? ComparisonResult.Less :
                result > 0 ? ComparisonResult.More :
                ComparisonResult.Equal;
        }

        /// <summary>
        /// Compare whether two sequences are equal, regardless of ordering
        /// </summary>
        public static bool UnorderedSequenceEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            ILookup<T, T> list1Lookup = list1.ToLookup(i => i);
            ILookup<T, T> list2Lookup = list2.ToLookup(i => i);

            return list1Lookup.Count == list2Lookup.Count &&
                list1Lookup.All(g => g.Count() == list2Lookup[g.Key].Count());
        }

        /// <summary>
        /// Inject a default value into the enumerable if it's empty or null
        /// </summary>
        public static IEnumerable<T> DefaultIfEmptyOrNull<T>(this IEnumerable<T> source) =>
            source.DefaultIfEmptyOrNull(default(T));

        /// <summary>
        /// Inject a default value into the enumerable if it's empty or null
        /// </summary>
        public static IEnumerable<T> DefaultIfEmptyOrNull<T>(this IEnumerable<T> source, T defaultValue) =>
            (source ?? Enumerable.Empty<T>()).DefaultIfEmpty(defaultValue);

        /// <summary>
        /// Perform a left join on an enumerable
        /// </summary>
        public static IEnumerable<Tuple<TLeft, TRight>> LeftJoin<TLeft, TRight, TKey>(this IEnumerable<TLeft> left,
            IEnumerable<TRight> right, Func<TLeft, TKey> getLeftKey, Func<TRight, TKey> getRightKey) =>
            left.GroupJoin(right, getLeftKey, getRightKey, (x, y) => Tuple.Create(x, y.FirstOrDefault()));

        /// <summary>
        /// Perform an async select, showing a progress dialog
        /// </summary>
        [NDependIgnoreTooManyParams]
        public static async Task<IEnumerable<TResult>> SelectWithProgress<T, TResult>(this IEnumerable<T> source,
            IMessageHelper messageHelper, string title, string description, string detailFormat,
            Func<T, Task<TResult>> processItem)
        {
            var results = new List<TResult>();

            using (var progress = messageHelper.ShowProgressDialog(title, description))
            {
                var updater = progress.ToUpdater(source, detailFormat);

                foreach (var key in source.TakeWhile(x => !progress.ProgressItem.IsCancelRequested))
                {
                    updater.Update();
                    results.Add(await processItem(key).ConfigureAwait(false));
                }
            }

            return results;
        }

        /// <summary>
        /// Append an item to an enumerable
        /// </summary>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));

            return AppendInternal(source, item);
        }

        /// <summary>
        /// Append an item to an enumerable
        /// </summary>
        private static IEnumerable<T> AppendInternal<T>(IEnumerable<T> source, T item)
        {
            using (IEnumerator<T> iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    yield return iter.Current;
                }
            }

            yield return item;
        }
    }
}
