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
        /// Generate SQL lines for a quick search for given store.
        /// </summary>
        public IEnumerable<string> GenerateSql(SqlGenerationContext context, string searchText)
        {
            context.ColumnsUsed.Add(SearsOrderFields.PoNumber);
            context.RegisterParameter(searchText);

            string paramName = $"@param{context.Parameters.Count}";

            return new[]
            {
                $"SELECT OrderId FROM [SearsOrder] WHERE PoNumber LIKE {paramName}",
                $"SELECT OrderId FROM [SearsOrderSearch] WHERE PoNumber LIKE {paramName}"
            };
        }
    }
}