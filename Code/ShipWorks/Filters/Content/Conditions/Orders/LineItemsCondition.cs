using System;
using System.Collections.Generic;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition based on the number of lines in the order
    /// </summary>
    [ConditionElement("Line Items", "Order.LineItems")]
    class OrderLineItemsCondition : NumericCondition<int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLineItemsCondition()
        {
            // Can't have a a count lower than zero
            minimumValue = 0;
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string countSql = context.GetChildAggregate("COUNT", OrderItemFields.OrderItemID);

            return GenerateSql(countSql, context);
        }
    }
}
