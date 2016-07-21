using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Creates an OdbcCommand for a store
    /// </summary>
    public class OdbcCommandFactory : IOdbcCommandFactory
    {
        private readonly IOdbcDataSource dataSource;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCommandFactory"/> class.
        /// </summary>
        public OdbcCommandFactory(IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
        {
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// Creates the download command.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store, IOdbcFieldMap odbcFieldMap)
        {
            IOdbcQuery downloadQuery = GetDownloadQuery(store, odbcFieldMap, dataSource, dbProviderFactory);

            return new OdbcDownloadCommand(odbcFieldMap, dataSource, dbProviderFactory, downloadQuery);
        }

        /// <summary>
        /// Creates the download command using OnlineLastModified.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store, DateTime onlineLastModified, IOdbcFieldMap odbcFieldMap)
        {
            IOdbcQuery downloadQuery = GetDownloadQuery(store, odbcFieldMap, dataSource, dbProviderFactory);
            IOdbcQuery lastModifiedQuery = new OdbcLastModifiedDownloadQuery(downloadQuery, onlineLastModified, odbcFieldMap, dbProviderFactory, dataSource);

            return new OdbcDownloadCommand(odbcFieldMap, dataSource, dbProviderFactory, lastModifiedQuery);
        }

        /// <summary>
        /// Creates the upload command for the given store and map
        /// </summary>
        public IOdbcUploadCommand CreateUploadCommand(OdbcStoreEntity store, IOdbcFieldMap map)
        {
            IOdbcQuery uploadQuery = GetUploadQuery(store, map, dataSource, dbProviderFactory);

            if (uploadQuery == null)
            {
                string uploadStrategy = EnumHelper.GetDescription((OdbcShipmentUploadStrategy) store.UploadStrategy);

                throw new ShipWorksOdbcException($"Unable to create upload command for store when the store upload strategy is '{uploadStrategy}'.");
            }

            return new OdbcUploadCommand(dataSource, dbProviderFactory, uploadQuery);
        }

        /// <summary>
        /// Creates the download query used to retrieve orders.
        /// </summary>
        private static IOdbcQuery GetDownloadQuery(OdbcStoreEntity store, IOdbcFieldMap odbcFieldMap, IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
        {
            dataSource.Restore(store.ImportConnectionString);

            return store.ImportSourceType == (int) OdbcColumnSourceType.Table
                ? (IOdbcQuery)new TableOdbcDownloadQuery(store, dbProviderFactory, odbcFieldMap, dataSource)
                : new CustomQueryOdbcDownloadQuery(store);
        }

        /// <summary>
        /// Creates the download query used to retrieve orders.
        /// </summary>
        private static IOdbcQuery GetUploadQuery(OdbcStoreEntity store, IOdbcFieldMap odbcFieldMap, IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
        {
            switch (store.UploadStrategy)
            {
                case (int) OdbcShipmentUploadStrategy.UseImportDataSource:
                    dataSource.Restore(store.ImportConnectionString);
                    break;
                case (int)OdbcShipmentUploadStrategy.UseShipmentDataSource:
                    dataSource.Restore(store.UploadConnectionString);
                    break;
                default:
                    return null;
            }

            return new OdbcTableUploadQuery(odbcFieldMap, store, dbProviderFactory, dataSource);
        }
    }
}
