using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Calculates order charges
    /// </summary>
    public interface IOrderChargeCalculator
    {
        /// <summary>
        /// Calculate the order total of the order. 
        /// </summary>
        decimal CalculateTotal(OrderEntity order);
    }
}
