using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Grid;
using ShipWorks.Filters.Grid;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data;
using ShipWorks.Data.Caching;
using System.Threading.Tasks;
using ShipWorks.Core.Common.Threading;
using System.Diagnostics;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// UserControl that holds an instance of IDockingPanelContent, for use on the MainForm and updating docking content
    /// </summary>
    [ToolboxItem(false)]
#if DEBUG
    [DebuggerDisplay("{"+nameof(DebuggerDisplay)+"} panel")]
#endif
    public partial class DockingPanelContentHolder : UserControl
    {
        IDockingPanelContent content;

        int lastSelectionVersion = -1;
        int lastUpdateVersion = -1;
        int lastDeleteVersion = -1;
        int lastInsertVersion = -1;

        List<long> lastSelectedKeys = new List<long>();

        /// <summary>
        /// Constructor
        /// </summary>
        public DockingPanelContentHolder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the holder with its content item
        /// </summary>
        public void Initialize(IDockingPanelContent content)
        {
            if (this.content != null)
            {
                throw new InvalidOperationException("The holder has already been initialized.");
            }

            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            Control control = content as Control;
            if (control == null)
            {
                throw new InvalidOperationException("The content must be of type Control.");
            }

            Controls.Add(control);

            control.Dock = DockStyle.Fill;
            control.BringToFront();

            this.content = content;
        }

#if DEBUG
        private string DebuggerDisplay => (this.content as Control)?.Name;
#endif

        /// <summary>
        /// The message to display in the panel
        /// </summary>
        public string Message
        {
            get { return labelMessage.Text; }
            set { labelMessage.Text = value; }
        }

        /// <summary>
        /// Initialize the underlying docking panel content for the currently logged on user
        /// </summary>
        public void InitializeForCurrentUser()
        {
            content.LoadState();
        }

        /// <summary>
        /// Save the state of the underlying content
        /// </summary>
        public void SaveState()
        {
            content.SaveState();
        }

        /// <summary>
        /// Update the displayed content based on the given entity keys
        /// </summary>
        public Task UpdateContent(FilterTarget activeTarget, IGridSelection selection)
        {
            string message = null;

            // If it doesn't support multi-select, pass no keys
            if (!content.SupportedTargets.Contains(activeTarget))
            {
                message = $"No {FormatTargetList(content.SupportedTargets)} are selected.";
            }
            else if (selection.Count == 0)
            {
                message = $"No {EnumHelper.GetDescription(activeTarget).ToLowerInvariant()} are selected.";
            }
            else if (selection.Count > 1 && !content.SupportsMultiSelect)
            {
                message = $"Multiple {EnumHelper.GetDescription(activeTarget).ToLowerInvariant()} are selected.";
            }
            else if (selection.Count > PagedEntityGrid.MaxMultiSelectCount)
            {
                message = $"Only {PagedEntityGrid.MaxMultiSelectCount} {EnumHelper.GetDescription(activeTarget).ToLowerInvariant()} can be displayed at a time.";
            }

            Message = message;

            // Update the visibility of the control depending on if we want the message to show through.
            ((Control) content).Visible = message == null;

            EntityTypeChangeVersion changeVersion = DataProvider.GetEntityTypeChangeVersion(content.EntityType);

            Task task;

            // If the actual keys being displayed may have changed, update the content completely
            if (HasSelectionChanged(selection))
            {
                task = content.ChangeContent(selection);
            }
            // May have new\removed rows - need a reload
            else if (changeVersion.InsertVersion != lastInsertVersion || changeVersion.DeleteVersion != lastDeleteVersion)
            {
                task = content.ReloadContent();
            }
            // Some entity data may have changed, just need an update
            else if (changeVersion.UpdateVersion != lastUpdateVersion)
            {
                task = content.UpdateContent();
            }
            else
            {
                task = TaskUtility.CompletedTask;
            }

            lastSelectionVersion = selection.Version;
            lastUpdateVersion = changeVersion.UpdateVersion;
            lastInsertVersion = changeVersion.InsertVersion;
            lastDeleteVersion = changeVersion.DeleteVersion;

            return task;
        }

        /// <summary>
        /// Determine if the selection has changed from the last time we saw it
        /// </summary>
        private bool HasSelectionChanged(IGridSelection selection)
        {
            // If nothing about the selection has changed, then we know it hasn't changed
            if (selection.Version == lastSelectionVersion)
            {
                return false;
            }

            // If the count changed, we know for sure it has changed
            if (selection.Count == lastSelectedKeys.Count && selection.OrderedKeys.SequenceEqual(lastSelectedKeys))
            {
                return false;
            }

            lastSelectedKeys = selection.OrderedKeys.ToList();
            return true;
        }

        /// <summary>
        /// Update the panel content baed on the current stores
        /// </summary>
        public void UpdateStoreDependentUI()
        {
            content.UpdateStoreDependentUI();
        }

        /// <summary>
        /// Format the list of targets into an english string
        /// </summary>
        private string FormatTargetList(FilterTarget[] targets)
        {
            if (targets.Length == 1)
            {
                return EnumHelper.GetDescription(targets[0]).ToLowerInvariant();
            }

            if (targets.Length == 2)
            {
                return string.Format("{0} or {1}", EnumHelper.GetDescription(targets[0]), EnumHelper.GetDescription(targets[1])).ToLowerInvariant();
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < targets.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append(", ");
                }

                if (i == targets.Length - 1)
                {
                    sb.Append("or ");
                }

                sb.Append(EnumHelper.GetDescription(targets[i]).ToLowerInvariant());
            }

            return sb.ToString();
        }
    }
}
