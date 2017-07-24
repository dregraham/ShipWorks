using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the Order# of an Order
    /// </summary>
    [ConditionElement("Order Number", "Order.Number")]
    public class OrderNumberCondition : NumericStringCondition<long>
    {
        /// <summary>
        /// Generate the SQL that evaluates the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            if (IsNumeric)
            {
                return $"{GenerateSql(context.GetColumnReference(OrderFields.OrderNumber), context)} AND OrderNumber != {int.MinValue}";
            }
            else
            {
                return StringCondition.GenerateSql(StringValue, StringOperator, context.GetColumnReference(OrderFields.OrderNumberComplete), context);
            }
        }
    }
}
