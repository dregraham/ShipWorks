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
    [ConditionElement("My eBay (Paid)", "EbayOrderItem.PaidStatus")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayItemPaidStatusCondition : BooleanCondition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayItemPaidStatusCondition()
            : base("Paid", "Not Paid")
        {

        }

        /// <summary>
        /// Generate the SQL for the filter
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, EbayOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(EbayOrderItemFields.MyEbayPaid), context));
            }
        }
    }
}
