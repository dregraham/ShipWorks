using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    [ConditionElement("eBay Payment Method", "EbayOrderItem.PaymentMethod")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayPaymentMethodCondition : EnumCondition<EbayEffectivePaymentMethod>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayPaymentMethodCondition()
        {
            Value = EbayEffectivePaymentMethod.PayPal;
        }

        /// <summary>
        /// Generate the filter sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, EbayOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(EbayOrderItemFields.EffectivePaymentMethod), context));
            }
        }
    }
}
