using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.PayPal;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    [ConditionElement("PayPal Address Status", "EbayOrderItem.PayPalAddressStatus")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayPayPalAddressStatusCondition : EnumCondition<PayPalAddressStatus>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayPayPalAddressStatusCondition()
        {
            Value = PayPalAddressStatus.Confirmed;
        }

        /// <summary>
        /// Generate the filter sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, EbayOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(EbayOrderItemFields.PayPalAddressStatus), context));
            }
        }
    }
}
