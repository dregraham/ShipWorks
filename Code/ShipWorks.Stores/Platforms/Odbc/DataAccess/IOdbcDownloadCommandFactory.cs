using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Creates commands for an ODBC Store
    /// </summary>
    public interface IOdbcDownloadCommandFactory
    {
        /// <summary>
        /// Creates the download command.
        /// </summary>
        IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store, IOdbcFieldMap odbcFieldMap);

        /// <summary>
        /// Creates the download command using OnlineLastModified.
        /// </summary>
        IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store, DateTime onlineLastModified, IOdbcFieldMap odbcFieldMap);
    }
}