using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource
{
    /// <summary>
    /// An interface intended to provide data source name (DSN) related information
    /// </summary>
    public interface IDsnProvider
    {
        /// <summary>
        /// Returns an IEnumerable of DSNs
        /// </summary>
        IEnumerable<string> GetDataSourceNames();
    }
}