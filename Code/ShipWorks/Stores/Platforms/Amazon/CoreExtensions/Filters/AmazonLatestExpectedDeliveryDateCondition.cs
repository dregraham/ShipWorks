﻿using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the amazon latest delivery date
    /// </summary>
    [ConditionElement("Amazon Latest Delivery", "Amazon.LatestExpectedDeliveryDate")]
    [ConditionStoreType(StoreTypeCode.Amazon)]
    public class AmazonLatestExpectedDeliveryDateCondition : DateCondition
    {
        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // We have to get from Order -> AmazonOrder            
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, AmazonOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(AmazonOrderFields.LatestExpectedDeliveryDate), context));
            }
        }
    }
}
