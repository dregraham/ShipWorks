using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Amazon.Content
{
    /// <summary>
    /// Generate SQL lines for a quick search for given store.
    /// </summary>
    public class AmazonQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Store type supported by this IQuickSearchStoreSql
        /// </summary>
        public StoreTypeCode StoreType => StoreTypeCode.Amazon;

        /// <summary>
        /// Generate SQL lines for a quick search for given store.
        /// The result of each line must ONLY be OrderId
        /// </summary>
        public IEnumerable<string> GenerateSql(ISqlGenerationBuilder context, string searchText)
        {
            string paramName = context.RegisterParameter(AmazonOrderFields.AmazonOrderID, searchText);

            return new[]
            {
                $"SELECT OrderId FROM [AmazonOrder] WHERE AmazonOrderID LIKE {paramName}",
                $"SELECT OrderId FROM [AmazonOrderSearch] WHERE AmazonOrderID LIKE {paramName}"
            };
        }
    }
}