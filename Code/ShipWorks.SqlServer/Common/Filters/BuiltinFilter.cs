using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Utility class for working with the filters that are builtin to ShipWorks
    /// </summary>
    public static class BuiltinFilter
    {
        readonly static Dictionary<FilterTarget, long> topLevelLeys = new Dictionary<FilterTarget, long>();
        readonly static Dictionary<FilterTarget, long> searchKeys = new Dictionary<FilterTarget, long>();

        /// <summary>
        /// Static constructor
        /// </summary>
        static BuiltinFilter()
        {
            topLevelLeys[FilterTarget.Orders] = -26;
            topLevelLeys[FilterTarget.Customers] = -28;

            searchKeys[FilterTarget.Orders] = -13;
            searchKeys[FilterTarget.Customers] = -23;
        }

        /// <summary>
        /// Indicates if the given FilterTarget has Layout, which mean's the user can organize the Filter Tree for it, and view
        /// the contents in a grid.
        /// </summary>
        public static bool HasTopLevelKey(FilterTarget target)
        {
            return topLevelLeys.Keys.Any(key => key == target);
        }

        /// <summary>
        /// Get the hard-coded primary key value used for the given builtin filter target
        /// </summary>
        public static long GetTopLevelKey(FilterTarget target)
        {
            long key;
            if (topLevelLeys.TryGetValue(target, out key))
            {
                return key;
            }

            throw new InvalidOperationException(string.Format("Filter Target {0} has no top level key.", target));
        }

        /// <summary>
        /// Indicates if the given id represents the key for a top-level filter.
        /// </summary>
        public static bool IsTopLevelKey(long id)
        {
            return topLevelLeys.Values.Any(key => key == id);
        }

        /// <summary>
        /// The hard-coded primary key of the builtin filter data for search
        /// </summary>
        public static long GetSearchPlaceholderKey(FilterTarget target)
        {
            long key;
            if (searchKeys.TryGetValue(target, out key))
            {
                return key;
            }

            throw new InvalidOperationException("Unhandled Filter Target value.");
        }

        /// <summary>
        /// Indicates if the given id represents a search placeholder
        /// </summary>
        public static bool IsSearchPlaceholderKey(long id)
        {
            return searchKeys.Values.Any(key => key == id);
        }
    }
}
