using System.Data.Common;
using System.Data.Odbc;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Class for obtaining odbc resources with interface that can be mocked up for testing purposes
    /// </summary>
    public class OdbcShipWorksDbProviderFactory : IShipWorksDbProviderFactory
    {
        /// <summary>
        /// Returns an OdbcConnection
        /// </summary>
        public DbConnection CreateOdbcConnection()
        {
            return DbProviderFactories.GetFactory("System.Data.Odbc").CreateConnection();
        }

        /// <summary>
        /// Creates an Odbc DbConnection with a given connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public DbConnection CreateOdbcConnection(string connectionString)
        {
            DbConnection connection = CreateOdbcConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        /// <summary>
        /// Creates an ODBC command from the given query and connection
        /// </summary>
        public IShipWorksOdbcCommand CreateOdbcCommand(string query, DbConnection connection)
        {
            return new ShipWorksOdbcCommand(query, (OdbcConnection) connection);
        }

        /// <summary>
        /// Creates a ShipWorks ODBC command builder.
        /// </summary>
        public IShipWorksOdbcCommandBuilder CreateShipWorksOdbcCommandBuilder(IShipWorksOdbcDataAdapter shipWorksAdapter)
        {
            return new ShipWorksOdbcCommandBuilder(shipWorksAdapter.Adapter);
        }

        /// <summary>
        /// Creates a ShipWorks ODBC data adapter.
        /// </summary>
        public IShipWorksOdbcDataAdapter CreateShipWorksOdbcDataAdapter(string selectCommandText, DbConnection selectConnection)
        {
            return new ShipWorksOdbcDataAdapter(selectCommandText,selectConnection);
        }
    }
}
