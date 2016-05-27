using Interapptive.Shared.Utility;
using System.Data.Common;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Represents an interface for interacting with an OdbcDataSource
    /// </summary>
    public interface IOdbcDataSource
    {
        /// <summary>
        /// Tests the Data Source's connection
        /// </summary>
        GenericResult<IOdbcDataSource> TestConnection();

        /// <summary>
        /// Creates an ODBC connection to the Data Source
        /// </summary>
        DbConnection CreateConnection();

        /// <summary>
        /// Name of the data source
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Set connection string from serialized json.
        /// </summary>
        /// <param name="json">The json.</param>
        void Restore(string json);

        /// <summary>
        /// Changes the connection.
        /// </summary>
        /// <param name="dsn">The DSN.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        void ChangeConnection(string dsn, string username, string password);

        /// <summary>
        /// Changes the connection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        void ChangeConnection(string connectionString);

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Gets or sets whether or not the data
        /// source is using a custom connection string.
        /// </summary>
        bool IsCustom { get; }

        /// <summary>
        /// Gets or sets the custom connection string.
        /// </summary>
        string ConnectionString { get; }
    }
}