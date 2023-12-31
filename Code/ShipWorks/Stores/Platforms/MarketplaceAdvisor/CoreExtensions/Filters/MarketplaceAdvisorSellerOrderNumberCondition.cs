﻿using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the MarketplaceAdvisor Invoice Number field
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [ConditionElement("MarketplaceAdvisor Seller Order #", "MarketplaceAdvisorOrder.SellerOrderNumber")]
    [ConditionStoreType(StoreTypeCode.MarketplaceAdvisor)]
    public class MarketplaceAdvisorSellerOrderNumberCondition : NumericStringCondition<long>
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, MarketplaceAdvisorOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(MarketplaceAdvisorOrderFields.SellerOrderNumber), context));
            }

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, MarketplaceAdvisorOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(MarketplaceAdvisorOrderSearchFields.SellerOrderNumber), context));
            }

            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}
