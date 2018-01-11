using System;
using Interapptive.Shared.ComponentRegistration;
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
    [Component]
    public class OdbcDownloaderExtraDependencies : IOdbcDownloaderExtraDependencies
    {
        private readonly IStoreTypeManager storeTypeManager;
        private readonly Func<Type, ILog> logFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcDownloaderExtraDependencies(IStoreTypeManager storeTypeManager, Func<Type, ILog> logFactory)
        {
            this.logFactory = logFactory;
            this.storeTypeManager = storeTypeManager;
        }

        /// <summary>
        /// Get the StoreType for the given store
        /// </summary>
        public StoreType GetStoreType(StoreEntity store) =>
            storeTypeManager.GetType(store);

        /// <summary>
        /// Get a log for the given type
        /// </summary>
        public ILog GetLog(Type type) => logFactory(type);
    }
}