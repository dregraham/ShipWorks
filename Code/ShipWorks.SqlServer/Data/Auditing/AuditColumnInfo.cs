using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.SqlServer.Data.Auditing
{
    /// <summary>
    /// Information we store on each column we need to audit, and is cached
    /// </summary>
    class AuditColumnInfo
    {
        /// <summary>
        /// The internal SQL Server ID for this column
        /// </summary>
        public long ColumnID
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the column
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// The ordinal position of the column in the table
        /// </summary>
        public int Ordinal
        {
            get;
            set;
        }

        /// <summary>
        /// The SQL server data type of the column
        /// </summary>
        public byte DataType
        {
            get;
            set;
        }

        /// <summary>
        /// How this column name will be displayed in ShipWorks
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// The ShipWorks specific way to display data for this column
        /// </summary>
        public byte DisplayFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates if the column should be stored in the Text* audit fields, vs. the Variant ones.
        /// </summary>
        public bool IsText
        {
            get { return DataType == 231 || DataType == 241; }
        }
    }
}
