using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Divelements.SandGrid;
using System.Collections;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.Filters;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Control for editing grid columns
    /// </summary>
    public partial class GridColumnLayoutEditor : UserControl
    {
        GridColumnLayout gridLayout;

        // Raised when any aspect of the layout changes
        public event EventHandler GridColumnLayoutChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnLayoutEditor()
        {
            InitializeComponent();

            if (sandGrid.Rows.Count > 0)
            {
                throw new InvalidOperationException("The designer added some rows - this can cause crashes in some situations due to not being the correct row data type.");
            }
        }

        /// <summary>
        /// The loaded grid layout
        /// </summary>
        [Browsable(false)]
        public GridColumnLayout GridColumnLayout
        {
            get { return gridLayout; }
        }

        /// <summary>
        /// Load the editor for the given layout
        /// </summary>
        public void LoadGridColumnLayout(GridColumnLayout gridLayout)
        {
            if (gridLayout == null)
            {
                throw new ArgumentNullException("gridLayout");
            }

            this.gridLayout = gridLayout;
            this.editColumnFormat.Enabled = true;

            LoadLayoutDetails();
        }

        /// <summary>
        /// Clear any selected columns from the grid
        /// </summary>
        public void ClearSelection()
        {
            sandGrid.SelectedElements.Clear();
        }

        /// <summary>
        /// Load the layout details into the GUI
        /// </summary>
        private void LoadLayoutDetails()
        {
            sandGrid.SelectionChanged -= new SelectionChangedEventHandler(OnGridSelectionChanged);

            sandGrid.Rows.Clear();

            List<DictionaryEntry> sortColumnList = new List<DictionaryEntry>();

            // Go through every column
            foreach (GridColumnPosition layoutColumn in gridLayout.ApplicableColumns)
            {
                GridColumnDefinitionRow row = new GridColumnDefinitionRow(layoutColumn.Definition);
                row.Tag = layoutColumn;

                row.Checked = layoutColumn.Visible;

                sandGrid.Rows.Add(row);
                sortColumnList.Add(new DictionaryEntry(layoutColumn.Definition.HeaderText, layoutColumn.Definition.ColumnGuid));
            }

            // Don't listen to sort changee while we reload
            comboSortColumn.SelectedIndexChanged -= new EventHandler(OnSortChanged);
            comboSortDirection.SelectedIndexChanged -= new EventHandler(OnSortChanged);

            // Setup the column combo
            comboSortColumn.DisplayMember = "Key";
            comboSortColumn.ValueMember = "Value";
            comboSortColumn.DataSource = sortColumnList;

            // Make sure of a selection
            comboSortColumn.SelectedValue = gridLayout.DefaultSortColumnGuid;
            if (comboSortColumn.SelectedValue == null)
            {
                comboSortColumn.SelectedIndex = 0;
            }

            // Select the sort order
            comboSortDirection.SelectedIndex = (gridLayout.DefaultSortOrder == ListSortDirection.Ascending) ? 0 : 1;

            // Start listening for changes
            comboSortColumn.SelectedIndexChanged += new EventHandler(OnSortChanged);
            comboSortDirection.SelectedIndexChanged += new EventHandler(OnSortChanged);

            // Select first row
            if (sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows[0].Selected = true;
            }

            sandGrid.SelectionChanged += new SelectionChangedEventHandler(OnGridSelectionChanged);
            OnGridSelectionChanged(null, null);
        }

        /// <summary>
        /// Update the state of the move buttons
        /// </summary>
        private void UpdateMoveButtons()
        {
            moveUp.Enabled = sandGrid.SelectedElements.Count > 0 && sandGrid.SelectedElements[0].Index > 0;
            moveDown.Enabled = sandGrid.SelectedElements.Count > 0 && sandGrid.SelectedElements[0].Index < sandGrid.Rows.Count - 1;
        }

        /// <summary>
        /// User selected a new row in the grid
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateWidthUI();
            UpdateMoveButtons();
        }

        /// <summary>
        /// Update the width UI to reflect the current selection.
        /// </summary>
        private void UpdateWidthUI()
        {
            // Stop listening for changes
            trackBarWidth.ValueChanged -= new EventHandler(OnWidthTrackChanged);
            width.TextChanged -= new EventHandler(OnWidthTextChanged);

            GridColumnPosition layoutColumn = null;
            if (sandGrid.SelectedElements.Count > 0)
            {
                layoutColumn = (GridColumnPosition) sandGrid.SelectedElements[0].Tag;
            }

            trackBarWidth.Enabled = layoutColumn != null;
            width.Enabled = layoutColumn != null;

            // Clear the values if no column
            if (layoutColumn == null)
            {
                trackBarWidth.Value = 0;
                width.Text = "";
            }
            else
            {
                trackBarWidth.Maximum = Math.Max(250, layoutColumn.Width);
                trackBarWidth.Value = layoutColumn.Width;

                width.Text = layoutColumn.Width.ToString();
            }

            // Start listening for changes
            trackBarWidth.ValueChanged += new EventHandler(OnWidthTrackChanged);
            width.TextChanged += new EventHandler(OnWidthTextChanged);
        }

        /// <summary>
        /// Dragging the width slider
        /// </summary>
        void OnWidthTrackChanged(object sender, EventArgs e)
        {
            GridRow row = sandGrid.SelectedElements[0] as GridRow;
            GridColumnPosition layoutColumn = (GridColumnPosition) row.Tag;

            layoutColumn.Width = trackBarWidth.Value;

            UpdateWidthUI();
            OnGridLayoutChanged();
        }

        /// <summary>
        /// Typing stuff into the width control
        /// </summary>
        void OnWidthTextChanged(object sender, EventArgs e)
        {
            GridRow row = sandGrid.SelectedElements[0] as GridRow;
            GridColumnPosition layoutColumn = (GridColumnPosition) row.Tag;

            int value;
            if (int.TryParse(width.Text, out value))
            {
                if (value >= 0)
                {
                    layoutColumn.Width = value;

                    UpdateWidthUI();
                    OnGridLayoutChanged();
                }
            }
        }

        /// <summary>
        /// Move the selected row up
        /// </summary>
        private void OnMoveUp(object sender, EventArgs e)
        {
            MoveSelectedRow(-1);
            ActiveControl = moveUp;
        }

        /// <summary>
        /// Move the selected row down
        /// </summary>
        private void OnMoveDown(object sender, EventArgs e)
        {
            MoveSelectedRow(1);
            ActiveControl = moveDown;
        }

        /// <summary>
        /// A row is being dragged & dropped somewhere
        /// </summary>
        private void OnGridRowDropped(object sender, GridRowDroppedEventArgs e)
        {
            sandGrid.Rows.Remove(e.SourceRow);

            int index = e.TargetRow.Index;

            if (e.RelativeLocation == DropTargetState.DropBelow)
            {
                index++;
            }

            sandGrid.Rows.Insert(index, e.SourceRow);

            e.SourceRow.Selected = true;

            GridColumnPosition layoutColumn = (GridColumnPosition) e.SourceRow.Tag;
            SetPositionFromIndex(layoutColumn, index);

            OnGridLayoutChanged();
        }

        /// <summary>
        /// Move the selected row in the given direction
        /// </summary>
        private void MoveSelectedRow(int direction)
        {
            GridRow row = sandGrid.SelectedElements[0] as GridRow;
            int newIndex = row.Index + direction;

            row.Remove();
            sandGrid.Rows.Insert(newIndex, row);
            row.Selected = true;
            sandGrid.FocusedElement = row;

            GridColumnPosition layoutColumn = (GridColumnPosition) row.Tag;
            SetPositionFromIndex(layoutColumn, newIndex);

            row.EnsureVisible();

            OnGridLayoutChanged();
        }

        /// <summary>
        /// Get the actual position that should be set on the column to be displayed at the given index.  Since some columns are not applicable,
        /// and aren't in the sandgrid, the two indices don't line up.
        /// </summary>
        private void SetPositionFromIndex(GridColumnPosition moving, int index)
        {
            if (index == 0)
            {
                moving.Position = 0;
            }
            else
            {
                GridColumnPosition before = (GridColumnPosition) sandGrid.Rows[index - 1].Tag;

                if (moving.Position < before.Position)
                {
                    moving.Position = before.Position;
                }
                else
                {
                    moving.Position = before.Position + 1;
                }
            }
        }

        /// <summary>
        /// The default sort has changed
        /// </summary>
        void OnSortChanged(object sender, EventArgs e)
        {
            gridLayout.DefaultSortColumnGuid = (Guid) comboSortColumn.SelectedValue;
            gridLayout.DefaultSortOrder = (comboSortDirection.SelectedIndex == 0) ? ListSortDirection.Ascending : ListSortDirection.Descending;
        }

        /// <summary>
        /// The check state (visibility) of a column is changing
        /// </summary>
        private void OnCheckChanged(object sender, GridRowCheckEventArgs e)
        {
            GridColumnPosition column = (GridColumnPosition) e.Row.Tag;
            column.Visible = e.Row.Checked;

            OnGridLayoutChanged();
        }

        /// <summary>
        /// Raise the GridLayoutChanged event
        /// </summary>
        private void OnGridLayoutChanged()
        {
            if (GridColumnLayoutChanged != null)
            {
                GridColumnLayoutChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Edit the grid column formatting
        /// </summary>
        private void OnEditFormatting(object sender, EventArgs e)
        {
            using (GridColumnDisplaySettingsDlg dlg = new GridColumnDisplaySettingsDlg(gridLayout.ApplicableColumns.Select(p => p.Definition).ToList()))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    OnGridLayoutChanged();
                }
            }
        }

        /// <summary>
        /// Reset the grid columns to their default state
        /// </summary>
        private void OnReset(object sender, EventArgs e)
        {
            DialogResult result = MessageHelper.ShowQuestion(this,
                "Reset the columns to their default size and positions?");

            if (result == DialogResult.OK)
            {
                gridLayout.ResetToDefault();

                LoadLayoutDetails();
                OnGridLayoutChanged();
            }
        }
    }
}
