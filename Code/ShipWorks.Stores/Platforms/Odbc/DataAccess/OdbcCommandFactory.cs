using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Creates an OdbcCommand for a store
    /// </summary>
    public class OdbcCommandFactory
    {
        private readonly IOdbcFieldMap odbcFieldMap;
        private readonly IOdbcDataSource dataSource;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCommandFactory"/> class.
        /// </summary>
        public OdbcCommandFactory(IOdbcFieldMap odbcFieldMap, IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
        {
            this.odbcFieldMap = odbcFieldMap;
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// Creates the download command.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store)
        {
            IOdbcDownloadQuery downloadQuery = GetDownloadQuery(store, odbcFieldMap, dataSource, dbProviderFactory);

            return new OdbcDownloadCommand(odbcFieldMap, dataSource, dbProviderFactory, downloadQuery);
        }

        /// <summary>
        /// Creates the download command using OnlineLastModified.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store, DateTime onlineLastModified)
        {
            IOdbcDownloadQuery downloadQuery = GetDownloadQuery(store, odbcFieldMap, dataSource, dbProviderFactory);
            IOdbcDownloadQuery lastModifiedQuery = new OdbcLastModifiedDownloadQuery(downloadQuery, onlineLastModified, odbcFieldMap, dbProviderFactory, dataSource);

            return new OdbcDownloadCommand(odbcFieldMap, dataSource, dbProviderFactory, lastModifiedQuery);
        }

        /// <summary>
        /// Creates the download query used to retrieve orders.
        /// </summary>
        private static IOdbcDownloadQuery GetDownloadQuery(OdbcStoreEntity store, IOdbcFieldMap odbcFieldMap, IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
        {
            odbcFieldMap.Load(store.ImportMap);
            dataSource.Restore(store.ConnectionString);

            return store.ImportSourceType == (int) OdbcColumnSourceType.Table
                ? (IOdbcDownloadQuery)new TableOdbcDownloadQuery(store, dbProviderFactory, odbcFieldMap, dataSource)
                : new CustomQueryOdbcDownloadQuery(store);
        }
    }
}
