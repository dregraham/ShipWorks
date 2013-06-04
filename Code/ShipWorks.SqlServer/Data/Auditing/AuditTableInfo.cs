using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.SqlServer.Data.Auditing
{
    /// <summary>
    /// Information on how to audit data for a table
    /// </summary>
    class AuditTableInfo
    {
        Dictionary<string, AuditColumnInfo> columnInfo = new Dictionary<string, AuditColumnInfo>();

        /// <summary>
        /// The SQL Server object_id of the table
        /// </summary>
        public long TableID
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the table
        /// </summary>
        public string TableName
        {
            get;
            set;
        }

        /// <summary>
        /// The primary key column of the table
        /// </summary>
        public string PrimaryKey
        {
            get;
            set;
        }

        /// <summary>
        /// Dictionary that maps column names to their auditing information
        /// </summary>
        public Dictionary<string, AuditColumnInfo> ColumnInfo
        {
            get { return columnInfo; }
        }
    }
}
