using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using ShipWorks.UI.Controls.SandGrid;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Window for editing the order and visibility of context menu items
    /// </summary>
    public partial class GridMenuEditor : Form
    {
        GridMenuLayoutProvider layoutProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridMenuEditor(GridMenuLayoutProvider layoutProvider)
        {
            InitializeComponent();

            this.layoutProvider = layoutProvider;

            LoadMenuItems(gridCustomers, layoutProvider.CustomerGridMenu);
            LoadMenuItems(gridOrders, layoutProvider.OrderGridMenu);

            UpdateMoveButtons();
        }

        /// <summary>
        /// Load the items into the grid
        /// </summary>
        private void LoadMenuItems(SandGrid sandGrid, ContextMenuStrip contextMenu)
        {
            sandGrid.Rows.Clear();

            foreach (ToolStripItem item in contextMenu.Items)
            {
                // Determine the text to show
                string text = item.Text;
                if (item is ToolStripSeparator)
                {
                    text = "-";
                }

                // Create the row
                SandGridDragDropRow gridRow = new SandGridDragDropRow(new string[] { "", text });
                gridRow.Tag = item;
                gridRow.Checked = layoutProvider.GetItemVisibility(item);
                gridRow.Cells[1].Image = item.Image;

                sandGrid.Rows.Add(gridRow);
            }
        }

        /// <summary>
        /// Returns the grid that is in the active tab
        /// </summary>
        public SandGrid ActiveGrid
        {
            get
            {
                return (SandGrid) tabControl.SelectedTab.Controls[0];
            }
        }

        /// <summary>
        /// Update the move buttons state
        /// </summary>
        private void UpdateMoveButtons()
        {
            if (ActiveGrid.SelectedElements.Count == 0)
            {
                moveUp.Enabled = false;
                moveDown.Enabled = false;
            }
            else
            {
                int index = ActiveGrid.SelectedElements[0].Index;

                moveUp.Enabled = index > 0;
                moveDown.Enabled = index < ActiveGrid.Rows.Count - 1;
            }
        }

        /// <summary>
        /// Selected tab has changed
        /// </summary>
        private void OnSelectedTabChanged(object sender, EventArgs e)
        {
            UpdateMoveButtons();
        }

        /// <summary>
        /// Selected grid row has changed
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMoveButtons();
        }

        /// <summary>
        /// Move the selected item up
        /// </summary>
        private void OnMoveUp(object sender, EventArgs e)
        {
            MoveSelectedRow(-1);
        }

        /// <summary>
        /// Move the selected item down
        /// </summary>
        private void OnMoveDown(object sender, EventArgs e)
        {
            MoveSelectedRow(+1);
        }

        /// <summary>
        /// Move the selected row
        /// </summary>
        private void MoveSelectedRow(int direction)
        {
            GridRow row = (GridRow) ActiveGrid.SelectedElements[0];
            int index = row.Index;

            ActiveGrid.Rows.RemoveAt(index);
            ActiveGrid.Rows.Insert(index + direction, row);

            row.Selected = true;
        }

        /// <summary>
        /// User has dropped a row to a new location
        /// </summary>
        private void OnGridRowDropped(object sender, GridRowDroppedEventArgs e)
        {
            ActiveGrid.Rows.Remove(e.SourceRow);

            int index = e.TargetRow.Index;

            if (e.RelativeLocation == DropTargetState.DropBelow)
            {
                index++;
            }

            ActiveGrid.Rows.Insert(index, e.SourceRow);

            e.SourceRow.Selected = true;
        }

        /// <summary>
        /// Save the settings of the specified grid to the given context menu
        /// </summary>
        private void SaveMenuSettings(SandGridDragDrop sandGrid, ContextMenuStrip contextMenu)
        {
            contextMenu.Items.Clear();

            foreach (GridRow row in sandGrid.Rows)
            {
                ToolStripItem item = (ToolStripItem) row.Tag;
                layoutProvider.SetItemVisibility(item, row.Checked);

                contextMenu.Items.Add(item);
            }
        }

        /// <summary>
        /// User is saving the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            SaveMenuSettings(gridCustomers, layoutProvider.CustomerGridMenu);
            SaveMenuSettings(gridOrders, layoutProvider.OrderGridMenu);

            DialogResult = DialogResult.OK;
        }
    }
}
