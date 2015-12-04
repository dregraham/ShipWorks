using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared;
using ShipWorks.Properties;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    /// <summary>
    /// Control for displaying the list of source columns in a grid
    /// </summary>
    public partial class GenericSpreadsheetSourceColumnGrid : UserControl
    {
        /// <summary>
        /// Raised when the selected column changes
        /// </summary>
        public event EventHandler SelectedColumnChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetSourceColumnGrid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the given list of columns into the grid
        /// </summary>
        [NDependIgnoreLongMethod]
        public void LoadColumns(IEnumerable<GenericSpreadsheetSourceColumn> sourceColumns)
        {
            if (sourceColumns == null)
            {
                throw new ArgumentNullException("sourceColumns");
            }

            columnsGrid.Rows.Clear();

            Image columnImage = Resources.table_selection_column;
            Image nullImage = Resources.forbidden;

            int samples = 1;

            foreach (var column in sourceColumns)
            {
                string text = column.Name;
                Image image = columnImage;

                if (column == GenericSpreadsheetSourceColumn.Null)
                {
                    text = "(None)";
                    image = nullImage;
                }

                List<string> columnData = new List<string>();
                columnData.Add(text);

                for (int i = 0; i < column.Samples.Count; i++)
                {
                    columnData.Add(column.Samples[i]);
                }

                while (columnData.Count < 4)
                {
                    columnData.Add("");
                }

                samples = Math.Max(samples, column.Samples.Count);

                SandGridDragDropRow row = new SandGridDragDropRow(columnData.ToArray());
                row.Cells[0].Image = image;
                row.Tag = column;

                columnsGrid.Rows.Add(row);
            }

            for (int i = 1; i <= 3; i++)
            {
                columnsGrid.Columns[i].Visible = samples >= i;
            }

            columnsGrid.Columns[1].HeaderText = (samples == 1) ? "Example" : "Example 1";
        }

        /// <summary>
        /// Indicates if selection is allowed
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool AllowSelection
        {
            get
            {
                return columnsGrid.RowHighlightType != RowHighlightType.None;
            }
            set
            {
                columnsGrid.RowHighlightType = value ? RowHighlightType.Partial : RowHighlightType.None;
            }
        }

        /// <summary>
        /// Indicates if the row under the mouse is always drawn hot.  It also changes how selection works, in that clicking the mouse and
        /// dragging it does not raise the selction change event until the mouse is released.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool HotTracking
        {
            get
            {
                return columnsGrid.HotTracking;
            }
            set
            {
                columnsGrid.HotTracking = value;
            }
        }

        /// <summary>
        /// Returns the selected SourceColumn, if any
        /// </summary>
        public GenericSpreadsheetSourceColumn SelectedColumn
        {
            get
            {
                if (AllowSelection && columnsGrid.SelectedElements.Count > 0)
                {
                    return (GenericSpreadsheetSourceColumn) columnsGrid.SelectedElements[0].Tag;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    columnsGrid.SelectedElements.Clear();
                }
                else
                {
                    SandGridDragDropRow row = columnsGrid.Rows.Cast<SandGridDragDropRow>().SingleOrDefault(r => r.Tag == value);
                    if (row != null)
                    {
                        row.Selected = true;
                    }
                    else
                    {
                        columnsGrid.SelectedElements.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// The selection in the grid has changed
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllowSelection && SelectedColumnChanged != null)
            {
                SelectedColumnChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Ensure the given column is visible within the grid's scroll range
        /// </summary>
        public void EnsureVisible(GenericSpreadsheetSourceColumn sourceColumn)
        {
            SandGridDragDropRow row = columnsGrid.Rows.Cast<SandGridDragDropRow>().SingleOrDefault(r => r.Tag == sourceColumn);
            if (row != null)
            {
                row.EnsureVisible();
            }
        }

        /// <summary>
        /// Gets \ sets the column that is the current target for hottracking
        /// </summary>
        public GenericSpreadsheetSourceColumn HotTrackColumn
        {
            get
            {
                SandGridDragDropRow hotTrackRow = columnsGrid.HotTrackRow as SandGridDragDropRow;

                if (hotTrackRow != null)
                {
                    return (GenericSpreadsheetSourceColumn) hotTrackRow.Tag;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    columnsGrid.HotTrackRow = null;
                }
                else
                {
                    columnsGrid.HotTrackRow = columnsGrid.Rows.Cast<SandGridDragDropRow>().SingleOrDefault(r => r.Tag == value);
                }
            }
        }
    }
}
