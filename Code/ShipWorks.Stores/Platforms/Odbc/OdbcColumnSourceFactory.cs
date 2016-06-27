using System;
using log4net;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Factory for resolving all of the dependencies needed to create an ODBC Table
    /// </summary>
    public class OdbcColumnSourceFactory
    {
        /// <summary>
        /// Creates an ODBC table with the given schema and name
        /// </summary>
        public OdbcColumnSource CreateTable(string tableName)
        {
            return new OdbcColumnSource(tableName);
        }
    }
}