using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition base on the Custom5 of an OrderItem
    /// </summary>
    [ConditionElement("Custom Field 5", "OrderItem.Custom5")]
    public class Custom5Condition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) => 
            GenerateSql(context.GetColumnReference(OrderItemFields.Custom5), context);
    }
}