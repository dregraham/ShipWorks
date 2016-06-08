using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Utility for working with orders.
    /// </summary>
    public interface IOrderUtility
    {
        /// <summary>
        /// Calculate the order total of the order. 
        /// </summary>
        decimal CalculateTotal(OrderEntity order);
    }
}
