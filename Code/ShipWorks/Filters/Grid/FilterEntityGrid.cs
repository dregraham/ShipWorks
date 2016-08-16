using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Grid;
using System.ComponentModel;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using System.Diagnostics;
using ShipWorks.Data;
using ShipWorks.Data.Grid.Columns;
using System.Windows.Forms;
using System.Drawing;
using Interapptive.Shared;
using Divelements.SandGrid;
using ShipWorks.Properties;
using ShipWorks.Users;
using System.Threading;
using log4net;
using ShipWorks.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Controls;
using ShipWorks.Data.Connection;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Filters.Grid;
using Interapptive.Shared.Win32;

namespace ShipWorks.Filters.Grid
{
    /// <summary>
    /// Specialized EntityGrid for showing the contents of filter nodes
    /// </summary>
    public class FilterEntityGrid : PagedEntityGrid
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(FilterEntityGrid));

        FilterTarget target;

        // The filternode currently loaded
        FilterNodeEntity activeFilterNode;

        // The last count for the active filter node as of the time we looked
        FilterCount lastKnownCount = null;

        // For counting animation
        Image countingImage = Resources.arrows_blue;
        bool isAnimating = false;

        // Allows overriding the default empty text and the font used to display it
        string overrideEmptyText = string.Empty;

        // Indicates if the first row of the grid should be automatically selected after the grid loads.  This is currently a hidden reg
        // setting intended for demoing.  Hopefully will be an official feature soon.  If and when it's a feature, it will be much more complicated than a flag.  Probably
        // a filter setting.
        bool autoSelectSingleSearchResult = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterEntityGrid(FilterTarget target)
        {
            this.target = target;

            // Don't display "Loading..." for the filter grid, this basically just applies to Search, when there are zero rows initially,
            // but could apply in other zero row situations.
            WaitingOnRowCountText = "";

            InitializeGrid();
            InitializeColumns(new FilterGridColumnStrategy());

            this.Disposed += new EventHandler(OnDisposed);
            GridCellLinkClicked += OnFilterEntityGridCellLinkClicked;

            RegistryHelper registry = new RegistryHelper(@"Software\Interapptive\ShipWorks\Options");
            autoSelectSingleSearchResult = registry.GetValue("AutoSelectSingleSearch", false);
        }

        /// <summary>
        /// The type of filter the grid is configured to display
        /// </summary>
        public FilterTarget FilterTarget
        {
            get { return target; }
        }

        /// <summary>
        /// Clear the current contents of the grid, and do not save any changed column settings.
        /// </summary>
        public void Clear()
        {
            ((FilterGridColumnStrategy) ColumnStrategy).Clear();
            ActiveFilterNode = null;
        }

        /// <summary>
        /// Sets the current filter used
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FilterNodeEntity ActiveFilterNode
        {
            get
            {
                return activeFilterNode;
            }
            set
            {
                activeFilterNode = value;

                UpdateGridRows();
            }
        }

        /// <summary>
        /// Completely reload the grid rows
        /// </summary>
        public override void ReloadGridRows()
        {
            UpdateFiltering(true);
        }

        /// <summary>
        /// Syncronize the grid to display the most up-to-date records for the current ActiveFilterNode
        /// </summary>
        public override void UpdateGridRows()
        {
            UpdateFiltering(false);
        }

        /// <summary>
        /// Update the grid to display the most up-to-date records for the specified filter node.  Returns false if there were no changes
        /// found that requiered an update.
        /// </summary>
        [NDependIgnoreLongMethod]
        private bool UpdateFiltering(bool forceReload)
        {
            Debug.Assert(!InvokeRequired);

            FilterCount currentCount = (activeFilterNode == null) ? null : FilterContentManager.GetCount(activeFilterNode.FilterNodeID);

            bool nodeChanged = false;
            bool definitionChanged = false;

            // If we are going from Ready, to counting for an Update, don't change the grid just yet
            if (currentCount != null)
            {
                // See if the FilterNode has changed since last time
                if (lastKnownCount == null || lastKnownCount.FilterNodeID != currentCount.FilterNodeID)
                {
                    nodeChanged = true;
                }

                // See if the count has changed since last time (indicating a change in either the filter node or the filter definition
                if (lastKnownCount == null || lastKnownCount.FilterNodeContentID != currentCount.FilterNodeContentID)
                {
                    definitionChanged = true;
                }
            }
            else
            {
                if (lastKnownCount != null)
                {
                    nodeChanged = true;
                    definitionChanged = true;
                }
                else
                {
                    // Going from no definition to no definition - just get out
                    return false;
                }
            }

            // If the node or definition didnt change, see if the contents of the current definition changed
            if (!definitionChanged && !forceReload)
            {
                if (lastKnownCount.CountVersion == currentCount.CountVersion)
                {
                    // The filter contents haven't changed - but the data may have for some of the rows.
                    FastResetRows();

                    log.InfoFormat("Updating grid contents - no need, count version the same. {0}", currentCount.CountVersion);
                    return false;
                }
            }

            // If not only the definition, but the entire node has changed, we have to update the columns
            if (nodeChanged)
            {
                SuspendSortProcessing = true;

                // Save the last settings.
                if (lastKnownCount != null)
                {
                    ColumnStrategy.SaveColumns(this);
                }

                // Load the current settings
                if (currentCount != null)
                {
                    ColumnStrategy.LoadColumns(this);
                    ColumnStrategy.ApplyInitialSort(this);
                }
                else
                {
                    Columns.Clear();
                }

                SuspendSortProcessing = false;
            }

            // Save the new current count
            lastKnownCount = currentCount;

            FilterEntityGateway newGateway = (currentCount != null) ?
                new FilterEntityGateway(FilterHelper.GetEntityType(target), currentCount) :
                null;

            if (definitionChanged)
            {
                // Clearing the virtual selection could put some rows in the to-be-removed list - but since we are selecting a new definition, we are starting from 
                // scratch, and those shouldnt be there
                ClearVirtualSelectionAll(false);

                // Open a new gateway with the updated count
                OpenGateway(newGateway);

                if (Rows.Count > 0)
                {
                    Rows[0].EnsureVisible();
                }
            }
            else
            {
                UpdateGateway(newGateway);
            }

            return true;
        }

        /// <summary>
        /// The loading of the gateway (filter rows) has completed
        /// </summary>
        protected override void OnGatewayLoadingComplete(int rows, bool wasCleared)
        {
            base.OnGatewayLoadingComplete(rows, wasCleared);

            if (autoSelectSingleSearchResult && rows == 1 && BuiltinFilter.IsSearchPlaceholderKey(ActiveFilterNode.FilterID))
            {
                this.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// Start or stop animating the loading for the given row
        /// </summary>
        private void Animate(bool animate)
        {
            if (animate && !isAnimating)
            {
                isAnimating = true;
                ImageAnimator.Animate(countingImage, new EventHandler(OnFrameChanged));
            }

            if (!animate && isAnimating)
            {
                isAnimating = false;
                ImageAnimator.StopAnimate(countingImage, new EventHandler(OnFrameChanged));
            }
        }

        /// <summary>
        /// The next frame is ready to be drawn
        /// </summary>
        private void OnFrameChanged(object o, EventArgs e)
        {
            if (ThreadSafeIsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(OnFrameChanged), o, e);
                return;
            }

            if (!isAnimating)
            {
                return;
            }

            ImageAnimator.UpdateFrames(countingImage);
            Invalidate();
        }

        /// <summary>
        /// Text to display when the grid is empty.  If this is set to null or empty, the grid will use default wording when its empty, based on its state.
        /// </summary>
        [DefaultValue("")]
        public string OverrideEmptyText
        {
            get
            {
                return overrideEmptyText;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                if (value != overrideEmptyText)
                {
                    overrideEmptyText = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Handle clicking on a grid cell
        /// </summary>
        private void OnFilterEntityGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            GridActionDisplayType displayType = e.Column.DisplayType as GridActionDisplayType;
            if (displayType == null)
            {
                return;
            }

            Action<object, GridHyperlinkClickEventArgs> action = displayType.ActionData as Action<object, GridHyperlinkClickEventArgs>;
            if (action != null)
            {
                action(sender, e);
            }
        }

        /// <summary>
        /// Specialized drawing
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle clientRectangle = base.ClientRectangle;
            clientRectangle.Inflate(-5, -5);
            clientRectangle.Offset(0, 30);

            bool animate = false;

            if ((clientRectangle.Width > 0) && (clientRectangle.Height > 0))
            {
                if (ActiveFilterNode == null)
                {
                    string message = string.Format("The filter that was selected has been deleted or moved.");
                    TextRenderer.DrawText(e.Graphics, message, this.Font, clientRectangle, SystemColors.GrayText, TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter);
                }

                else if (lastKnownCount != null && 
                        (lastKnownCount.Status == FilterCountStatus.RunningInitialCount || lastKnownCount.Status == FilterCountStatus.NeedsInitialCount) &&
                         ActiveFilterNode.Purpose != (int) FilterNodePurpose.Search )
                {
                    animate = true;

                    string message = string.Format("ShipWorks is determining the contents of the {0}...", ActiveFilterNode.Filter.IsFolder ? "folder" : "filter");

                    e.Graphics.DrawImage(countingImage, clientRectangle.X, clientRectangle.Top, 16, 16);
                    clientRectangle.Offset(18, 1);

                    TextRenderer.DrawText(e.Graphics, message, this.Font, clientRectangle, this.ForeColor, TextFormatFlags.WordBreak);
                }

                else if (Rows.Count == 0)
                {
                    string message = string.Format("There are no {0} to display.", EnumHelper.GetDescription((FilterTarget) ActiveFilterNode.Filter.FilterTarget).ToLowerInvariant());

                    // See if there is an override of the empty text display
                    if (OverrideEmptyText.Length > 0)
                    {
                        message = overrideEmptyText;
                    }

                    TextRenderer.DrawText(e.Graphics, message, this.Font, clientRectangle, SystemColors.GrayText, TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter);
                }
            }

            Animate(animate);
        }

        /// <summary>
        /// Make sure we cancel the animation loop on disposal
        /// </summary>
        void OnDisposed(object sender, EventArgs e)
        {
            Animate(false);
        }    
    }
}
