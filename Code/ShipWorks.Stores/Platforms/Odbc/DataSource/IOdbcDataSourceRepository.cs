﻿using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource
{
    /// <summary>
    /// Repository for OdbcDataSources
    /// </summary>
    public interface IOdbcDataSourceRepository
    {
        /// <summary>
        /// Gets the available data sources.
        /// </summary>
        IEnumerable<IOdbcDataSource> GetDataSources();
    }
}