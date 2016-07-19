using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
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
        /// Executes the query and returns the number or rows affected
        /// </summary>
        int ExecuteNonQuery();

        /// <summary>
        /// Sets the command text
        /// </summary>
        /// <param name="sql"></param>
        void ChangeCommandText(string sql);

        /// <summary>
        /// Adds the given parameter to the command
        /// </summary>
        void AddParameter(string parameterName, OdbcType type, object value);

        /// <summary>
        /// Tries to cancel the execution of the OdbcCommand
        /// </summary>
        void Cancel();
    }
}