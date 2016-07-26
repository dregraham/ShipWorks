using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Interface for an OdbcCommand.
    /// </summary>
    public interface IOdbcCommand
    {
        /// <summary>
        /// Gets the name of the driver that was used to execute this command.
        /// </summary>
        /// <value>The name of the driver. An empty string is returned if the
        /// command has not yet been executed.</value>
        string Driver { get; }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        IEnumerable<OdbcRecord> Execute();
    }
}