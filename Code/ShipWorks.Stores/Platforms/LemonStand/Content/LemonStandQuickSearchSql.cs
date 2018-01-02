using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.LemonStand.Content
{
    /// <summary>
    /// Generate SQL lines for a quick search for LemonStand
    /// </summary>
    public class LemonStandQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Store type supported by this IQuickSearchStoreSql
        /// </summary>
        public StoreTypeCode StoreType => StoreTypeCode.LemonStand;

        /// <summary>
        /// Generate SQL lines for a quick search for LemonStand
        /// </summary>
        public IEnumerable<string> GenerateSql(ISqlGenerationBuilder context, string searchText)
        {
            var customOrderIdentifierParam = context.RegisterParameter(LemonStandOrderFields.LemonStandOrderID, searchText);

            return new[]
            {
                $"SELECT OrderId FROM [LemonStandOrder] WHERE { LemonStandOrderFields.LemonStandOrderID.Name } LIKE { customOrderIdentifierParam }",
                $"SELECT OrderId FROM [LemonStandOrderSearch] WHERE { LemonStandOrderSearchFields.LemonStandOrderID.Name } LIKE { customOrderIdentifierParam }"
            };
        }
    }
}
