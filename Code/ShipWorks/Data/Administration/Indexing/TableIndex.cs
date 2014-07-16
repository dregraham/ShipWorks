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
        /// Initializes a new instance of the <see cref="TableIndex"/> class.
        /// </summary>
        public TableIndex()
        {
            TableName = string.Empty;
            IndexName = string.Empty;
        }

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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return TableName + "." + IndexName;
        }
    }
}
