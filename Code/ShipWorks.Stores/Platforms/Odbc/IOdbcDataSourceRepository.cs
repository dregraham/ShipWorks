using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Repository for OdbcDataSources
    /// </summary>
    public interface IOdbcDataSourceRepository : IDisposable
    {
        /// <summary>
        /// Gets the available data sources.
        /// </summary>
        IEnumerable<OdbcDataSource> GetDataSources();
    }
}