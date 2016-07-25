using log4net;
using System;
using System.Data.Common;
using System.Data.Odbc;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Class for obtaining odbc resources with interface that can be mocked up for testing purposes
    /// </summary>
    public class OdbcShipWorksDbProviderFactory : IShipWorksDbProviderFactory
    {
        private readonly ILog log;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;

        public OdbcShipWorksDbProviderFactory(Func<Type, ILog> logFactory, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.apiLogEntryFactory = apiLogEntryFactory;
            log = logFactory(typeof(OdbcShipWorksDbProviderFactory));
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
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        public DbConnection CreateOdbcConnection(string connectionString)
        {
            DbConnection connection = CreateOdbcConnection();
            try
            {
                connection.ConnectionString = connectionString;
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
                throw new ShipWorksOdbcException(ex);
            }
            return connection;
        }

        /// <summary>
        /// Creates an ODBC command from the given query and connection
        /// </summary>
        public IShipWorksOdbcCommand CreateOdbcCommand(string query, DbConnection connection)
        {
            return new ShipWorksOdbcCommand(query, (OdbcConnection) connection, apiLogEntryFactory);
        }

        /// <summary>
        /// Creates an ODBC command from the given query and connection
        /// </summary>
        public IShipWorksOdbcCommand CreateOdbcCommand(DbConnection connection)
        {
            return new ShipWorksOdbcCommand((OdbcConnection)connection, apiLogEntryFactory);
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
