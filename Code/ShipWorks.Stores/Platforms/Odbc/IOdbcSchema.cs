using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Represents an ODBC Schema which contains the ODBC Data Source's tables
    /// </summary>
    public interface IOdbcSchema
    {
        /// <summary>
        /// The DataSource that this schema is for
        /// </summary>
        IOdbcDataSource DataSource { get; }

        /// <summary>
        /// ODBC Tables in this schema
        /// </summary>
        IEnumerable<OdbcTable> Tables { get; }

        /// <summary>
        /// Load the given DataSource and retrieve a list of its tables
        /// </summary>
        /// <param name="dataSource"></param>
        void Load(IOdbcDataSource dataSource);
    }
}