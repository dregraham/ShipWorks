using System.Data.Common;
using System.Data.Odbc;

namespace ShipWorks.Stores.Platforms.Odbc
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
        /// Sends the System.Data.Odbc.OdbcCommand.CommandText to the System.Data.Odbc.OdbcCommand.Connection
        /// and builds an System.Data.Odbc.OdbcDataReader.
        /// </summary>
        /// <returns>
        /// An System.Data.Odbc.OdbcDataReader object.
        /// </returns>
        public DbDataReader ExecuteReader()
        {
            return command.ExecuteReader();
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