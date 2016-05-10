using System;
using log4net;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Factory for resolving all of the dependencies needed to create an ODBC Table
    /// </summary>
    public class OdbcTableFactory
    {
        private readonly Func<Type, ILog> logFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcTableFactory(Func<Type, ILog> logFactory)
        {
            this.logFactory = logFactory;
        }

        /// <summary>
        /// Creates an ODBC table with the given schema and name
        /// </summary>
        public OdbcTable CreateTable(OdbcSchema schema, string tableName)
        {
            return new OdbcTable(schema, tableName, logFactory(typeof(OdbcTable)));
        }
    }
}