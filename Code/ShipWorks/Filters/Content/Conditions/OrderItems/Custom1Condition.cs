using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition base on the Custom1 of an OrderItem
    /// </summary>
    [ConditionElement("Custom Field 1", "OrderItem.Custom1")]
    public class Custom1Condition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) => 
            GenerateSql(context.GetColumnReference(OrderItemFields.Custom1), context);
    }
}