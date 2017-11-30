using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Enumerable extensions
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Get values from successful results
        /// </summary>
        public static IEnumerable<T> GetSuccessfulValues<T>(this IEnumerable<GenericResult<T>> source) =>
            source.Where(x => x.Success && x.Value != null).Select(x => x.Value);

        /// <summary>
        /// Throw a consolidated exception if the collection isn't empty
        /// </summary>
        public static IList<T> ThrowFailures<T>(this IEnumerable<T> source, Func<string, Exception, Exception> createException) where T : IResult
        {
            var sourceAsList = source as List<T> ?? source.ToList();

            sourceAsList.Select(x => x.Exception).ThrowExceptions(createException);

            return sourceAsList;
        }

        /// <summary>
        /// Throw a consolidated exception if the collection isn't empty
        /// </summary>
        public static void ThrowExceptions<T>(this IEnumerable<T> source, Func<string, T, Exception> createException) where T : Exception
        {
            var sourceAsList = source as List<T> ?? source.ToList();
            var exceptions = sourceAsList.Where(x => x != null);

            if (exceptions.Count() == 1)
            {
                throw exceptions.Single();
            }

            if (exceptions.Any())
            {
                throw createException(string.Join(Environment.NewLine, exceptions.Select(x => x.Message)), exceptions.First());
            }
        }

        /// <summary>
        /// Applies an accumulator function over a sequence 
        /// </summary>
        public static GenericResult<TResult> Aggregate<T, TResult>(this IEnumerable<T> source, TResult accumulator, Func<TResult, T, GenericResult<TResult>> aggregator) =>
            Enumerable.Aggregate(source,
                GenericResult.FromSuccess(accumulator),
                (acc, item) => acc.Bind(v => aggregator(v, item)));

        /// <summary>
        /// Applies an accumulator function over a sequence 
        /// </summary>
        public static GenericResult<IEnumerable<T>> Flatten<T>(this IEnumerable<GenericResult<T>> source) =>
            Enumerable.Aggregate(source,
                GenericResult.FromSuccess(Enumerable.Empty<T>()),
                (acc, item) => acc.Bind(v => item.Map(i => v.Append(i))));

        /// <summary>
        /// Match on a dictionary
        /// </summary>
        /// <returns>
        /// Success response with the result of the onFound method, or else the value of the onMissing method
        /// </returns>
        public static GenericResult<T> Match<T, TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue, T> onFound, Func<Exception> onMissing) =>
            source.Match(key, x => onFound(x), () => GenericResult.FromError<T>(onMissing()));

        /// <summary>
        /// Match on a dictionary
        /// </summary>
        /// <returns>
        /// Success response with the result of the onFound method, or else the value of the onMissing method
        /// </returns>
        public static T Match<T, TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue, T> onFound, Func<T> onMissing) =>
            source.ContainsKey(key) ? onFound(source[key]) : onMissing();
    }
}
