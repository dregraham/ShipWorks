using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
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
        void Load(IOdbcDataSource dataSource);

        /// <summary>
        /// Sets tables to be a single table representing the schema of this query.
        /// </summary>
        void Load(IOdbcDataSource dataSource, string query);
    }
}