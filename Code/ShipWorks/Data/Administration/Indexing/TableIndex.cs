using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.Indexing
{
    /// <summary>
    /// DTO to hold TableIndex Information.
    /// </summary>
    public class TableIndex
    {
        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        public string TableName
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the name of the index.
        /// </summary>
        public string IndexName
        {
            get; 
            set;
        }
    }
}
