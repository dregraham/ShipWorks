﻿using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource
{
    /// <summary>
    /// Factory for creating datasource
    /// </summary>
    public class OdbcDataSourceService : IOdbcDataSourceService
    {
        private readonly Func<IOdbcDataSource> dataSourceFactory;
        private readonly IOdbcDataSourceRepository dataSourceRepository;
        private readonly IOdbcStoreRepository storeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSourceFactory"></param>
        /// <param name="dataSourceRepository"></param>
        public OdbcDataSourceService(
            Func<IOdbcDataSource> dataSourceFactory,
            IOdbcDataSourceRepository dataSourceRepository,
            IOdbcStoreRepository storeRepository)
        {
            this.dataSourceFactory = dataSourceFactory;
            this.dataSourceRepository = dataSourceRepository;
            this.storeRepository = storeRepository;
        }

        /// <summary>
        /// Get a list of data sources on the machine
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IOdbcDataSource> GetDataSources()
        {
            return dataSourceRepository.GetDataSources();
        }

        /// <summary>
        /// Create a data source form the given connection string
        /// </summary>
        /// <returns></returns>
        public IOdbcDataSource GetDataSource(string connectionString)
        {
            IOdbcDataSource source = dataSourceFactory();
            source.ChangeConnection(connectionString);

            return source;
        }

        /// <summary>
        /// Create the import data source for the given store
        /// </summary>
        public IOdbcDataSource GetImportDataSource(OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            IOdbcDataSource dataSource = dataSourceFactory();
            dataSource.Restore(store.ImportConnectionString);

            return dataSource;
        }

        /// <summary>
        /// Create the upload data source for the given store
        /// </summary>
        public IOdbcDataSource GetUploadDataSource(OdbcStoreEntity store, bool useLocalEntity)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            if (useLocalEntity)
            {
                string connectionString = GetUploadConnectionString(store);

                if (connectionString != null)
                {
                    IOdbcDataSource localDataSource = dataSourceFactory();
                    localDataSource.Restore(connectionString);
                    return localDataSource;
                }

                return null;
            }

            OdbcStore odbcStore = storeRepository.GetStore(store);

            if (odbcStore.UploadStrategy == (int) OdbcShipmentUploadStrategy.DoNotUpload)
            {
                return null;
            }

            IOdbcDataSource dataSource = dataSourceFactory();

            if (odbcStore.UploadStrategy == (int) OdbcShipmentUploadStrategy.UseImportDataSource && !string.IsNullOrEmpty(store.ImportConnectionString))
            {
                dataSource.Restore(store.ImportConnectionString);
            }
            else
            {
                dataSource.Restore(store.UploadConnectionString);
            }

            return dataSource;
        }

        /// <summary>
        /// Get the stores connection string for uploading
        /// </summary>
        /// <param name="store"></param>
        private string GetUploadConnectionString(OdbcStoreEntity store)
        {
            if (store.UploadStrategy == (int) OdbcShipmentUploadStrategy.DoNotUpload)
            {
                return null;
            }

            if (store.UploadStrategy == (int) OdbcShipmentUploadStrategy.UseImportDataSource && !string.IsNullOrEmpty(store.ImportConnectionString))
            {
                return store.ImportConnectionString;
            }

            return store.UploadConnectionString;
        }
    }
}