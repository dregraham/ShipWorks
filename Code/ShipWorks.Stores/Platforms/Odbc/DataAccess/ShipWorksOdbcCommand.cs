using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;

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
            apiLogger.LogRequest(BuildLoggedCommandText(), "log");

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
            apiLogger.LogRequest(BuildLoggedCommandText(), "log");

            return command.ExecuteReader(commandBehavior);
        }

        /// <summary>
        /// Executes the query and returns the number or rows affected
        /// </summary>
        public int ExecuteNonQuery()
        {
            IApiLogEntry apiLogger = apiLogEntryFactory(ApiLogSource.Odbc, "Write");
            apiLogger.LogRequest(BuildLoggedCommandText(), "log");

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
        /// Adds the given parameter to the command
        /// </summary>
        public void AddParameter(string name, object value)
        {
            command.Parameters.AddWithValue(name, value);
        }

        /// <summary>
        /// Uses the configuration of the ODBC command (command text and any parameters) to build
        /// a string that can be used for logging the interaction with the database.
        /// </summary>
        /// <returns>A formatted string containing the command text along with the parameter names/values.</returns>
        private string BuildLoggedCommandText()
        {
            StringBuilder commandData = new StringBuilder(command.CommandText);

            commandData.Append(Environment.NewLine);
            commandData.Append("Parameters: ");

            // Write out "(none)" when there aren't any parameters; otherwise turn
            // the command's parameters into a comma separated list in the format
            // of [ParameterName] = [Value]. (Sonar Lint prevented compilation if
            // this didn't used the ternary operator.)
            commandData.Append(command.Parameters.Count == 0 ?
                "(none)" :
                string.Join(", ", command.Parameters
                .Cast<OdbcParameter>()
                .ToDictionary(parameter => parameter.ParameterName,
                    parameter => parameter.Value?.ToString() ?? "null")
                .Select(kvp => kvp.Key + " = " + kvp.Value)));

            return commandData.ToString();
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