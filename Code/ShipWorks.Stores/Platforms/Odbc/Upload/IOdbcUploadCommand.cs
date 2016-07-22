namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Interface for uploading data to ODBC
    /// </summary>
    public interface IOdbcUploadCommand
    {
        /// <summary>
        /// Executes the command and returns the number of rows affected.
        /// </summary>
        int Execute();
    }
}