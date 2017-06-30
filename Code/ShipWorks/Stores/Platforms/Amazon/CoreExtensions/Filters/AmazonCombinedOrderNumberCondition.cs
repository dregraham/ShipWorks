using System;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the Amazon order numbers that were combined into a single order
    /// </summary>
    [ConditionElement("Amazon Order #", "Amazon.CombinedOrderNumbers")]
    [ConditionStoreType(StoreTypeCode.Amazon)]
    [Component]
    public class AmazonCombinedOrderNumberCondition : NumericStringCondition<long>, ICombinedOrderCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string amazonOrderSearchSql = String.Empty;

            EntityField2 searchField = IsNumeric ? AmazonOrderSearchFields.OrderNumber : AmazonOrderSearchFields.OrderNumberComplete;

            // Add any combined order number/number complete entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, AmazonOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                amazonOrderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(searchField), context));
            }

            return amazonOrderSearchSql;
        }
    }
}
