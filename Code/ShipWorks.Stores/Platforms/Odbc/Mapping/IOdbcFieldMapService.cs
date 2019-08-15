using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Service for interacting with odbc field maps
    /// </summary>
    public interface IOdbcFieldMapService
    {
        /// <summary>
        /// Get the import odbc field map for the given store
        /// </summary>
        IOdbcFieldMap GetImportMap(OdbcStoreEntity store);

        /// <summary>
        /// Get the upload odbc field map for the given store
        /// </summary>
        IOdbcFieldMap GetUploadMap(OdbcStoreEntity store);

        /// <summary>
        /// Update the cached import odbc field map for the given store
        /// </summary>
        void UpdateImportMapCache(OdbcStoreEntity store);

        /// <summary>
        /// Update the cached import odbc field map for the given store
        /// </summary>
        void UpdateUploadMapCache(OdbcStoreEntity store);
    }
}
