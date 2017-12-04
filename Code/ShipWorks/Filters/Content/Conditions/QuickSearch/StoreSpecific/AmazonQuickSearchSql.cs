using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.QuickSearch.StoreSpecific
{
    /// <summary>
    /// Generate SQL lines for a quick search for given store.
    /// </summary>
    public class AmazonQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Generate SQL lines for a quick search for given store.
        /// </summary>
        public IEnumerable<string> GenerateSql(SqlGenerationContext context, string searchText)
        {
            context.ColumnsUsed.Add(AmazonOrderFields.AmazonOrderID);
            context.RegisterParameter(searchText);

            string paramName = $"@param{context.Parameters.Count}";

            return new[]
            {
                $"SELECT OrderId FROM [AmazonOrder] WHERE AmazonOrderID LIKE {paramName}",
                $"SELECT OrderId FROM [AmazonOrderSearch] WHERE AmazonOrderID LIKE {paramName}"
            };
        }
    }
}