using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Grid = Divelements.SandGrid.SandGrid;
using Interapptive.Shared;
using System.Drawing;
using System.ComponentModel;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using ShipWorks.Common.Threading;

namespace ShipWorks.UI.Controls.SandGrid
{
    /// <summary>
    /// Provides the ability to show a context menu for grid columns in a SandGrid
    /// </summary>
    public class ColumnContextMenuProvider : NativeWindow
    {
        Grid sandGrid;

        public event ColumnContextMenuEventHandler ShowContextMenu;

        /// <summary>
        /// Constructor
        /// </summary>
        public ColumnContextMenuProvider(Grid sandGrid)
        {
            this.sandGrid = sandGrid;

            if (sandGrid.IsHandleCreated)
            {
                AssignHandle(sandGrid.Handle);
            }

            sandGrid.HandleCreated += new EventHandler(this.OnHandleCreated);
            sandGrid.HandleDestroyed += new EventHandler(this.OnHandleDestroyed);
        }

        /// <summary>
        /// The handle of the owned control is being created
        /// </summary>
        void OnHandleCreated(object sender, EventArgs e)
        {
            // Window is now created, assign handle to NativeWindow.
            AssignHandle(((Control)sender).Handle);
        }

        /// <summary>
        /// The handle of the owned control is being destroyed
        /// </summary>
        void OnHandleDestroyed(object sender, EventArgs e) 
        {
            // Window was destroyed, release hook.
            ReleaseHandle();
        }

        /// <summary>
        /// Intercept messages
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_CONTEXTMENU)
            {
                if (WmContextMenu(ref m))
                {
                    return;
                }
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Handle WM_CONTEXTMENU message
        /// </summary>
        private bool WmContextMenu(ref Message m)
        {
            Point point;
            int x = NativeMethods.SignedLOWORD(m.LParam);
            int y = NativeMethods.SignedHIWORD(m.LParam);

            if (((int) ((long) m.LParam)) == -1)
            {
                point = new Point(sandGrid.Width / 2, sandGrid.Height / 2);
            }
            else
            {
                point = sandGrid.PointToClient(new Point(x, y));
            }

            bool showMenu = false;

            // To show it has to be within client area...
            if (sandGrid.ClientRectangle.Contains(point))
            {
                // And if there are any columns, has to be in the header area
                if (sandGrid.Columns.DisplayColumns.Length > 0)
                {
                    Point gridPoint = sandGrid.PointToGrid(point);
                    if (sandGrid.Columns.DisplayColumns[0].Bounds.Top < gridPoint.Y && sandGrid.Columns.DisplayColumns[0].Bounds.Bottom > gridPoint.Y)
                    {
                        showMenu = true;
                    }
                }
                // If there are no visible columns, it can be anywhere
                else
                {
                    showMenu = true;
                }
            }

            if (showMenu)
            {
                if (ShowContextMenu != null)
                {
                    sandGrid.BeginInvoke(new MethodInvoker<Point>(RaiseShowContextMenu), point);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Raise the event to show the context menu
        /// </summary>
        public void RaiseShowContextMenu(Point point)
        {
            if (ShowContextMenu != null)
            {
                ShowContextMenu(this, new ColumnContextMenuEventArgs(point));
            }
        }
    }
}
