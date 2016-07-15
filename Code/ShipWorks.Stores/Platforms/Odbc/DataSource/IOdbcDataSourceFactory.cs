using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource
{
    /// <summary>
    /// Interface that represents a data source factory
    /// </summary>
    public interface IOdbcDataSourceFactory
    {
        /// <summary>
        /// Create an empty data source
        /// </summary>
        /// <returns></returns>
        IOdbcDataSource CreateEmptyDataSource();

        /// <summary>
        /// Create the import datasource for the given store
        /// </summary>
        IOdbcDataSource CreateImportDataSource(OdbcStoreEntity store);

        /// <summary>
        /// Create the upload datasource for the given store
        /// </summary>
        IOdbcDataSource CreateUploadDataSource(OdbcStoreEntity store);
    }
}