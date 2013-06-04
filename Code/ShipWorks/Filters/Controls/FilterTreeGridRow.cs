using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using System.Drawing;
using Divelements.SandGrid.Rendering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.UI.Controls.SandGrid;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Customized grid row for use in the FilterTree
    /// </summary>
    class FilterTreeGridRow : SandGridTreeRow
    {
        FilterNodeEntity filterNode;
        FilterCount filterCount;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterTreeGridRow(FilterNodeEntity filterNode)
            : base(filterNode.Filter.Name, FilterHelper.GetFilterImage(filterNode))
        {
            this.filterNode = filterNode;
            this.filterCount = FilterContentManager.GetCount(filterNode.FilterNodeID);
        }

        /// <summary>
        /// The filter node that this row represents
        /// </summary>
        public FilterNodeEntity FilterNode
        {
            get { return filterNode; }
        }

        /// <summary>
        /// The count as of its last update
        /// </summary>
        public FilterCount FilterCount
        {
            get { return filterCount; }
        }

        /// <summary>
        /// Indicates if the row represents a folder that can contain other items
        /// </summary>
        public override bool IsFolder
        {
            get { return filterNode.Filter.IsFolder; }
        }

        /// <summary>
        /// Indicates if the row can be dragged
        /// </summary>
        public override bool IsDraggable
        {
            get
            {
                return !FilterHelper.IsBuiltin(filterNode);
            }
        }

        /// <summary>
        /// Indicates if this row is a valid drop target from the sourceRow, for the given state
        /// </summary>
        public override bool IsValidDrop(SandGridDragDropRow sourceRow, DropTargetState state)
        {
            // Cant drop things above or below a builtin item
            if (FilterHelper.IsBuiltin(filterNode) && state != DropTargetState.DropInside)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update the filter count
        /// </summary>
        public void UpdateFilterCount()
        {
            FilterCount count = FilterContentManager.GetCount(filterNode.FilterNodeID);
            if (count != filterCount)
            {
                filterCount = count;
                RedrawNeeded();
            }
        }
    }
}
