using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Filters
{
    [ConditionElement("BigCommerce Event Date", "BigCommerceOrderItem.EventDate")]
    [ConditionStoreType(StoreTypeCode.BigCommerce)]
    class BigCommerceEventDateCondition : DateCondition
    {
        /// <summary>
        /// Generate the filter sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, BigCommerceOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(BigCommerceOrderItemFields.EventDate), context));
            }
        }
    }
}
