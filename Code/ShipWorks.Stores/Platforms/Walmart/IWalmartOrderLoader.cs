using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Represents an order loader for walmart
    /// </summary>
    public interface IWalmartOrderLoader
    {
        /// <summary>
        /// Load the given walmart order into the WalmartOrderEntity
        /// </summary>
        /// <param name="downloadedOrder">The order from Walmart</param>
        /// <param name="orderToSave">The order entity to load data into</param>
        void LoadOrder(Order downloadedOrder, WalmartOrderEntity orderToSave);
    }
}