using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Filters
{
    [ConditionElement("BigCommerce Event Name", "BigCommerceOrderItem.EventName")]
    [ConditionStoreType(StoreTypeCode.BigCommerce)]
    class BigCommerceEventNameCondition : StringCondition
    {
        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from OrderItem -> BigCommerceOrderItem
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, BigCommerceOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(BigCommerceOrderItemFields.EventName), context));
            }
        }
    }
}
