using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

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
        /// Create the import datasource for the given store
        /// </summary>
        public IOdbcDataSource CreateImportDataSource(OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, "Odbc Store");

            IOdbcDataSource dataSource = dataSourceFactory();
            dataSource.Restore(store.ConnectionString);

            return dataSource;
        }

        /// <summary>
        /// Create the upload datasource for the given store
        /// </summary>
        public IOdbcDataSource CreateUploadDataSource(OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, "Odbc Store");

            IOdbcDataSource dataSource = dataSourceFactory();
            dataSource.Restore(store.ConnectionString);

            return dataSource;
        }
    }
}