using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Overstock.DTO;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Interface for OverstockWebClient
    /// </summary>
    public interface IOverstockWebClient
    {
        /// <summary>
        /// Get orders overstock
        /// </summary>
        Task<GenericResult<List<OverstockOrderDto>>> GetOrders(IOverstockStoreEntity store, DateTime startDateTime, DateTime endDateTime);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        Task UploadShipmentDetails(long overstockOrderId, ShipmentEntity shipment, IOverstockStoreEntity store);

        /// <summary>
        /// Test the connection
        /// </summary>
        Task<bool> TestConnection(IOverstockStoreEntity store);
    }
}
