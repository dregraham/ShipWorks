using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Represents data about a single table that has an identity column in SQL Server
    /// </summary>
    public class SqlTableSeed
    {
        int objectID;
        string table;
        long seedValue;
        long? lastValue;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlTableSeed(int objectID, string table, long seedValue, long? lastValue)
        {
            this.objectID = objectID;
            this.table = table;
            this.lastValue = lastValue;
            this.seedValue = seedValue;
        }

        /// <summary>
        /// Gets the SQL Server objectID of the table
        /// </summary>
        public int ObjectID
        {
            get { return objectID; }
        }

        /// <summary>
        /// Gets the table name
        /// </summary>
        public string Table
        {
            get { return table; }
        }

        /// <summary>
        /// The initial seed value configured for the table.
        /// </summary>
        public long SeedValue
        {
            get { return seedValue; }
        }

        /// <summary>
        /// Gets the last seed value given out to a row of the table, or null if no rows have been inserted.
        /// </summary>
        public long? LastValue
        {
            get { return lastValue; }
        }
    }
}
