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
        public static IEnumerable<T> ThrowFailures<T>(this IEnumerable<T> source, Func<string, Exception, Exception> createException) where T : IResult
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
    }
}
