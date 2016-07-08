using System;
using System.Data.Common;
using System.Data.Odbc;
using log4net;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Class for obtaining odbc resources with interface that can be mocked up for testing purposes
    /// </summary>
    public class OdbcShipWorksDbProviderFactory : IShipWorksDbProviderFactory
    {
        private readonly Func<Type, ILog> logFactory;

        public OdbcShipWorksDbProviderFactory(Func<Type, ILog> logFactory)
        {
            this.logFactory = logFactory;
        }

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
            return new ShipWorksOdbcCommand(query, (OdbcConnection) connection, logFactory(typeof(ShipWorksOdbcCommand)));
        }

        /// <summary>
        /// Creates an ODBC command from the given query and connection
        /// </summary>
        public IShipWorksOdbcCommand CreateOdbcCommand(DbConnection connection)
        {
            return new ShipWorksOdbcCommand((OdbcConnection)connection, logFactory(typeof(ShipWorksOdbcCommand)));
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
            return new ShipWorksOdbcDataAdapter(selectCommandText, selectConnection);
        }
    }
}
