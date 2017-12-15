using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Groupon.Content
{
    /// <summary>
    /// Generate SQL lines for a quick search for Groupon
    /// </summary>
    public class GrouponQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Store type supported by this IQuickSearchStoreSql
        /// </summary>
        public StoreTypeCode StoreType => StoreTypeCode.Groupon;

        /// <summary>
        /// Generate SQL lines for a quick search for Groupon
        /// </summary>
        public IEnumerable<string> GenerateSql(ISqlGenerationBuilder context, string searchText)
        {
            var orderIDParam = context.RegisterParameter(GrouponOrderFields.GrouponOrderID, searchText);
            var parentIDParam = context.RegisterParameter(GrouponOrderFields.ParentOrderID, searchText);

            return new[]
            {
                $"SELECT OrderId FROM [GrouponOrder] WHERE {GrouponOrderFields.GrouponOrderID.Name} LIKE {orderIDParam} OR {GrouponOrderFields.ParentOrderID.Name} LIKE {parentIDParam}",
                $"SELECT OrderId FROM [GrouponOrderSearch] WHERE {GrouponOrderSearchFields.GrouponOrderID.Name} LIKE {orderIDParam} OR {GrouponOrderFields.ParentOrderID.Name} LIKE {parentIDParam}"
            };
        }
    }
}
