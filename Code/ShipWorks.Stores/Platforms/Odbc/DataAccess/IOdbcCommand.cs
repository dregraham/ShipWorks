using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Interface for an OdbcCommand.
    /// </summary>
    public interface IOdbcCommand
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        IEnumerable<OdbcRecord> Execute();
    }
}