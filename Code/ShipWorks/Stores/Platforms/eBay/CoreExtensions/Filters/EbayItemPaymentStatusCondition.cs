using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for testing Checkout Status of an eBay order item.
    /// </summary>
    [ConditionElement("eBay Payment Status", "EbayOrderItem.CheckoutStatus")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayItemPaymentStatusCondition : EnumCondition<EbayEffectivePaymentStatus>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayItemPaymentStatusCondition()
        {
            Value = EbayEffectivePaymentStatus.Paid;
        }

        /// <summary>
        /// Generate the SQL for the filter
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, EbayOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(EbayOrderItemFields.EffectiveCheckoutStatus), context));
            }
        }
    }
}
