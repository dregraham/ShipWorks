using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Upload;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    public interface IOdbcUploadCommandFactory
    {
        /// <summary>
        /// Creates the upload command for an Odbc Store.
        /// </summary>
        IOdbcUploadCommand CreateUploadCommand(OdbcStoreEntity store, ShipmentEntity shipment);
    }
}