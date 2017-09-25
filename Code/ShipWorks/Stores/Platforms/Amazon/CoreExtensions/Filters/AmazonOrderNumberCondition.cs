using System;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the Amazon order numbers
    /// </summary>
    [ConditionElement("Amazon Order #", "Amazon.OrderID")]
    [ConditionStoreType(StoreTypeCode.Amazon)]
    public class AmazonOrderNumberCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string amazonOrderSql = String.Empty;
            string amazonOrderSearchSql = String.Empty;

            // Add any combined order AmazonOrderID entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, AmazonOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                amazonOrderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(AmazonOrderSearchFields.AmazonOrderID), context));
            }

            // Add any existing order AmazonOrderID entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, AmazonOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                amazonOrderSql = scope.Adorn(GenerateSql(context.GetColumnReference(AmazonOrderFields.AmazonOrderID), context));
            }

            // OR the two together.
            return $"{amazonOrderSql} OR {amazonOrderSearchSql}";
        }
    }
}
