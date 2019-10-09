using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition base on the Custom8 of an OrderItem
    /// </summary>
    [ConditionElement("Custom Field 8", "OrderItem.Custom8")]
    public class Custom8Condition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) =>
            GenerateSql(context.GetColumnReference(OrderItemFields.Custom8), context);
    }
}
