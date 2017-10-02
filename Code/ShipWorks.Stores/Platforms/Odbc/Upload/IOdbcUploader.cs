using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Uploader for ODBC stores
    /// </summary>
    public interface IOdbcUploader
    {
        /// <summary>
        /// Uploads the latest shipment.
        /// </summary>
        Task UploadLatestShipment(OdbcStoreEntity store, long orderid);

        /// <summary>
        /// Uploads the shipments.
        /// </summary>
        Task UploadShipments(OdbcStoreEntity store, IEnumerable<long> shipmentIds);
    }
}