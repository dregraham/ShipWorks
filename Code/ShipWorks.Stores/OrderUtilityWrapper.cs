using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Wraps the OrderUtility
    /// </summary>
    public class OrderUtilityWrapper : IOrderUtility
    {
        /// <summary>
        /// Calculate the order total of the order.
        /// </summary>
        public decimal CalculateTotal(OrderEntity order) => OrderUtility.CalculateTotal(order);
    }
}