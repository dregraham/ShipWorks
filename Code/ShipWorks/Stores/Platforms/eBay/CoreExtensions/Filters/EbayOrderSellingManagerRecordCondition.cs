using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for testing Checkout Status of an eBay order item.
    /// </summary>
    [ConditionElement("eBay Sales Record #", "EbayOrder.SellingManagerRecord")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayOrderSellingManagerRecordCondition : NumericStringCondition<long>
    {
        /// <summary>
        /// Generate the SQL that evaluates the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, EbayOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(base.GenerateSql(context.GetColumnReference(EbayOrderFields.SellingManagerRecord), context));
            }

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, EbayOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(base.GenerateSql(context.GetColumnReference(EbayOrderSearchFields.SellingManagerRecord), context));
            }

            return $"{orderSql} OR {orderSearchSql}"; ;
        }
    }
}
