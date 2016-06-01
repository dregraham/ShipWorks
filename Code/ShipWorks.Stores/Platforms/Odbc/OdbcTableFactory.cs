using System;
using log4net;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Factory for resolving all of the dependencies needed to create an ODBC Table
    /// </summary>
    public class OdbcTableFactory
    {
        /// <summary>
        /// Creates an ODBC table with the given schema and name
        /// </summary>
        public OdbcTable CreateTable(string tableName)
        {
            return new OdbcTable(tableName);
        }
    }
}