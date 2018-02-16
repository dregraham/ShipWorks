using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.Shopify.Enums;

namespace ShipWorks.Stores.Platforms.Shopify.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for Shopify Payment Status
    /// </summary>
    [ConditionElement("Shopify Payment Status", "ShopifyOrder.OnlinePaymentStatusCode")]
    [ConditionStoreType(StoreTypeCode.Shopify)]
    public class ShopifyPaymentStatusCondition : EnumCondition<ShopifyPaymentStatus>
    {
        /// <summary>
        /// Default Constructor.  Defaults Value to Authorized.
        /// </summary>
        public ShopifyPaymentStatusCondition()
        {
            Value = ShopifyPaymentStatus.Authorized;
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "context is required");
            }

            // Get sql for matching up with payment status            
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ShopifyOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(ShopifyOrderFields.PaymentStatusCode), context));
            }
        }
    }
}
