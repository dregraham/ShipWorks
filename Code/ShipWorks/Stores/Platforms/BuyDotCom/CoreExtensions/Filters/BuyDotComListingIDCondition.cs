using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.BuyDotCom.CoreExtensions.Filters
{
    [ConditionElement("Buy.com Listing ID", "BuyDotComOrderItem.ListingID")]
    [ConditionStoreType(StoreTypeCode.BuyDotCom)]
    public class BuyDotComListingIDCondition : StringCondition
    {
        public override string GenerateSql(SqlGenerationContext context)
        {
            // first get from OrderItem -> BuyDotComOrderItem
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, BuyDotComOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(BuyDotComOrderItemFields.ListingID), context));
            }
        }
    }
}
