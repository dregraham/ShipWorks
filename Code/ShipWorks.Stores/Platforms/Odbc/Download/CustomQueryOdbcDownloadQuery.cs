using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Download query to be used when user has entered a custom query
    /// </summary>
    public class CustomQueryOdbcDownloadQuery : IOdbcDownloadQuery
    {
        private readonly OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomQueryOdbcDownloadQuery"/> class.
        /// </summary>
        public CustomQueryOdbcDownloadQuery(OdbcStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Generates the Sql to download orders.
        /// </summary>
        public string GenerateSql() => store.OdbcColumnSource;
    }
}