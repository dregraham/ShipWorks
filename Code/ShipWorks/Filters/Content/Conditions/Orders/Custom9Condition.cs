using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the Custom9 of an Order
    /// </summary>
    [ConditionElement("Custom Field 9", "Order.Custom9")]
    public class Custom9Condition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) => GenerateSql(context.GetColumnReference(OrderFields.Custom9), context);
    }
}
