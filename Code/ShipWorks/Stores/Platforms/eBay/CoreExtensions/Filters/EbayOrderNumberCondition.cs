using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the ebay extendex order id field
    /// </summary>
    [ConditionElement("eBay Order Number", "EbayOrder.ExtendedOrderID")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    class ExtendedOrderIDCondition : StringCondition
    {
        /// <summary>
        /// Create the sql for filtering
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            // First we have to get from Order -> EbayOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, EbayOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(EbayOrderFields.ExtendedOrderID), context));
            }

            // First we have to get from Order -> EbayOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, EbayOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(EbayOrderSearchFields.ExtendedOrderID), context));
            }

            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}
