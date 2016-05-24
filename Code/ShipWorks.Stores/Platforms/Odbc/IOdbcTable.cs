using System.Collections.Generic;
using log4net;

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
        IEnumerable<OdbcColumn> Columns { get; }

        /// <summary>
        /// The tables name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Resets the columns.
        /// </summary>
        void ResetColumns();

        void Load(IOdbcDataSource dataSource, ILog logFactory);
    }
}