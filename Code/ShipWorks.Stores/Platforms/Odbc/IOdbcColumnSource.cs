using log4net;
using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc
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
        void Load(IOdbcDataSource dataSource, ILog logFactory);

        /// <summary>
        /// Loads the columns for the column source
        /// </summary>
        void Load(OdbcDataSource dataSource, ILog log, string query, IShipWorksDbProviderFactory dbProviderFactory);
    }
}