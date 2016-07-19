namespace ShipWorks.Stores.Platforms.Odbc.Upload
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