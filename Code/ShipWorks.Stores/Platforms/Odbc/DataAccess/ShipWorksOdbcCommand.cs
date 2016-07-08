﻿using System.Data;
using System.Data.Common;
using System.Data.Odbc;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Wrapper for OdbcCommand. Needed for unit testing.
    /// </summary>
    public class ShipWorksOdbcCommand : IShipWorksOdbcCommand
    {
        private readonly OdbcCommand command;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcCommand"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connection">The connection.</param>
        public ShipWorksOdbcCommand(string query, OdbcConnection connection)
        {
            command = new OdbcCommand(query, connection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcCommand"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public ShipWorksOdbcCommand(OdbcConnection connection)
        {
            command = connection.CreateCommand();
        }

        /// <summary>
        /// Gets the command text
        /// </summary>
        public string CommandText => command?.CommandText;

        /// <summary>
        /// Sends the System.Data.Odbc.OdbcCommand.CommandText to the System.Data.Odbc.OdbcCommand.Connection
        /// and builds an System.Data.Odbc.OdbcDataReader.
        /// </summary>
        /// <returns>
        /// An System.Data.Odbc.OdbcDataReader object.
        /// </returns>
        public DbDataReader ExecuteReader() => command.ExecuteReader();

        /// <summary>
        /// Sends the System.Data.Odbc.OdbcCommand.CommandText to the System.Data.Odbc.OdbcCommand.Connection
        /// and builds an System.Data.Odbc.OdbcDataReader using one of the CommandBehavior values
        /// </summary>
        /// <returns>
        /// An System.Data.Odbc.OdbcDataReader object.
        /// </returns>
        public DbDataReader ExecuteReader(CommandBehavior commandBehavior) => command.ExecuteReader(commandBehavior);

        /// <summary>
        /// Sets the command text of the command
        /// </summary>
        public void SetCommandText(string sql)
        {
            command.CommandText = sql;
        }

        /// <summary>
        /// Adds the given parameter to the command
        /// </summary>
        public void AddParameter(string parameterName, OdbcType type, object value)
        {
            command.Parameters.Add(parameterName, type).Value = value;
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