using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Control that allows editing of name value pairs
    /// </summary>
    public class NameValueGrid : Panel
    {
        readonly DataGridView grid;
        private readonly Image deleteIcon = Properties.Resources.delete16;
        private readonly Image nullImage = new Bitmap(1, 1);
        private bool mouseDown;
        private const int deleteCellIndex = 0;
        private const int nameCellIndex = 1;
        private const int valueCellIndex = 2;

        /// <summary>
        /// Constructor
        /// </summary>
        public NameValueGrid()
        {
            grid = new DataGridView
            {
                AllowUserToDeleteRows = true,
                Dock = DockStyle.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                RowHeadersWidth = 24,
                RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
            };

            // Wire up event handlers
            grid.CellContentClick += OnGridCellContentClick;
            grid.CellEnter += OnGridCellEnter;
            grid.RowPrePaint += OnGridRowPrePaint;

            // Set up the event handlers to let us know when data has changed
            grid.RowsRemoved += (sender, args) => OnDataChanged();
            grid.CellEndEdit += (sender, args) => OnDataChanged();

            // If the mouse is down, we don't want to tab to the next cell if a readonly cell is entered
            grid.MouseDown += (sender, args) => mouseDown = true;
            grid.MouseUp += (sender, args) => mouseDown = false;
            
            EnableDoubleBuffering();
        }

        /// <summary>
        /// Gets or sets the collection of key/values that is displayed in the grid
        /// </summary>
        public ICollection<KeyValuePair<string, string>> Values
        {
            get
            {
                return grid.Rows.OfType<DataGridViewRow>()
                    .Where(x => !x.IsNewRow)
                    .Select(x => new KeyValuePair<string, string>(CellValue(x.Cells[nameCellIndex]), CellValue(x.Cells[valueCellIndex])))
                    .ToList();
            }
            set
            {
                grid.Rows.Clear();

                foreach (object[] rowValue in value.Select(x => new object[] { deleteIcon, x.Key, x.Value }))
                {
                    grid.Rows.Add(rowValue);
                }

                OnDataChanged();
            }
        }

        /// <summary>
        /// Fires when the data in the grid changes
        /// </summary>
        public event EventHandler<EventArgs> DataChanged;

        /// <summary>
        /// Create the layout of the grid
        /// </summary>
        protected override void InitLayout()
        {
            Controls.Add(grid);
            AddDeleteImageColumnToGrid();
            AddTextColumnToGrid("Name", "NameColumn", x => x.FillWeight = 33);
            AddTextColumnToGrid("Value", "ValueColumn");

            base.InitLayout();
        }

        /// <summary>
        /// A grid row is about to be painted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGridRowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewCell deleteCell = grid.Rows[e.RowIndex].Cells[deleteCellIndex];

            if (grid.Rows[e.RowIndex].IsNewRow)
            {
                // Show an empty image if this is the last row
                deleteCell.Value = nullImage;
            }
            else if (deleteCell.Value == nullImage)
            {
                // Show the default image if it's not already shown
                deleteCell.Value = null;
            }
        }

        /// <summary>
        /// A cell in the grid has been entered
        /// </summary>
        void OnGridCellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Check if cell is read-only. If so then force move to next control.
            if (!mouseDown && grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            {
                SendKeys.Send("{TAB}");
            }
        }

        /// <summary>
        /// The contents of a grid cell has been clicked
        /// </summary>
        void OnGridCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Delete the current rows if the user clicked on the first column of an existing row
            if (e.ColumnIndex == 0 && !grid.Rows[e.RowIndex].IsNewRow)
            {
                grid.Rows.RemoveAt(e.RowIndex);
            }
        }

        /// <summary>
        /// Add a delete image column to the grid
        /// </summary>
        private void AddDeleteImageColumnToGrid()
        {
            var deleteIconCell = new DataGridViewImageColumn();
            deleteIconCell.Image = deleteIcon;
            deleteIconCell.DefaultCellStyle.NullValue = null;
            deleteIconCell.Width = 24;
            deleteIconCell.ImageLayout = DataGridViewImageCellLayout.Normal;
            deleteIconCell.ReadOnly = true;
            deleteIconCell.Resizable = DataGridViewTriState.False;
            grid.Columns.Add(deleteIconCell);
        }

        /// <summary>
        /// Fire the DataChanged event, if there are any subscribers
        /// </summary>
        protected virtual void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Add a text column to the grid
        /// </summary>
        /// <param name="headerText">Text to display in the column header</param>
        /// <param name="name">Name of the column</param>
        /// <param name="setExtraProperties">Action that will allow the caller to set any other properties on the cell</param>
        private void AddTextColumnToGrid(string headerText, string name, Action<DataGridViewTextBoxColumn> setExtraProperties = null)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
            {
                HeaderText = headerText,
                Name = name,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            if (setExtraProperties != null)
            {
                setExtraProperties(column);    
            }
            
            grid.Columns.Add(column);
        }

        /// <summary>
        /// Gets the value of a cell, or an empty string if null
        /// </summary>
        /// <param name="cell">Cell from which to retrieve a value</param>
        /// <returns>A text version of the value, or an empty string if null</returns>
        private static string CellValue(DataGridViewCell cell)
        {
            return cell.Value == null ? string.Empty : cell.Value.ToString();
        }

        /// <summary>
        /// Enables double-buffering to reduce flickering when a cell is clicked
        /// </summary>
        private void EnableDoubleBuffering()
        {
            Type gridType = grid.GetType();
            PropertyInfo property = gridType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            property.SetValue(grid, true, null);
        }

        /// <summary>
        /// Dispose of resources
        /// </summary>
        /// <param name="disposing">Should managed resources be disposed</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                grid.Dispose();
                deleteIcon.Dispose();
                nullImage.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
