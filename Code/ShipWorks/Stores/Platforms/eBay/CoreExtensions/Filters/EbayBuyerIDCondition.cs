using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    [ConditionElement("eBay Buyer", "EbayOrder.EbayBuyerID")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayBuyerIDCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            // First we have to get from Order -> EbayOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, EbayOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(EbayOrderFields.EbayBuyerID), context));
            }

            // First we have to get from Order -> EbayOrder
            using (SqlGenerationScope scope = context.PushScope(OrderSearchFields.OrderID, EbayOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(EbayOrderSearchFields.EbayBuyerID), context));
            }

            return $"{orderSql} OR {orderSearchSql}"; ;
        }
    }
}
