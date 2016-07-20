using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Creates commands for an ODBC Store
    /// </summary>
    public interface IOdbcCommandFactory
    {
        /// <summary>
        /// Creates the upload command for an Odbc Store.
        /// </summary>
        IOdbcUploadCommand CreateUploadCommand(OdbcStoreEntity store, IOdbcFieldMap map);
    }
}