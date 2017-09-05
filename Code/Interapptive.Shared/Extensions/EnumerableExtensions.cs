using System;
using System.Collections.Generic;
using System.Linq;
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
        public static void ThrowIfNotEmpty(this IEnumerable<IResult> source, Func<string, Exception, Exception> createException) =>
            source.Select(x => x.Exception).ThrowIfNotEmpty(createException);

        /// <summary>
        /// Throw a consolidated exception if the collection isn't empty
        /// </summary>
        public static IEnumerable<GenericResult<T>> ThrowIfNotEmpty<T>(this IEnumerable<GenericResult<T>> source, Func<string, Exception, Exception> createException)
        {
            source.OfType<IResult>().ThrowIfNotEmpty(createException);

            return source;
        }

        /// <summary>
        /// Throw a consolidated exception if the collection isn't empty
        /// </summary>
        public static void ThrowIfNotEmpty(this IEnumerable<Exception> source, Func<string, Exception, Exception> createException)
        {
            var exceptions = source
                .Where(x => x != null)
                .ToList();

            if (exceptions.Count == 1)
            {
                throw exceptions.Single();
            }

            if (exceptions.Any())
            {
                throw createException(string.Join(Environment.NewLine, exceptions.Select(x => x.Message)), exceptions.First());
            }
        }
    }
}
