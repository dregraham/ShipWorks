using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the MarketplaceAdvisor Invoice Number field
    /// </summary>
    [ConditionElement("Amazon Order #", "Amazon.OrderID")]
    [ConditionStoreType(StoreTypeCode.Amazon)]
    public class AmazonOrderNumberCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, AmazonOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(AmazonOrderFields.AmazonOrderID), context));
            }
        }
    }
}
