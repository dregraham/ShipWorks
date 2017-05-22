using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Filters
{
    [ConditionElement("BigCommerce Digital Item", "BigCommerceOrderItem.IsDigitalItem")]
    [ConditionStoreType(StoreTypeCode.BigCommerce)]
    class BigCommerceDigitalItemCondition : BooleanCondition
    {
        public BigCommerceDigitalItemCondition()
            : base("Yes", "No")
        { }

        /// <summary>
        /// Generate the filter sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, BigCommerceOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(BigCommerceOrderItemFields.IsDigitalItem), context));
            }
        }
    }
}
