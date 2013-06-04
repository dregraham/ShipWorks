﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the MarketplaceAdvisor Invoice Number field
    /// </summary>
    [ConditionElement("MarketplaceAdvisor Invoice #", "MarketplaceAdvisorOrder.InvoiceNumber")]
    [ConditionStoreType(StoreTypeCode.MarketplaceAdvisor)]
    public class MarketplaceAdvisorInvoiceNumberCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, MarketplaceAdvisorOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(MarketplaceAdvisorOrderFields.InvoiceNumber), context));
            }
        }
    }
}
