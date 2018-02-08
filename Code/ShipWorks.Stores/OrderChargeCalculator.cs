using ShipWorks.Data.Model.EntityInterfaces;
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
        public decimal CalculateTotal(IOrderEntity order) => OrderUtility.CalculateTotal(order);
    }
}