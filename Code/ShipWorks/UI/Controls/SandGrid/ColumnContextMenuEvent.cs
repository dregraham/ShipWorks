using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace ShipWorks.UI.Controls.SandGrid
{
    /// <summary>
    /// Delegate for the ColumnContextMenu event
    /// </summary>
    public delegate void ColumnContextMenuEventHandler(object sender, ColumnContextMenuEventArgs e);

    /// <summary>
    /// EventArgs for the ColumnContextMenu event
    /// </summary>
    public class ColumnContextMenuEventArgs : EventArgs
    {
        Point clientPoint;

        /// <summary>
        /// Constructor
        /// </summary>
        public ColumnContextMenuEventArgs(Point clientPoint)
        {
            this.clientPoint = clientPoint;
        }

        /// <summary>
        /// The point where the menu should be shown
        /// </summary>
        public Point ClientPoint
        {
            get { return clientPoint; }
        }
    }
}
