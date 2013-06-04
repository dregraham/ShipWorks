using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the total weight of an order
    /// </summary>
    [ConditionElement("Total Weight", "Order.TotalWeight")]
    public class TotalWeightCondition : NumericCondition<double>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TotalWeightCondition()
        {

        }

        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.RollupItemTotalWeight), context);
        }
    }
}
