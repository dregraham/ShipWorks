using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition based on the number of lines in the order
    /// </summary>
    [ConditionElement("Shipment Count", "Order.ShipmentCount")]
    class OrderShipmentsCondition : NumericCondition<int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderShipmentsCondition()
        {
            // Can't have a a count lower than zero
            minimumValue = 0;
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string countSql = context.GetChildAggregate("COUNT", ShipmentFields.ShipmentID);

            return GenerateSql(countSql, context);
        }
    }
}
