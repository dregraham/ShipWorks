using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ShipWorks.Templates.Controls;
using Divelements.SandGrid;
using System.Linq;

namespace ShipWorks.UI.Controls.SandGrid
{
    /// <summary>
    /// Customized SandGrid for providing Drag & Drop functionality
    /// </summary>
    public partial class SandGridDragDrop : Divelements.SandGrid.SandGrid, ISupportInitialize
    {
        bool allowDrag;

        // The current target of a drag-drop operation
        SandGridDragDropRow dragDropTargetRow;
        DropTargetState dragDropState;

        // Used to detect hovering and screen scrolling.
        SandGridDragDropRow hoverRow = null;

        // Used for scrolling as the mouse gets to an edge
        DateTime timeOfLastAutoScroll = DateTime.UtcNow;

        /// <summary>
        /// Raised when a drag has been succesfully dropped
        /// </summary>
        public event GridRowDroppedEventHandler GridRowDropped;

        // Indiciates if the "nice" looking selection mode is used
        bool themedSelection = true;

        // Indicates if the row under the mouse is always drawn "hot"
        bool hotTracking = false;

        // Help with hot-tracking
        bool suspendSelectionChangeEvent = false;

        // The current target of hot-tracking
        SandGridDragDropRow hotTrackRow;

        /// <summary>
        /// Constructor
        /// </summary>
        public SandGridDragDrop()
        {
            InitializeComponent();

            if (themedSelection)
            {
                Renderer = new SandGridThemedSelectionRenderer();
            }
        }

        #region ISupportInitialize Members

        /// <summary>
        /// BeginInit - nothing to do
        /// </summary>
        void ISupportInitialize.BeginInit()
        {

        }

        /// <summary>
        /// EndInit - Setup renderer
        /// </summary>
        void ISupportInitialize.EndInit()
        {
            if (themedSelection)
            {
                Renderer = new SandGridThemedSelectionRenderer();
            }
        }

        #endregion

        /// <summary>
        /// Indiciates if the "nice" looking selection mode is used
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ThemedSelection
        {
            get
            {
                return themedSelection;
            }
            set
            {
                themedSelection = value;
            }
        }

        /// <summary>
        /// Controls if drag operations can be started in the grid
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool AllowDrag
        {
            get
            {
                return allowDrag;
            }
            set
            {
                allowDrag = value;
            }
        }


        #region Drag Drop

        /// <summary>
        /// A drag-drop operation is begninning
        /// </summary>
        private void OnBeginDrag(object sender, ItemDragEventArgs e)
        {
            if (!allowDrag)
            {
                return;
            }

            if (SelectedElements.Count != 1)
            {
                return;
            }

            SandGridDragDropRow row = SelectedElements[0] as SandGridDragDropRow;

            if (row == null)
            {
                throw new InvalidOperationException("Using non-SandGridDragDropRow derived GridRow in SandGridDragDrop control.");
            }

            if (row.IsDraggable)
            {
                DataObject data = new DataObject(typeof(SandGridDragDropRow).FullName, row);
                DoDragDrop(data, DragDropEffects.All);
            }
        }

        /// <summary>
        /// Currently in a drag drop operation
        /// </summary>
        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (!allowDrag)
            {
                DragDropTargetRow = null;
                return;
            }

            // Node being dragged into us
            SandGridDragDropRow sourceRow = e.Data.GetData(typeof(SandGridDragDropRow).FullName) as SandGridDragDropRow;

            // Not something we know how to accept
            if (sourceRow == null || !FlatRows.OfType<SandGridDragDropRow>().Contains(sourceRow))
            {
                DragDropTargetRow = null;
                return;
            }

            if (DetermineDropTargetRow(e, sourceRow) != null)
            {
                e.Effect = DragDropEffects.Move;
            }

            Point client = PointToClient(new Point(e.X, e.Y));
            Point hit = PointToGrid(client);

            SandGridDragDropRow oldHoverRow = hoverRow;
            hoverRow = GetElementAt(hit) as SandGridDragDropRow;

