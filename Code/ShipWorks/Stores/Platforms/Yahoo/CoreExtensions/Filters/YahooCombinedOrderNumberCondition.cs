﻿using System;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Yahoo.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Condition for the Yahoo order numbers that were combined into a single order
    /// </summary>
    [ConditionElement("Yahoo Order #", "Yahoo.CombinedOrderNumbers")]
    [ConditionStoreType(StoreTypeCode.Yahoo)]
    [Component]
    public class YahooCombinedOrderNumberCondition : NumericStringCondition<long>, ICombinedOrderCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSearchSql = String.Empty;

            EntityField2 searchField = IsNumeric ? YahooOrderSearchFields.OrderNumber : YahooOrderSearchFields.OrderNumberComplete;

            // Add any combined order number/number complete entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, YahooOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(searchField), context));
            }

            return orderSearchSql;
        }
    }
}
