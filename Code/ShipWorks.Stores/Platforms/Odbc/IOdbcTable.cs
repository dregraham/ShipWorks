using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Represents an ODBC table with columns and name
    /// </summary>
    public interface IOdbcTable
    {
        /// <summary>
        /// Columns that belong to the table
        /// </summary>
        IEnumerable<OdbcColumn> Columns { get; set; }

        /// <summary>
        /// The tables name
        /// </summary>
        string Name { get; }
    }
}