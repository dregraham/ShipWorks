using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Retrieves DSN names
    /// </summary>
    public interface IDsnProvider
    {
        /// <summary>
        /// Gets the names
        /// </summary>
        IEnumerable<string> GetDataSourceNames();
    }
}