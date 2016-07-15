using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System;
using ShipWorks.Stores.Platforms.Odbc.Upload;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource
{
    /// <summary>
    /// Factory for creating datasource
    /// </summary>
    public class OdbcDataSourceFactory : IOdbcDataSourceFactory
    {
        private readonly Func<IOdbcDataSource> dataSourceFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSourceFactory"></param>
        public OdbcDataSourceFactory(Func<IOdbcDataSource> dataSourceFactory)
        {
            this.dataSourceFactory = dataSourceFactory;
        }

        /// <summary>
        /// Create an empty data source
        /// </summary>
        /// <returns></returns>
        public IOdbcDataSource CreateEmptyDataSource()
        {
            return dataSourceFactory();
        }

        /// <summary>
        /// Create the import datasource for the given store
        /// </summary>
        public IOdbcDataSource CreateImportDataSource(OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, "Odbc Store");

            IOdbcDataSource dataSource = dataSourceFactory();
            dataSource.Restore(store.ImportConnectionString);

            return dataSource;
        }

        /// <summary>
        /// Create the upload datasource for the given store
        /// </summary>
        public IOdbcDataSource CreateUploadDataSource(OdbcStoreEntity store)
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