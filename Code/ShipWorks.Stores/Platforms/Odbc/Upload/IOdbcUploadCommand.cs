namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Interface for uploading data to ODBC
    /// </summary>
    public interface IOdbcUploadCommand
    {
        /// <summary>
        /// Gets the name of the ODBC driver being used to execute this command.
        /// </summary>
        string Driver { get;  }

        /// <summary>
        /// Executes the command and returns the number of rows affected.
        /// </summary>
        int Execute();
    }
}