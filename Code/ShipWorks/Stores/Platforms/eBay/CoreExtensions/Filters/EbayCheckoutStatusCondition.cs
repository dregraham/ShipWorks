﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for testing Checkout Status of an eBay order item.
    /// </summary>
    [ConditionElement("eBay Checkout", "EbayOrderItem.CheckoutStatus")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayCheckoutStatusCondition : EnumCondition<EbayEffectiveCheckoutStatus>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayCheckoutStatusCondition()
        {
            Value = EbayEffectiveCheckoutStatus.Paid;
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
