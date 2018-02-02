using System;
using System.Collections.Generic;
using System.Linq;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extension methods on collections
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Remove all items that match the predicate
        /// </summary>
        /// <typeparam name="T">Type of item to remove</typeparam>
        /// <param name="source">Source collection from which to remove items</param>
        /// <param name="predicate">Predicate that will select items to remove</param>
        /// <returns>Enumerable of items that were removed from the source collection</returns>
        public static IEnumerable<T> RemoveWhere<T>(this ICollection<T> source, Func<T, bool> predicate)
        {
            var itemsToRemove = source
                .Where(predicate)
                .ToList();

            foreach (var item in itemsToRemove)
            {
                source.Remove(item);
            }

            return itemsToRemove;
        }
    }
}
