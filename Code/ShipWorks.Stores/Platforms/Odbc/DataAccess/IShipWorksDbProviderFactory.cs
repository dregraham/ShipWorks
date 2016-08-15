using System.Data.Common;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Interface for getting odbc resources, can be mocked up for testing
    /// </summary>
    public interface IShipWorksDbProviderFactory
    {
        /// <summary>
        /// Creates an Odbc DbConnection
        /// </summary>
        DbConnection CreateOdbcConnection();

        /// <summary>
        /// Creates an Odbc DbConnection with a given connection string
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        DbConnection CreateOdbcConnection(string connectionString);

        /// <summary>
        /// Creates a ShipWorksOdbcCommand from the given query and connection
        /// </summary>
        IShipWorksOdbcCommand CreateOdbcCommand(string query, DbConnection connection);

        /// <summary>
        /// Creates a ShipWorksOdbcCommand from the given connection
        /// </summary>
        IShipWorksOdbcCommand CreateOdbcCommand(DbConnection connection);

        /// <summary>
        /// Creates a ShipWorks ODBC command builder.
        /// </summary>
        IShipWorksOdbcCommandBuilder CreateShipWorksOdbcCommandBuilder(IShipWorksOdbcDataAdapter adapter);

        /// <summary>
        /// Creates a ShipWorks ODBC data adapter.
        /// </summary>
        IShipWorksOdbcDataAdapter CreateShipWorksOdbcDataAdapter(string selectCommandText, DbConnection selectConnection);
    }
}