            // We are over a row
            if (hoverRow != null)
            {
                // If its not the same as the one we were over a bit ago
                if (hoverRow != oldHoverRow)
                {
                    // Reset the hover timer
                    hoverTimer.Stop();
                    hoverTimer.Start();
                }

                TimeSpan timeSinceLastAutoScroll = DateTime.UtcNow - timeOfLastAutoScroll;

                // If the user is near an edge, we automatically scroll them
                if (timeSinceLastAutoScroll > TimeSpan.FromSeconds(.1))
                {
                    int scrollZone = 30;

                    // We also need to see if we are close to an edge
                    if (client.Y + scrollZone > Height && hoverRow.NextVisibleRow != null)
                    {
                        ScrollElementIntoView(hoverRow.NextVisibleRow);
                    }

                    if (client.Y <= scrollZone && hoverRow.PreviousVisibleRow != null)
                    {
                        ScrollElementIntoView(hoverRow.PreviousVisibleRow);
                    }

                    timeOfLastAutoScroll = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// The user let go
        /// </summary>
        private void OnDragDrop(object sender, DragEventArgs e)
        {
            hoverTimer.Stop();

            // Node being dragged into us
            SandGridDragDropRow sourceRow = (SandGridDragDropRow) e.Data.GetData(typeof(SandGridDragDropRow));

            // Not something we know how to accept
            if (sourceRow == null || !FlatRows.OfType<SandGridDragDropRow>().Contains(sourceRow))
            {
                DragDropTargetRow = null;
                return;
            }

            SandGridDragDropRow targetRow = DetermineDropTargetRow(e, sourceRow);
            if (targetRow == null)
            {
                return;
            }

            RaiseOnGridRowDropped(sourceRow, targetRow, e);
        }

        /// <summary>
        /// Determine the drop target row and set its drag drop state
        /// </summary>
        private SandGridDragDropRow DetermineDropTargetRow(DragEventArgs e, SandGridDragDropRow sourceRow)
        {
            DragDropTargetRow = null;

            Point client = PointToClient(new Point(e.X, e.Y));
            Point hit = PointToGrid(client);

            SandGridDragDropRow row = GetElementAt(hit) as SandGridDragDropRow;
            if (row == null)
            {
                return null;
            }

            DropTargetState state = DropTargetState.None;

            // If not a folder, it will either go above or below
            if (!row.IsFolder)
            {
                if (hit.Y < ((row.Bounds.Height / 2) + row.Bounds.Top))
                {
                    state = DropTargetState.DropAbove;
                }
                else
                {
                    state = DropTargetState.DropBelow;
                }
            }

            // For a folder, it can go above, below, or in
            else
            {
                if (hit.Y < ((row.Bounds.Height / 4) + row.Bounds.Top))
                {
                    state = DropTargetState.DropAbove;
                }
                else if (hit.Y > ((row.Bounds.Height * .75) + row.Bounds.Top))
                {
                    if (!row.Expanded || row.NestedRows.Count == 0)
                    {
                        state = DropTargetState.DropBelow;
                    }
                    else
                    {
                        row = (SandGridDragDropRow) row.NestedRows[0];
                        state = DropTargetState.DropAbove;
                    }
                }
                else
                {
                    state = DropTargetState.DropInside;
                }
            }

            // Make sure its a valid drop point
            if (!row.IsValidDrop(sourceRow, state))
            {
                return null;
            }

            DragDropTargetRow = row;
            DragDropState = state;

            return row;
        }

        /// <summary>
        /// Raise the DragDropComplete event
        /// </summary>
        private void RaiseOnGridRowDropped(SandGridDragDropRow sourceRow, SandGridDragDropRow targetRow, DragEventArgs e)
        {
            if (GridRowDropped != null)
            {
                GridRowDroppedEventArgs args = new GridRowDroppedEventArgs(this, sourceRow, targetRow, e.KeyState);
                GridRowDropped(this, args);

                if (args.AutoClearDropIndicator)
                {
                    DragDropTargetRow = null;
                }
            }
            else
            {
                DragDropTargetRow = null;
            }
        }

        /// <summary>
        /// Drag drop is ending
        /// </summary>
        private void OnDragLeave(object sender, EventArgs e)
        {
            DragDropTargetRow = null;
        }

        /// <summary>
        /// User is hovering over a node during a drag operation
        /// </summary>
        private void OnDragHoverTimer(object sender, EventArgs e)
        {
            hoverTimer.Stop();

            if (DragDropTargetRow != null && DragDropTargetRow.NestedRows.Count > 0)
            {
                DragDropTargetRow.Expanded = true;
            }
        }

        /// <summary>
        /// Clear the drop indicator currently being displayed, if any.  Used to clear the indicator if the ClearDropIndicator of
        /// the FilterDragDropCompleteEventArgs from a DragpDropComplete event was set to false.
        /// </summary>
        public void ClearDropIndicator()
        {
            DragDropTargetRow = null;
        }

        /// <summary>
        /// Returns the node that is currently highlighted for a drag-drop operation
        /// </summary>
        [Browsable(false)]
        public SandGridDragDropRow DragDropTargetRow
        {
            get
            {
                return dragDropTargetRow;
            }
            private set
            {
                if (value == null)
                {
                    hoverRow = null;
                }

                // If its the same, then nothing to do
                if (dragDropTargetRow == value)
                {
                    return;
                }

                // If one is highlighted now, reset it
                if (dragDropTargetRow != null)
                {
                    dragDropState = DropTargetState.None;
                    dragDropTargetRow.UpdateDragDropDisplay();
                }

                dragDropTargetRow = value;
            }
        }

        /// <summary>
        /// The current drop target state
        /// </summary>
        [Browsable(false)]
        public DropTargetState DragDropState
        {
            get
            {
                if (dragDropTargetRow == null)
                {
                    return DropTargetState.None;
                }

                return dragDropState;
            }
            private set
            {
                if (value != dragDropState)
                {
                    dragDropState = value;

                    if (dragDropTargetRow != null)
                    {
                        dragDropTargetRow.UpdateDragDropDisplay();
                    }
                }
            }
        }

        #endregion

        #region Hot Track

        /// <summary>
        /// Indicates if the row under the mouse is always drawn hot.  It also changes how selection works, in that clicking the mouse and
        /// dragging it does not raise the selction change event until the mouse is released.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool HotTracking
        {
            get
            {
                return hotTracking;
            }
            set
            {
                hotTracking = value;
            }
        }

        /// <summary>
        /// Gets or sets the node that is currently being displayed as hot-tracked
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SandGridDragDropRow HotTrackRow
        {
            get
            {
                if (!hotTracking)
                {
                    return null;
                }

                return hotTrackRow;
            }
            set
            {
                if (!hotTracking)
                {
                    return;
                }

                UpdateHotTrackRow(value);
            }
        }

        /// <summary>
        /// Mouse is going down on the grid
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Don't allow mouse-down selection during hot track
            if (hotTracking)
            {
                suspendSelectionChangeEvent = true;
            }

            try
            {
                base.OnMouseDown(e);

                if (hotTracking)
                {
                    SelectedElements.Clear();
                }
            }
            finally
            {
                suspendSelectionChangeEvent = false;
            }
        }

        /// <summary>
        /// Mouse is being released
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (hotTracking && e.Button == MouseButtons.Left)
            {
                if (hotTrackRow != null)
                {
                    Point client = PointToClient(Cursor.Position);
                    Point hit = PointToGrid(client);

                    SandGridTreeRow row = GetElementAt(hit) as SandGridTreeRow;

                    if (row == null || !row.IsPointInExpansionArea(hit))
                    {
                        if (hotTrackRow.IsValidHotTrackSelection)
                        {
                            hotTrackRow.Selected = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The mouse is moving over the grid
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (hotTracking)
            {
                Point client = PointToClient(Cursor.Position);
                Point hit = PointToGrid(client);

                SandGridDragDropRow row = GetElementAt(hit) as SandGridDragDropRow;
                UpdateHotTrackRow(row);
            }
        }

        /// <summary>
        /// The mouse is leaving the grid
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (hotTracking)
            {
                UpdateHotTrackRow(null);
            }

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Grid selection is changing \ has changed
        /// </summary>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (suspendSelectionChangeEvent)
            {
                return;
            }

            base.OnSelectionChanged(e);
        }

        /// <summary>
        /// Update the given row as the new hot track row
        /// </summary>
        private void UpdateHotTrackRow(SandGridDragDropRow row)
        {
            if (hotTrackRow == row)
            {
                return;
            }

            if (hotTrackRow != null)
            {
                hotTrackRow.HotTrack = false;
            }

            hotTrackRow = row;

            if (hotTrackRow != null)
            {
                hotTrackRow.HotTrack = true;
            }
        }

        #endregion

    }
}
