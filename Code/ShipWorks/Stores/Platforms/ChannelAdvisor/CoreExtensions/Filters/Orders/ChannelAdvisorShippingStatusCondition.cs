﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters
{
    [ConditionElement("ChannelAdvisor Shipping Status", "ChannelAdvisorOrder.ShippingStatus")]
    [ConditionStoreType(StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorShippingStatusCondition : EnumCondition<ChannelAdvisorShippingStatus>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorShippingStatusCondition()
        {
            Value = ChannelAdvisorShippingStatus.Shipped;
            SelectedValues = new[] { Value };
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // We have to get from Order -> ChannelAdvisorOrder            
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ChannelAdvisorOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(ChannelAdvisorOrderFields.OnlineShippingStatus), context));
            }
        }
    }
}
