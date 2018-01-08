using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Prime
    /// </summary>
    [ConditionElement("Generic Module Amazon Order ID", "GenericModule.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.GenericModule)]
    public class GenericModuleAmazonOrderIDCondition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, GenericModuleOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(GenericModuleOrderFields.AmazonOrderID), context));
            }
        }
    }
}
