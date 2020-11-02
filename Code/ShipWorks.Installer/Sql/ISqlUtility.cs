using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace ShipWorks.Installer.Sql
{
    /// <summary>
    /// Interface for SqlUtility
    /// </summary>
    public interface ISqlUtility
    {
        /// <summary>
        /// The name given to the special default instance of sql server
        /// </summary>
        string DefaultInstanceName { get; }

        /// <summary>
        /// The default password ShipWorks uses for sa when it installs new SQL instances
        /// </summary>
        string ShipWorksSaPassword { get; }

        /// <summary>
        /// See if we can figure out the credentials necessary to connect to the given instance.  If provided, the configuration given in firstTry will be attempted first
        /// </summary>
        SqlSessionConfiguration DetermineCredentials(string instance, SqlSessionConfiguration firstTry = null);

        /// <summary>
        /// Get all of the details about all of the databases on the instance of the connection
        /// </summary>
        Task<IEnumerable<string>> GetDatabaseDetails(DbConnection con);

        /// <summary>
        /// Validates that an open connection is actually open.  With connection pooling, a connection in the Open state
        /// may not actually have a real connection to the database if the database has gone down or the network has dropped.
        ///
        /// This also serves to reset the connection isolation level to READ COMMITTED for every connection, as the connection pool
        /// does not do this! http://support.microsoft.com/kb/309544
        ///
        /// </summary>
        bool ValidateOpenConnection(DbConnection con);
    }
}