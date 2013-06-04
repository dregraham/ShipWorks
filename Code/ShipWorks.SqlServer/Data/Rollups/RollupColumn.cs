using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.SqlServer.Data.Rollups
{
    /// <summary>
    /// Defines a column to be rolled up
    /// </summary>
    public class RollupColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RollupColumn()
        {
            IsBitField = false;
        }

        /// <summary>
        /// The name of the column on the child table.  This would be like the "Name" column on OrderItem.
        /// </summary>
        public string SourceColumn { get; set; }

        /// <summary>
        /// The list of columns that change the value of the SourceColumn.  Only applicable for computed columns.
        /// </summary>
        public List<string> SourceDependencies { get; set; }

        /// <summary>
        /// The name of the column of the parent table that will hold the results of the rollup.  This would be like the "RollupItemName" column on the order table.
        /// </summary>
        public string TargetColumn { get; set; }

        /// <summary>
        /// The rollup method\strategy
        /// </summary>
        public RollupMethod Method { get; set; }

        /// <summary>
        /// How the rollup is implemented is dependant on if it's a bit or not
        /// </summary>
        public bool IsBitField { get; set; }
    }
}
