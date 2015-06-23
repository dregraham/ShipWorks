using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters;
using ShipWorks.UI;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Users;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.UI;
using ShipWorks.Data.Grid;

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

            WindowStateSaver windowSaver = new WindowStateSaver(this);
            windowSaver.ManageSplitter(splitContainer);

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
