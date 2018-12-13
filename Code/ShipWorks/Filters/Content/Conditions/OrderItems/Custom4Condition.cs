using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition base on the Custom4 of an OrderItem
    /// </summary>
    [ConditionElement("Custom Field 4", "OrderItem.Custom4")]
    public class Custom4Condition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) => 
            GenerateSql(context.GetColumnReference(OrderItemFields.Custom4), context);
    }
}