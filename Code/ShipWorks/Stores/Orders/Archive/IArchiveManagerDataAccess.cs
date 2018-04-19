using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Administration;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Interact with the database for managing archives
    /// </summary>
    public interface IArchiveManagerDataAccess
    {
        /// <summary>
        /// Change the database
        /// </summary>
        bool ChangeDatabase(ISqlDatabaseDetail selectedArchive);

        /// <summary>
        /// Get a list of archive databases for the current database
        /// </summary>
        Task<IEnumerable<ISqlDatabaseDetail>> GetArchiveDatabases();

        /// <summary>
        /// Get the live database
        /// </summary>
        Task<ISqlDatabaseDetail> GetLiveDatabase();
    }
}