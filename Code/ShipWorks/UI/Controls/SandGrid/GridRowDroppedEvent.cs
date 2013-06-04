using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Filters;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.UI.Controls.SandGrid
{
    public delegate void GridRowDroppedEventHandler(object sender, GridRowDroppedEventArgs e);

    public class GridRowDroppedEventArgs : EventArgs
    {
        SandGridDragDrop grid;

        SandGridDragDropRow sourceRow;
        SandGridDragDropRow targetRow;

        int keyState;

        bool autoClearDropIndicator = true;

        /// <summary>
        /// Construtor
        /// </summary>
        public GridRowDroppedEventArgs(SandGridDragDrop grid, SandGridDragDropRow sourceRow, SandGridDragDropRow targetRow, int keyState)
        {
            this.grid = grid;
            this.sourceRow = sourceRow;
            this.targetRow = targetRow;
            this.keyState = keyState;
        }

        /// <summary>
        /// The row that was dragged and is being dropped
        /// </summary>
        public SandGridDragDropRow SourceRow
        {
            get { return sourceRow; }
        }

        /// <summary>
        /// The row that the SourceRow is being dropped on.
        /// </summary>
        public SandGridDragDropRow TargetRow
        {
            get { return targetRow; }
        }

        /// <summary>
        /// Where the item is being dropped relative to the target row
        /// </summary>
        public DropTargetState RelativeLocation
        {
            get { return targetRow.DragDropState; }
        }

        /// <summary>
        /// The state of the CTRL SHIIFT ALT keys, and mouse buttons
        /// </summary>
        public int KeyState
        {
            get { return keyState; }
        }

        /// <summary>
        /// Indicates if the drop insert position indicator should be automatically cleared
        /// </summary>
        public bool AutoClearDropIndicator
        {
            get
            {
                return autoClearDropIndicator;
            }
            set
            {
                autoClearDropIndicator = value;
            }
        }

        /// <summary>
        /// Used to clear the drop indiciator, when AutoClearDropIndicator was false
        /// </summary>
        public void ClearDropIndicator()
        {
            grid.ClearDropIndicator();
        }
    }
}
