namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Interface for uploading data to ODBC
    /// </summary>
    public interface IOdbcUploadCommand
    {
        /// <summary>
        /// Execute the command
        /// </summary>
        int Execute();
    }
}