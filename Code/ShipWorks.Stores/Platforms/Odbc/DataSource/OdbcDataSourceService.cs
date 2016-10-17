using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Odbc.Upload;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource
{
    /// <summary>
    /// Factory for creating datasource
    /// </summary>
    public class OdbcDataSourceService : IOdbcDataSourceService
    {
        private readonly Func<IOdbcDataSource> dataSourceFactory;
        private readonly IOdbcDataSourceRepository dataSourceRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSourceFactory"></param>
        /// <param name="dataSourceRepository"></param>
        public OdbcDataSourceService(Func<IOdbcDataSource> dataSourceFactory, IOdbcDataSourceRepository dataSourceRepository)
        {
            this.dataSourceFactory = dataSourceFactory;
            this.dataSourceRepository = dataSourceRepository;
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
            MethodConditions.EnsureArgumentIsNotNull(store, "Odbc Store");

            IOdbcDataSource dataSource = dataSourceFactory();
            dataSource.Restore(store.ImportConnectionString);

            return dataSource;
        }

        /// <summary>
        /// Create the upload data source for the given store
        /// </summary>
        public IOdbcDataSource GetUploadDataSource(OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, "Odbc Store");

            if (store.UploadStrategy == (int)OdbcShipmentUploadStrategy.DoNotUpload)
            {
                return null;
            }

            IOdbcDataSource dataSource = dataSourceFactory();

            if (store.UploadStrategy == (int) OdbcShipmentUploadStrategy.UseImportDataSource)
            {
                dataSource.Restore(store.ImportConnectionString);
            }

            if (store.UploadStrategy == (int) OdbcShipmentUploadStrategy.UseShipmentDataSource)
            {
                dataSource.Restore(store.UploadConnectionString);
            }

            return dataSource;
        }
    }
}