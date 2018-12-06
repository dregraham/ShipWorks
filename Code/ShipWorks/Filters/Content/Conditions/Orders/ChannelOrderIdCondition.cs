using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the ChannelOrderID of an Order
    /// </summary>
    [ConditionElement("Channel Order ID", "Order.ChannelOrderID")]
    public class ChannelOrderIdCondition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) => GenerateSql(context.GetColumnReference(OrderFields.ChannelOrderID), context);
    }
}
