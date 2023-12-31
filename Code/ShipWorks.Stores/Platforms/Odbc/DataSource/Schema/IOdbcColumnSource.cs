﻿using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource.Schema
{
    /// <summary>
    /// Represents an ODBC table with columns and name
    /// </summary>
    public interface IOdbcColumnSource
    {
        /// <summary>
        /// Columns that belong to the table
        /// </summary>
        IEnumerable<OdbcColumn> Columns { get; }

        /// <summary>
        /// The Column Source Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Loads the columns for the column source
        /// </summary>
        void Load(IOdbcDataSource dataSource, string source, OdbcColumnSourceType sourceType);
    }
}