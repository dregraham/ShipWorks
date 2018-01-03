using System;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Extra dependencies for the ODBC Downloader
    /// </summary>
    /// <remarks>
    /// These are extra dependencies needed by the ODBC downloader that aren't part
    /// of its core functionality.
    /// </remarks>
    public interface IOdbcDownloaderExtraDependencies
    {
        /// <summary>
        /// Get the StoreType for the given store
        /// </summary>
        StoreType GetStoreType(StoreEntity store);

        /// <summary>
        /// Get a log for the given type
        /// </summary>
        ILog GetLog(Type type);
    }
}