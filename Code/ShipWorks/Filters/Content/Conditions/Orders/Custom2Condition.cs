using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the Custom2 of an Order
    /// </summary>
    [ConditionElement("Custom Field 2", "Order.Custom2")]
    public class Custom2Condition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) => GenerateSql(context.GetColumnReference(OrderFields.Custom2), context);
    }
}