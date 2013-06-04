using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Represents the position of a label on a sheet
    /// </summary>
    public class LabelPosition
    {
        int row = 1;
        int column = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelPosition()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelPosition(int column, int row)
        {
            this.column = column;
            this.row = row;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public LabelPosition(LabelPosition other)
        {
            this.column = other.column;
            this.row = other.row;
        }

        /// <summary>
        /// The row
        /// </summary>
        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        /// <summary>
        /// The column
        /// </summary>
        public int Column
        {
            get { return column; }
            set { column = value; }
        }
    }
}
