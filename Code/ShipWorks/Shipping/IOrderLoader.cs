using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Loads shipments from a list of orders in the background with progress
    /// </summary>
    public interface IOrderLoader
    {
        /// <summary>
        /// Load the shipments for the given collection of orders or shipments
        /// </summary>
        Task<ShipmentsLoadedEventArgs> LoadAsync(IEnumerable<long> entityIDs, ProgressDisplayOptions displayOptions, bool createIfNoShipments, TimeSpan timeout);

        /// <summary>
        /// Load the shipments for the given collection of orders or shipments
        /// </summary>
        Task<ShipmentsLoadedEventArgs> LoadAsync(IEnumerable<long> entityIDs, ProgressDisplayOptions displayOptions, bool createIfNoShipments, int timeoutInMilliseconds);
    }
}
