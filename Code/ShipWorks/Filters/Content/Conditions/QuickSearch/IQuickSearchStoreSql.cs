using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Filters.Content.SqlGeneration;

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
        IEnumerable<string> GenerateSql(SqlGenerationContext context, string searchText);
    }
}