using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Filters;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls.SandGrid;

namespace ShipWorks.Filters.Controls
{
    public delegate void FilterDragDropCompleteEventHandler(object sender, FilterDragDropCompleteEventArgs e);

    public class FilterDragDropCompleteEventArgs : EventArgs
    {
        FilterNodeEntity node;
        FilterLocation moveLocation;
        FilterLocation copyLocation;

        // The original args that triggered this one
        GridRowDroppedEventArgs dropArgs;

        /// <summary>
        /// Construtor
        /// </summary>
        public FilterDragDropCompleteEventArgs(FilterNodeEntity node, FilterLocation moveLocation, FilterLocation copyLocation, GridRowDroppedEventArgs dropArgs)
        {
            this.node = node;
            this.moveLocation = moveLocation;
            this.copyLocation = copyLocation;
            this.dropArgs = dropArgs;
        }

        /// <summary>
        /// The node that is being dropped
        /// </summary>
        public FilterNodeEntity FilterNode
        {
            get { return node; }
        }

        /// <summary>
        /// The location in the tree where the item should go if moved
        /// </summary>
        public FilterLocation MoveLocation
        {
            get { return moveLocation; }
        }

        /// <summary>
        /// The location in the tree where the item should go if copied
        /// </summary>
        public FilterLocation CopyLocation
        {
            get { return copyLocation; }
        }

        /// <summary>
        /// The state of the CTRL SHIIFT ALT keys, and mouse buttons
        /// </summary>
        public int KeyState
        {
            get { return dropArgs.KeyState; }
        }

        /// <summary>
        /// Indicates if the drop insert position indicator should be automatically cleared
        /// </summary>
        public bool AutoClearDropIndicator
        {
            get
            {
                return dropArgs.AutoClearDropIndicator;
            }
            set
            {
                dropArgs.AutoClearDropIndicator = value;
            }
        }

        /// <summary>
        /// Used to clear the drop indiciator, when AutoClearDropIndicator was false
        /// </summary>
        public void ClearDropIndicator()
        {
            dropArgs.ClearDropIndicator();
        }
    }
}
