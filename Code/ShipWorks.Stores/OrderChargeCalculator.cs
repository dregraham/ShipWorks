using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Calculates order charges
    /// </summary>
    public class OrderChargeCalculator : IOrderChargeCalculator
    {
        /// <summary>
        /// Calculate the order total of the order.
        /// </summary>
        public decimal CalculateTotal(OrderEntity order) => OrderUtility.CalculateTotal(order);
    }
}