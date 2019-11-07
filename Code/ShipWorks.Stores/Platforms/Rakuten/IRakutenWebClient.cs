using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Rakuten.DTO;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// The client for communicating with the Rakuten API
    /// </summary>
    public interface IRakutenWebClient
    {
        /// <summary>
        /// Get a list of orders from Rakuten
        /// </summary>
        RakutenOrdersResponse GetOrders(DateTime startDate);

        /// <summary>
        /// Mark order as shipped and upload tracking number
        /// </summary>
        RakutenShipmentResponse ConfirmShipping(ShipmentEntity shipment);

        bool TestConnection(RakutenStoreEntity store);
    }
}