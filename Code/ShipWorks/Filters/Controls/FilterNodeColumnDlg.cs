using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Filters.Search;
using ShipWorks.Filters.Grid;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Window for editing all of the grid columns for any filter
    /// </summary>
    public partial class FilterNodeColumnDlg : Form
    {
        FilterNodeEntity initialNode;

        List<FilterNodeColumnSettings> editedLayouts = new List<FilterNodeColumnSettings>();

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterNodeColumnDlg(FilterNodeEntity initialNode)
        {
            InitializeComponent();

            this.initialNode = initialNode;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // If its a search node, we need to be showing search nodes
            if (initialNode != null && initialNode.Purpose == (int) FilterNodePurpose.Search)
            {
                filterComboBox.ActiveSearchNode = initialNode;
            }

            filterComboBox.LoadLayouts(FilterTarget.Orders, FilterTarget.Customers);
            filterComboBox.SelectedFilterNode = initialNode;

            if (filterComboBox.SelectedFilterNode == null)
            {
                gridColumnLayoutEditor.Enabled = false;
            }
        }

        /// <summary>
        /// Raised when the selected filter node changes
        /// </summary>
        private void OnSelectedFilterNodeChanged(object sender, EventArgs e)
        {
            FilterNodeColumnSettings gridLayout = FilterNodeColumnManager.GetUserSettings(filterComboBox.SelectedFilterNode);

            // We have to track which ones we have edited, so we know what to save or rollback
            if (!editedLayouts.Contains(gridLayout))
            {
                editedLayouts.Add(gridLayout);
            }

            // Load the layout for editing
            gridColumnLayoutEditor.LoadSettings(gridLayout);
            gridColumnLayoutEditor.Enabled = true;
        }

        /// <summary>
        /// User wants to save changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                foreach (FilterNodeColumnSettings gridLayout in editedLayouts)
                {
                    gridLayout.Save(adapter);
                }

                adapter.Commit();
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// The window has closed
        /// </summary>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                foreach (FilterNodeColumnSettings gridLayout in editedLayouts)
                {
                    gridLayout.CancelChanges();
                }
            }
        }
    }
}
