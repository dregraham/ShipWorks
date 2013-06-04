using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns;

namespace ShipWorks.Data.Grid
{
    /// <summary>
    /// Delegate for event handler fo rthe grid hyplink click event
    /// </summary>
    public delegate void GridHyperlinkClickEventHandler(object sender, GridHyperlinkClickEventArgs e);

    /// <summary>
    /// EventArgs for the GridHyperlinkClick event
    /// </summary>
    public class GridHyperlinkClickEventArgs : EventArgs
    {
        EntityGridRow row;
        EntityGridColumn column;

        bool handled = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridHyperlinkClickEventArgs(EntityGridRow row, EntityGridColumn column)
        {
            this.row = row;
            this.column = column;
        }

        /// <summary>
        /// The column of the cell
        /// </summary>
        public EntityGridColumn Column
        {
            get { return column; }
        }

        /// <summary>
        /// The row of the cell
        /// </summary>
        public EntityGridRow Row
        {
            get { return row; }
        }

        /// <summary>
        /// Can be set to false by an event handler to indicate the link was not handled
        /// </summary>
        public bool Handled
        {
            get { return handled; }
            set { handled = value; }
        }
    }
}
