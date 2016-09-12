﻿using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Creates an OdbcDownloadCommand for a store
    /// </summary>
    public class OdbcDownloadCommandFactory : IOdbcDownloadCommandFactory
    {
        private readonly IOdbcDataSource dataSource;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDownloadCommandFactory"/> class.
        /// </summary>
        public OdbcDownloadCommandFactory(IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
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
        /// Creates the download query used to retrieve orders.
        /// </summary>
        private static IOdbcQuery GetDownloadQuery(OdbcStoreEntity store, IOdbcFieldMap odbcFieldMap, IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
        {
            dataSource.Restore(store.ImportConnectionString);

            return store.ImportColumnSourceType == (int) OdbcColumnSourceType.Table
                ? (IOdbcQuery)new OdbcTableDownloadQuery(store, dbProviderFactory, odbcFieldMap, dataSource)
                : new OdbcCustomDownloadQuery(store);
        }
    }
}