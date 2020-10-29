using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Installer.Sql;

namespace ShipWorks.Installer.Services
{
    public interface ISqlServerLookupService
    {
        /// <summary>
        /// Gets a list of databases from the provided server instance
        /// </summary>
        Task<IEnumerable<SqlSessionConfiguration>> GetDatabases(string serverInstance, string username = "", string password = "");

        /// <summary>
        /// Test a connection to a database
        /// </summary>
        Task<bool> TestConnection(SqlSessionConfiguration config);
    }
}