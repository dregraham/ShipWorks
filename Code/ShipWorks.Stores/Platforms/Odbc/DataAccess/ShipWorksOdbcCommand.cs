using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Wrapper for OdbcCommand. Needed for unit testing.
    /// </summary>
    public class ShipWorksOdbcCommand : IShipWorksOdbcCommand
    {
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly OdbcCommand command;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcCommand"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="apiLogEntryFactory"></param>
        public ShipWorksOdbcCommand(string query, OdbcConnection connection, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.apiLogEntryFactory = apiLogEntryFactory;
            command = new OdbcCommand(query, connection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcCommand"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="apiLogEntryFactory"></param>
        public ShipWorksOdbcCommand(OdbcConnection connection, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.apiLogEntryFactory = apiLogEntryFactory;
            command = connection.CreateCommand();
        }

        /// <summary>
        /// Sends the System.Data.Odbc.OdbcCommand.CommandText to the System.Data.Odbc.OdbcCommand.Connection
        /// and builds an System.Data.Odbc.OdbcDataReader.
        /// </summary>
        /// <returns>
        /// An System.Data.Odbc.OdbcDataReader object.
        /// </returns>
        public DbDataReader ExecuteReader()
        {
            IApiLogEntry apiLogger = apiLogEntryFactory(ApiLogSource.Odbc, "Read");
            apiLogger.LogRequest(command.CommandText, "log");

            return command.ExecuteReader();
        }

        /// <summary>
        /// Sends the System.Data.Odbc.OdbcCommand.CommandText to the System.Data.Odbc.OdbcCommand.Connection
        /// and builds an System.Data.Odbc.OdbcDataReader using one of the CommandBehavior values
        /// </summary>
        /// <returns>
        /// An System.Data.Odbc.OdbcDataReader object.
        /// </returns>
        public DbDataReader ExecuteReader(CommandBehavior commandBehavior)
        {
            IApiLogEntry apiLogger = apiLogEntryFactory(ApiLogSource.Odbc, "Read");
            apiLogger.LogRequest(command.CommandText, "log");

            return command.ExecuteReader();
        }

        /// <summary>
        /// Executes the query and returns the number or rows affected
        /// </summary>
        public int ExecuteNonQuery()
        {
            IApiLogEntry apiLogger = apiLogEntryFactory(ApiLogSource.Odbc, "Write");
            apiLogger.LogRequest(command.CommandText, "log");

            int recordsAffected = command.ExecuteNonQuery();
            apiLogger.LogResponse($"{recordsAffected} records affected", "log");

            return recordsAffected;
        }

        /// <summary>
        /// Sets the command text of the command
        /// </summary>
        public void ChangeCommandText(string sql)
        {
            command.CommandText = sql;
        }

        /// <summary>
        /// Adds the given parameter to the command
        /// </summary>
        public void AddParameter(OdbcParameter parameter)
        {
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// Tries to cancel the execution of the OdbcCommand
        /// </summary>
        public void Cancel()
        {
            command.Cancel();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            command.Dispose();
        }
    }
}