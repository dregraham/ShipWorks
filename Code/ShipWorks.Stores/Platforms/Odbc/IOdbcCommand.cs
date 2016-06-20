using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Interface for an OdbcCommand.
    /// </summary>
    public interface IOdbcCommand
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        IEnumerable<OdbcRecord> Execute();
    }
}