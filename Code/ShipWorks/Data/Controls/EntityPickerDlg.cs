using System;
using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Users;

namespace ShipWorks.Data.Controls
{
    /// <summary>
    /// Window for choosing orders
    /// </summary>
    public partial class EntityPickerDlg : Form
    {
        FilterTarget filterTarget;

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityPickerDlg(FilterTarget filterTarget)
        {
            InitializeComponent();

            WindowStateSaver.Manage(this).ManageSplitter(splitContainer);

            this.filterTarget = filterTarget;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // Initialize the grid
            gridControl.InitializeForTarget(filterTarget, null, null);

            UserEntity user = UserSession.User;

            filterTree.LoadLayouts(filterTarget);
            filterTree.ApplyFolderState(new FolderExpansionState(user.Settings.OrderFilterExpandedFolders));

            // Select the root node
            filterTree.SelectFirstNode();
        }

        /// <summary>
        /// The current selection
        /// </summary>
        [Browsable(false)]
        public IGridSelection Selection
        {
            get { return gridControl.Selection; }
        }

        /// <summary>
        /// The selected filter node has changed
        /// </summary>
        private void OnSelectedFilterChanged(object sender, EventArgs e)
        {
            gridControl.ActiveFilterNode = filterTree.SelectedFilterNode;
            UpdateStatusBar();
        }

        /// <summary>
        /// The selection in the grid has changed
        /// </summary>
        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            UpdateStatusBar();
        }

        /// <summary>
        /// Update the status bar display
        /// </summary>
        private void UpdateStatusBar()
        {
            labelTotal.Text = string.Format("Total: {0}", gridControl.TotalCount);
            labelSelected.Text = string.Format("Selected: {0}", gridControl.Selection.Count);

            labelStatusEtch.Left = labelTotal.Right + 2;
            labelSelected.Left = labelStatusEtch.Right + 3;
        }

        /// <summary>
        /// GridRow has been double-clicked
        /// </summary>
        private void OnRowActivated(object sender, Divelements.SandGrid.GridRowEventArgs e)
        {
            OnOK(this, EventArgs.Empty);
        }

        /// <summary>
        /// User clicked OK to select orders.
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
