using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Creates an OdbcCommand for a store
    /// </summary>
    public class OdbcCommandFactory
    {
        private readonly IOdbcFieldMap odbcFieldMap;
        private readonly IOdbcDataSource dataSource;
        private readonly Func<Type, ILog> logFactory;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCommandFactory"/> class.
        /// </summary>
        public OdbcCommandFactory(IOdbcFieldMap odbcFieldMap, IOdbcDataSource dataSource, Func<Type, ILog> logFactory, IShipWorksDbProviderFactory dbProviderFactory)
        {
            this.odbcFieldMap = odbcFieldMap;
            this.dataSource = dataSource;
            this.logFactory = logFactory;
            this.dbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// Creates the download command.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store)
        {
            odbcFieldMap.Load(store.Map);
            dataSource.Restore(store.ConnectionString);

            return new OdbcDownloadCommand(odbcFieldMap, dataSource, dbProviderFactory, logFactory(typeof(OdbcDownloadCommand)));
        }
    }
}
