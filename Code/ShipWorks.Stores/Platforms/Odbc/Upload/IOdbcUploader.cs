using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Uploader for Odbc stores
    /// </summary>
    public interface IOdbcUploader
    {
        /// <summary>
        /// Uploads the latest shipment.
        /// </summary>
        void UploadLatestShipment(OdbcStoreEntity store, long orderid);

        /// <summary>
        /// Uploads the shipments.
        /// </summary>
        void UploadShipments(OdbcStoreEntity store, IEnumerable<long> shipmentIds);
    }
}