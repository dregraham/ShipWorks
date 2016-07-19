using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using log4net;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Wrapper for OdbcCommand. Needed for unit testing.
    /// </summary>
    public class ShipWorksOdbcCommand : IShipWorksOdbcCommand
    {
        private readonly ILog log;
        private readonly OdbcCommand command;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcCommand"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="log"></param>
        public ShipWorksOdbcCommand(string query, OdbcConnection connection, ILog log)
        {
            this.log = log;
            command = new OdbcCommand(query, connection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcCommand"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="log"></param>
        public ShipWorksOdbcCommand(OdbcConnection connection, ILog log)
        {
            this.log = log;
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
            log.Info(command.CommandText);
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
            log.Info(command.CommandText);
            return command.ExecuteReader(commandBehavior);
        }

        /// <summary>
        /// Executes the query and returns the number or rows affected
        /// </summary>
        public int ExecuteNonQuery()
        {
            throw new System.NotImplementedException();
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