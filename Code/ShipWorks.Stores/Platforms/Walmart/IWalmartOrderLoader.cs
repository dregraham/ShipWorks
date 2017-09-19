using System.Collections.Generic;
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

        /// <summary>
        /// Loads the order items.
        /// </summary>
        /// <remarks>
        /// Creates new items or updates existing items. This method assumes that
        /// a line item will not be deleted and that the price will go to 0 if
        /// the order is canceled.
        /// </remarks>
        void LoadItems(IEnumerable<orderLineType> downloadedOrderOrderLines, WalmartOrderEntity orderToSave);
    }
}