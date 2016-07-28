using Interapptive.Shared.Utility;
using System.Data.Common;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource
{
    /// <summary>
    /// Represents an interface for interacting with an OdbcDataSource
    /// </summary>
    public interface IOdbcDataSource
    {
        /// <summary>
        /// Name of the data source
        /// </summary>
        string Name { get; }

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

        /// <summary>
        /// Gets the name of the ODBC driver for the data source.
        /// </summary>
        /// <value>The driver.</value>
        string Driver { get; }

        /// <summary>
        /// Set connection string from serialized json.
        /// </summary>
        /// <param name="json">The json.</param>
        void Restore(string json);

        /// <summary>
        /// Changes the connection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        void ChangeConnection(string connectionString);

        /// <summary>
        /// Changes the connection to use a new dsn, username, and password.
        /// </summary>
        void ChangeConnection(string dsn, string username, string password);


        /// <summary>
        /// Changes the connection to use a new dsn, username, password, and ODBC driver
        /// used to connect to the data source.
        /// </summary>
        void ChangeConnection(string dsn, string username, string password, string driver);

        /// <summary>
        /// Tests the Data Source's connection
        /// </summary>
        GenericResult<IOdbcDataSource> TestConnection();

        /// <summary>
        /// Creates an ODBC connection to the Data Source
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        DbConnection CreateConnection();
    }
}