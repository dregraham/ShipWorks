﻿using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource.Schema
{
    /// <summary>
    /// Represents an ODBC Schema which contains the ODBC Data Source's tables
    /// </summary>
    public interface IOdbcSchema
    {
        /// <summary>
        /// ODBC Tables in this schema
        /// </summary>
        IEnumerable<IOdbcColumnSource> Tables { get; }

        /// <summary>
        /// Load the given DataSource
        /// </summary>
        /// <exception cref="ShipWorksOdbcException"/>
        void Load(IOdbcDataSource dataSource);
    }
}