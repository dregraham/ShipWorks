using System;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Condition for the NetworkSolutions order numbers that were combined into a single order
    /// </summary>
    [ConditionElement("Network Solutions Order #", "NetworkSolutions.CombinedOrderNumbers")]
    [ConditionStoreType(StoreTypeCode.NetworkSolutions)]
    [Component]
    public class NetworkSolutionsCombinedOrderNumberCondition : NumericStringCondition<long>, ICombinedOrderCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSearchSql = String.Empty;

            EntityField2 searchField = IsNumeric ? NetworkSolutionsOrderSearchFields.OrderNumber : NetworkSolutionsOrderSearchFields.OrderNumberComplete;

            // Add any combined order number/number complete entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, NetworkSolutionsOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(searchField), context));
            }

            return orderSearchSql;
        }
    }
}
