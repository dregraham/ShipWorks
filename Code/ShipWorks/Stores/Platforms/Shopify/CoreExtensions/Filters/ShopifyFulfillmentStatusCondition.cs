using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Shopify.Enums;

namespace ShipWorks.Stores.Platforms.Shopify.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for Shopify Fulfillment Status
    /// </summary>
    [ConditionElement("Shopify Fulfillment Status", "ShopifyOrder.OnlineFulfillmentStatusCode")]
    [ConditionStoreType(StoreTypeCode.Shopify)]
    public class ShopifyFulfillmentStatusCondition : EnumCondition<ShopifyFulfillmentStatus>
    {
        /// <summary>
        /// Default Constructor.  Defaults Value to Unshipped.
        /// </summary>
        public ShopifyFulfillmentStatusCondition()
        {
            Value = ShopifyFulfillmentStatus.Unshipped;
            SelectedValues = new[] { Value };
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "context is required");
            }

            // Get sql for matching up with fulfillment status        
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ShopifyOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(ShopifyOrderFields.FulfillmentStatusCode), context));
            }
        }
    }
}
