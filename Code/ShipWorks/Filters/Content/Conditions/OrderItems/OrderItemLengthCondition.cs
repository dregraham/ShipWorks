using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition that compares against the Length of an OrderItem
    /// </summary>
    [ConditionElement("Item Length", "OrderItem.Length")]
    public class OrderItemLengthCondition : NumericCondition<decimal>
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderItemFields.Length), context);
        }
    }
}