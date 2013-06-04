using System;
using System.Collections.Generic;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Condition based on the number of orders a customer has placed
    /// </summary>
    [ConditionElement("Orders Placed", "Customer.OrderCount")]
    class OrderCountCondition : NumericCondition<int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderCountCondition()
        {
            // Can't have a a count lower than zero
            minimumValue = 0;
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.RollupOrderCount), context);
        }
    }
}
