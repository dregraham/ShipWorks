using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Interface of DsnProvider
    /// </summary>
    public interface IDsnProvider
    {
        /// <summary>
        /// Gets the names
        /// </summary>
        IEnumerable<string> GetDataSourceNames();
    }
}