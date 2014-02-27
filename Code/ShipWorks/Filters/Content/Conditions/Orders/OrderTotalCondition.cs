using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the total of an Order
    /// </summary>
    [ConditionElement("Order Total", "Order.Total")]
    public class OrderTotalCondition : MoneyCondition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderTotalCondition()
        {
            // Format as currency
            format = "C";
        }

        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.OrderTotal), context);
        }
    }
}
