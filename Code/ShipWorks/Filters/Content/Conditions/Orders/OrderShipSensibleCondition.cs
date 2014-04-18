using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    ///     Filter for OrderShipSensibleCondition
    /// </summary>
    [ConditionElement("ShipSensible", "Order.ShipSensible")]
    internal class OrderShipSensibleCondition : BooleanCondition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderShipSensibleCondition" /> class.
        /// </summary>
        public OrderShipSensibleCondition()
            : base("Yes", "No")
        {
            Value = false;
        }

        /// <summary>
        ///     Generate the SQL for the condition clement
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "context required");
            }

            return base.GenerateSql(context.GetColumnReference(OrderFields.ShipSensible), context);
        }
    }
}
