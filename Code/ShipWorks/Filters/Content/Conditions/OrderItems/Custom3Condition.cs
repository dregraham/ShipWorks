using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition base on the Custom3 of an OrderItem
    /// </summary>
    [ConditionElement("Custom Field 3", "OrderItem.Custom3")]
    public class CustomItem3Condition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) => 
            GenerateSql(context.GetColumnReference(OrderItemFields.Custom3), context);
    }
}