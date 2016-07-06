using System.Collections.Generic;
using System.Reflection;
using log4net;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;

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
        /// The tables name
        /// </summary>
        [Obfuscation(Exclude = true)]
        string Name { get; }

        /// <summary>
        /// Loads the columns for the column source
        /// </summary>
        void Load(IOdbcDataSource dataSource, ILog log);

        /// <summary>
        /// Loads the columns for the column source
        /// </summary>
        void Load(IOdbcDataSource dataSource, ILog log, string query, IShipWorksDbProviderFactory dbProviderFactory);

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        string Query { get; set; }
    }
}