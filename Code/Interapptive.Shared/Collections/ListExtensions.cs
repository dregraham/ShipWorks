using System.Collections.Generic;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Extension methods for the List class
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Add a range of items to a collection
        /// </summary>
        public static void AddRange<T>(this List<T> source, params T[] items) => source.AddRange(items);
    }
}
