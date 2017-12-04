using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Sears.Content
{
    /// <summary>
    /// Generate SQL lines for a quick search for given store.
    /// </summary>
    public class SearsQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Store type supported by this IQuickSearchStoreSql
        /// </summary>
        public StoreTypeCode StoreType => StoreTypeCode.Sears;

        /// <summary>
        /// Generate SQL lines for a quick search for given store.
        /// </summary>
        public IEnumerable<string> GenerateSql(ISqlGenerationBuilder context, string searchText)
        {
            string paramName = context.RegisterParameter(SearsOrderFields.PoNumber, searchText);

            return new[]
            {
                $"SELECT OrderId FROM [SearsOrder] WHERE PoNumber LIKE {paramName}",
                $"SELECT OrderId FROM [SearsOrderSearch] WHERE PoNumber LIKE {paramName}"
            };
        }
    }
}