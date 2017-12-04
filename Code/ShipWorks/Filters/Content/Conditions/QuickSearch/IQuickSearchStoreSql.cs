using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping;
using ShipWorks.Stores;

namespace ShipWorks.Filters.Content.Conditions.QuickSearch
{
    /// <summary>
    /// Generate SQL lines for a quick search for given store.
    /// </summary>
    [Service]
    public interface IQuickSearchStoreSql
    {
        /// <summary>
        /// Generate SQL lines for a quick search for given store.
        /// The result of each line must ONLY be OrderId
        /// </summary>
        IEnumerable<string> GenerateSql(ISqlGenerationBuilder context, string searchText);

        /// <summary>
        /// Store type supported by this IQuickSearchStoreSql
        /// </summary>
        StoreTypeCode StoreType { get; }
    }
}