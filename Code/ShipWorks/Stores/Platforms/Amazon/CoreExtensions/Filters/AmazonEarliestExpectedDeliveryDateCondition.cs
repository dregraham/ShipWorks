using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters
{

    /// <summary>
    /// Condition for the amazon earliest delivery date
    /// </summary>
    [ConditionElement("Amazon Earliest Delivery", "Amazon.EarliestExpectedDeliveryDate")]
    [ConditionStoreType(StoreTypeCode.Amazon)]
    public class AmazonEarliestExpectedDeliveryDateCondition : DateCondition
    {
        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // We have to get from Order -> AmazonOrder            
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, AmazonOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(AmazonOrderFields.EarliestExpectedDeliveryDate), context));
            }
        }
    }
}
