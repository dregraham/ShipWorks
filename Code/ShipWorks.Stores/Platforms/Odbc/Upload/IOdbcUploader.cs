using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    public interface IOdbcUploader
    {
        void UploadLatestShipment(OdbcStoreEntity store, long orderid);
        void UploadShipments(OdbcStoreEntity store, IEnumerable<long> shipmentIds);
    }
}