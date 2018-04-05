using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Utility class for creating new empty databases
    /// </summary>
    [Component]
    public class ShipWorksDatabaseUtilityWrapper : IShipWorksDatabaseUtility
    {
        /// <summary>
        /// Get all of the details about all of the databases on the instance of the connection
        /// </summary>
        public Task<IEnumerable<ISqlDatabaseDetail>> GetDatabaseDetails(DbConnection con) =>
            ShipWorksDatabaseUtility.GetDatabaseDetails(con);
    }
}
