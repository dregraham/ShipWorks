using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Overstock.OnlineUpdating;

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
        Task<XDocument> GetOrders(IOverstockStoreEntity store, Range<DateTime> downloadRange);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        Task UploadShipmentDetails(IOverstockStoreEntity store, IEnumerable<OverstockSupplierShipment> shipments);

        /// <summary>
        /// Test the connection
        /// </summary>
        Task<bool> TestConnection(IOverstockStoreEntity store);
    }
}
