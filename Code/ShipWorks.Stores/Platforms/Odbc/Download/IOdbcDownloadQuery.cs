namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Generates a context specific download query
    /// </summary>
    public interface IOdbcDownloadQuery
    {
        /// <summary>
        /// Generates the Sql to download orders.
        /// </summary>
        string GenerateSql();
    }
}
