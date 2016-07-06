using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Download query to be used when user has entered a custom query
    /// </summary>
    public class CustomQueryOdbcDownloadQuery : IOdbcDownloadQuery
    {
        private readonly IOdbcFieldMap map;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomQueryOdbcDownloadQuery"/> class.
        /// </summary>
        public CustomQueryOdbcDownloadQuery(IOdbcFieldMap map)
        {
            this.map = map;
        }

        /// <summary>
        /// Generates the Sql to download orders.
        /// </summary>
        public string GenerateSql() => map.CustomQuery;
    }
}