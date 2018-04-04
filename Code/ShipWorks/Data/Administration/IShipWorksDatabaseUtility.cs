using System.Collections.Generic;
using System.Data.Common;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Utility class for creating new empty databases
    /// </summary>
    public interface IShipWorksDatabaseUtility
    {
        /// <summary>
        /// Get all of the details about all of the databases on the instance of the connection
        /// </summary>
        IEnumerable<SqlDatabaseDetail> GetDatabaseDetails(DbConnection con);
    }
}