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
        /// Throw a consolidated exception if the collection isn't empty
        /// </summary>
        public static void ThrowIfNotEmpty(this IEnumerable<IResult> results, Func<string, Exception, Exception> createException) =>
            results.Select(x => x.Exception).ThrowIfNotEmpty(createException);

        /// <summary>
        /// Throw a consolidated exception if the collection isn't empty
        /// </summary>
        public static void ThrowIfNotEmpty(this IEnumerable<Exception> results, Func<string, Exception, Exception> createException)
        {
            var exceptions = results
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
