using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Control that allows editing of name value pairs
    /// </summary>
    public class NameValueGrid : Panel
    {
        readonly DataGridView grid = new DataGridView { AllowUserToDeleteRows = true };

        /// <summary>
        /// Constructor
        /// </summary>
        public NameValueGrid()
        {
            // Set up the event handlers to let us know when data has changed
            grid.RowsRemoved += (sender, args) => OnDataChanged();
            grid.CellEndEdit += (sender, args) => OnDataChanged();
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
                    .Select(x => new KeyValuePair<string, string>(CellValue(x.Cells[0]), CellValue(x.Cells[1])))
                    .ToList();
            }
            set
            {
                grid.Rows.Clear();

                foreach (object[] rowValue in value.Select(x => new[] { x.Key, x.Value }))
                {
                    grid.Rows.Add(rowValue);
                }

                OnDataChanged();
            }
        }

        /// <summary>
        /// Fires when the data in the grid changes
        /// </summary>
        public event Action<object, EventArgs> DataChanged;

        /// <summary>
        /// Create the layout of the grid
        /// </summary>
        protected override void InitLayout()
        {
            Controls.Add(grid);

            AddGridColumn("Name", "NameColumn", x => x.FillWeight = 33);
            AddGridColumn("Value", "ValueColumn");

            grid.Dock = DockStyle.Fill;

            base.InitLayout();
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
        /// Add a column to the grid
        /// </summary>
        /// <param name="headerText">Text to display in the column header</param>
        /// <param name="name">Name of the column</param>
        /// <param name="setExtraProperties">Action that will allow the caller to set any other properties on the cell</param>
        private void AddGridColumn(string headerText, string name, Action<DataGridViewTextBoxColumn> setExtraProperties = null)
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
    }
}
