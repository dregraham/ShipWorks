using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the Custom10 of an Order
    /// </summary>
    [ConditionElement("Deliver By Date", "Order.DeliverByDate")]
    public class DeliverByDateCondition : DateCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) => GenerateSql(context.GetColumnReference(OrderFields.DeliverByDate), context);
    }
}