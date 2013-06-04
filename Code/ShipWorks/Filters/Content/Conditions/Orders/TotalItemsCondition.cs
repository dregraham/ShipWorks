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
    /// Condition based on the total number of items ordered in an order
    /// </summary>
    [ConditionElement("Total Items", "Order.TotalItems")]
    class OrderTotalItemsCondition : NumericCondition<int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderTotalItemsCondition()
        {
            // Can't have a a count lower than zero
            minimumValue = 0;
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string countSql = context.GetChildAggregate("SUM", OrderItemFields.Quantity);

            return GenerateSql(countSql, context);
        }
    }
}
