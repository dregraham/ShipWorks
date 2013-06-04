using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using ShipWorks.Filters.Controls;
using System.Drawing;
using Divelements.SandGrid.Rendering;

namespace ShipWorks.UI.Controls.SandGrid
{
    /// <summary>
    /// Customized grid row 
    /// </summary>
    public class SandGridDragDropRow : GridRow
    {
        bool hotTrack = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public SandGridDragDropRow()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SandGridDragDropRow(string text)
            : base(text)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SandGridDragDropRow(string text, Image image)
            : base(text, image)
        {

        }

        // Constructor
        public SandGridDragDropRow(string[] cellText)
            : base(cellText)
        {

        }

        /// <summary>
        /// Indicates if the row represnts a folder that can contain other items
        /// </summary>
        public virtual bool IsFolder
        {
            get { return false; }
        }

        /// <summary>
        /// Indicates if the row can be dragged
        /// </summary>
        public virtual bool IsDraggable
        {
            get { return true; }
        }

        /// <summary>
        /// Indicates if its valid to drop the specified row on this row at the given location
        /// </summary>
        public virtual bool IsValidDrop(SandGridDragDropRow sourceRow, DropTargetState state)
        {
            return true;
        }

        /// <summary>
        /// The current state of this row as a drag and drop target
        /// </summary>
        public DropTargetState DragDropState
        {
            get
            {
                SandGridDragDrop grid = null;

                if (Grid != null)
                {
                    grid = Grid.SandGrid as SandGridDragDrop;
                }
                
                if (grid == null)
                {
                    return DropTargetState.None;
                }

                if (grid.DragDropTargetRow == this)
                {
                    return grid.DragDropState;
                }

                return DropTargetState.None;
            }
        }

        /// <summary>
        /// Force a redraw of the raw due to a dragdrop state change
        /// </summary>
        public void UpdateDragDropDisplay()
        {
            RedrawNeeded();
        }

        /// <summary>
        /// Draw the background of the row
        /// </summary>
        protected override void DrawRowBackground(RenderingContext context)
        {
            base.DrawRowBackground(context);

            if (hotTrack)
            {
                context.Renderer.DrawSelectionRectangle(context.Graphics, Bounds, true, true, false);
            }
        }

        /// <summary>
        /// Indicates if the row should be drawn "hot"
        /// </summary>
        public bool HotTrack
        {
            get
            {
                return hotTrack;
            }
            set
            {
                if (hotTrack != value)
                {
                    hotTrack = value;

                    RedrawNeeded();
                }
            }
        }

        /// <summary>
        /// Indicates if this row can be the terminating selection of a HotTrack operation.
        /// </summary>
        public virtual bool IsValidHotTrackSelection
        {
            get { return true; }
        }
    }
}
