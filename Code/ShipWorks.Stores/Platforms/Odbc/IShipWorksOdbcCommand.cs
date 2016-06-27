using System;
using System.Data;
using System.Data.Common;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public interface IShipWorksOdbcCommand : IDisposable
    {
        /// <summary>
        /// Sends the System.Data.Odbc.OdbcCommand.CommandText to the System.Data.Odbc.OdbcCommand.Connection
        /// and builds an System.Data.Odbc.OdbcDataReader.
        /// </summary>
        /// <returns>An System.Data.Odbc.OdbcDataReader object.</returns>
        DbDataReader ExecuteReader();

        /// <summary>
        /// Executes the reader.
        /// </summary>
        DbDataReader ExecuteReader(CommandBehavior commandBehavior);

        /// <summary>
        /// Tries to cancel the execution of the OdbcCommand
        /// </summary>
        void Cancel();
    }
}