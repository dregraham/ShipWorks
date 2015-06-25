using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using System.Drawing;
using Divelements.SandGrid.Rendering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.UI.Controls.SandGrid;
using log4net;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Customized grid row for use in the FilterTree
    /// </summary>
    public class FilterTreeGridRow : SandGridTreeRow
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FilterTreeGridRow));

        FilterNodeEntity filterNode;
        FilterCount filterCount;

        private FilterState? previousFilterState;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterTreeGridRow(FilterNodeEntity filterNode)
            : base(filterNode.Filter.Name, FilterHelper.GetFilterImage(filterNode, false))
        {
            SetFilter(filterNode);
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
        /// Gets or sets whether the filter for the row has been flagged as a slow running
        /// filter.
        /// </summary>
        private bool IsFlaggedAsSlowRunning { get; set; }

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
        /// Updates the style of the row based on the state of the underlying data.
        /// </summary>
        public void UpdateStyle()
        {
            FilterState currentState = CurrentFilterState;

            // We have a previous state, so only toggle the style if the state has changed
            if (!previousFilterState.HasValue || previousFilterState != currentState)
            {
                ToggleStyle(currentState);
                previousFilterState = currentState;
            }
        }

        /// <summary>
        /// Toggles the style of the cells based on the state of the filter: uses
        /// the disabled font if the filter is disabled; otherwise the "normal" 
        /// font/color.
        /// </summary>
        /// <param name="filterState">State of the filter.</param>
        private void ToggleStyle(FilterState filterState)
        {
            bool isFilterDisabled = filterState == (byte)FilterState.Disabled;

            // We need to use a new disabled font each time we toggle since it's disposable and the
            // FilterTreeGridRow is not
            using (DisabledFilterFont disabledFont = new DisabledFilterFont(Font))
            {
                foreach (GridCell cell in Cells)
                {
                    cell.Font = isFilterDisabled ? disabledFont.Font : Font;
                    cell.ForeColor = isFilterDisabled ? disabledFont.TextColor : Grid.Columns[0].ForeColor;
                }
            }
        }

        /// <summary>
        /// Update the filter count
        /// </summary>
        public void UpdateFilterCount()
        {
            FilterNodeEntity currentFilterNode = filterNode;

            if (currentFilterNode == null)
            {
                log.Warn("Cannot update filter count, no filter node is set");
                return;
            }

            FilterEntity filter = filterNode.Filter;
            if (filter == null)
            {
                log.Warn("Cannot update filter count, no filter is set");
                return;
            }

            FilterCount count = FilterContentManager.GetCount(currentFilterNode.FilterNodeID);
            if (count != filterCount)
            {
                filterCount = count;
                RedrawNeeded();
            }

            UpdateLayoutForSpeed(filter, currentFilterNode, count);
        }

        /// <summary>
        /// Update the filter node associated with this grid row
        /// </summary>
        public void UpdateFilterNode(FilterNodeEntity filterNodeEntity)
        {
            SetFilter(filterNodeEntity);
            UpdateFilterCount();
            UpdateStyle();
        }

        /// <summary>
        /// Set the specified filter
        /// </summary>
        private void SetFilter(FilterNodeEntity filterNodeEntity)
        {
            filterNode = filterNodeEntity;
            filterCount = FilterContentManager.GetCount(filterNode.FilterNodeID);
        }

        /// <summary>
        /// Inspects the cost of the filter to determine if the row should be flagged as slow
        /// running filter by adjusting the image and filter name if it took more than 10,000
        /// milliseconds to calculate the filter; otherwise the standard filter image and filter 
        /// name is used.
        /// </summary>
        /// <remarks>This method uses arguments for the filter and filter node instead of using the properties to 
        /// ensure that the null checks done earlier still apply</remarks>
        private void UpdateLayoutForSpeed(FilterEntity filter, FilterNodeEntity currentFilterNode, FilterCount currentFilterCount)
        {
            // We want to flag the filter if it takes more than 10 seconds to complete
            const int WarningThresholdInMilliseconds = 10000;
            bool statusChangeWrittenToLog = false;

            int filterCountCost = currentFilterCount == null ? 0 : currentFilterCount.CostInMilliseconds;

            foreach (GridCell cell in Cells)
            {
                // Make a note whether the filter was already flagged as a slow running filter
                bool previousFlag = IsFlaggedAsSlowRunning;

                if (filterCountCost > WarningThresholdInMilliseconds)
                {
                    IsFlaggedAsSlowRunning = true;
                    cell.Image = Properties.Resources.funnel_warning;
                    cell.Text = filter.Name + " (slow)";

                    if (!previousFlag && !statusChangeWrittenToLog)
                    {
                        // Write an entry to the log when a filter goes from normal to slow
                        log.InfoFormat("The {0} filter took {1} ms to complete and has been flagged as a slow running filter.", filter.Name, filterCountCost);
                        statusChangeWrittenToLog = true;
                    }
                }
                else
                {
                    // The filter does not exceed the warning threshold, so set the 
                    // text and image to normal
                    IsFlaggedAsSlowRunning = false;
                    cell.Image = FilterHelper.GetFilterImage(currentFilterNode, false);
                    cell.Text = filter.Name;

                    if (previousFlag && !statusChangeWrittenToLog)
                    {
                        // Write an entry to the log when a filter goes from slow to normal
                        log.InfoFormat("The {0} filter took {1} ms to complete and the slow running filter flag has been removed.", filter.Name, filterCountCost);
                        statusChangeWrittenToLog = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets whether the filter associated with this row is disabled 
        /// </summary>
        public bool IsFilterDisabled()
        {
            return CurrentFilterState == FilterState.Disabled;
        }

        /// <summary>
        /// Gets the current state of the filter associated with this row
        /// </summary>
        /// <returns>The current state if it can be retrieved, otherwise we assume it is enabled</returns>
        private FilterState CurrentFilterState
        {
            get
            {
                FilterEntity filter = filterNode.Filter;

                return filter != null ? 
                    (FilterState)filter.State : 
                    FilterState.Enabled;
            }
        }
    }
}
