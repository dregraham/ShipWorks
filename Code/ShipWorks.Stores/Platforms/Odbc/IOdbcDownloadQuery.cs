namespace ShipWorks.Stores.Platforms.Odbc
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
