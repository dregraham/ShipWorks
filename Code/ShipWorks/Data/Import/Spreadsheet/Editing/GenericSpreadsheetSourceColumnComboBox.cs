using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.UI.Controls;
using System.Windows.Forms;
using ShipWorks.UI;
using ShipWorks.Templates.Controls;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using System.ComponentModel;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Templates;
using ShipWorks.Users;
using ShipWorks.Properties;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    /// <summary>
    /// ComboBox for selecting the source field for a mapping
    /// </summary>
    public class GenericSpreadsheetSourceColumnComboBox : PopupComboBox
    {
        PopupController popupController;

        GenericSpreadsheetSourceColumnGrid columnGrid;
        List<GenericSpreadsheetSourceColumn> loadedColumns = new List<GenericSpreadsheetSourceColumn>();

        string selectedColumnName;

        TextFormattingInformation textFormat;

        public event EventHandler SelectedColumnChanged;

        static string lastUserChosenColumn = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetSourceColumnComboBox()
        {
            textFormat = new TextFormattingInformation();
            textFormat.StringFormat = new StringFormat(StringFormatFlags.NoWrap);
            textFormat.TextFormatFlags = TextFormatFlags.Top;

            // Create the template tree that will be popped up
            columnGrid = new GenericSpreadsheetSourceColumnGrid();
            columnGrid.BorderStyle = BorderStyle.None;
            columnGrid.HotTracking = true;
            columnGrid.AllowSelection = true;
            
            // When it becomes visible we want to move the selected item into view
            columnGrid.VisibleChanged += new EventHandler(OnGridVisibleChanged);

            // Create the drop-down
            popupController = new PopupController(columnGrid);
            popupController.Animate = PopupAnimation.System;

            // The filter is what we are going to be dropping down
            this.PopupController = popupController;
        }

        /// <summary>
        /// Load the grid that will display the source columns
        /// </summary>
        public void LoadSourceColumns(IEnumerable<GenericSpreadsheetSourceColumn> columns)
        {
            loadedColumns = columns.ToList();

            columnGrid.SelectedColumnChanged -= OnSelectedColumnChanged;

            columnGrid.LoadColumns(
                new GenericSpreadsheetSourceColumn[] { GenericSpreadsheetSourceColumn.Null }
                .Concat(columns));

            columnGrid.SelectedColumnChanged += OnSelectedColumnChanged;
        }

        /// <summary>
        /// The currently selected column
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedColumnName
        {
            get
            {
                return selectedColumnName;
            }
            set
            {
                if (selectedColumnName == value)
                {
                    return;
                }

                selectedColumnName = value;

                Invalidate();

                if (SelectedColumnChanged != null)
                {
                    SelectedColumnChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Get the ideal size the dropdown area should be
        /// </summary>
        protected override Size GetIdealPopupSize()
        {
            return new Size(340, 340);
        }

        /// <summary>
        /// The selected column changed
        /// </summary>
        void OnSelectedColumnChanged(object sender, EventArgs e)
        {
            SelectedColumnName = (columnGrid.SelectedColumn == null || columnGrid.SelectedColumn == GenericSpreadsheetSourceColumn.Null) ? null : columnGrid.SelectedColumn.Name;

            // Save what the last selection was
            lastUserChosenColumn = SelectedColumnName;

            // And we can close the drop down
            popupController.Close(DialogResult.OK);
        }

        /// <summary>
        /// The visibility of the grid has changed
        /// </summary>
        void OnGridVisibleChanged(object sender, EventArgs e)
        {
            if (columnGrid.Visible)
            {
                columnGrid.SelectedColumnChanged -= OnSelectedColumnChanged;
                columnGrid.SelectedColumn = null;
                columnGrid.SelectedColumnChanged += OnSelectedColumnChanged;

                GenericSpreadsheetSourceColumn selectedColumn = null;
                GenericSpreadsheetSourceColumn visibleColumn = null;

                // If no name is selected, its unmapped (the 'Null' column)
                if (selectedColumnName == null)
                {
                    selectedColumn = GenericSpreadsheetSourceColumn.Null;
                }
                else
                {
                    selectedColumn = loadedColumns.FirstOrDefault(c => c.Name == selectedColumnName);
                    visibleColumn = selectedColumn;
                }

                if (visibleColumn == null && lastUserChosenColumn != null)
                {
                    visibleColumn = loadedColumns.FirstOrDefault(c => c.Name == lastUserChosenColumn);
                }

                // Could be null if it was mapped to a source column that now doesnt exist (after changing\updating) a map
                if (selectedColumn != null)
                {
                    columnGrid.HotTrackColumn = selectedColumn;
                }

                if (visibleColumn != null)
                {
                    columnGrid.EnsureVisible(loadedColumns.Last());
                    columnGrid.EnsureVisible(visibleColumn);
                }
            }
        }

        /// <summary>
        /// Draw the selected filter
        /// </summary>
        protected override void OnDrawSelectedItem(Graphics g, Color foreColor, Rectangle bounds)
        {
            Image image = null;
            string text = null;

            if (selectedColumnName != null)
            {
                bool isValid = loadedColumns.Any(c => c.Name == selectedColumnName);

                image = isValid ? Resources.table_selection_column : Resources.warning16;
                text = selectedColumnName;

                if (!isValid)
                {
                    text += " (Removed)";
                }
            }

            if (text != null)
            {
                bounds.Offset(1, 0);

                if (Enabled)
                {
                    g.DrawImage(image, bounds.Left, bounds.Top, image.Width, image.Height);
                }
                else
                {
                    ControlPaint.DrawImageDisabled(g, image, bounds.Left, bounds.Top, BackColor);
                }

                int imageTextSeparation = 5;
                bounds.Offset(image.Width + imageTextSeparation, 1);
                bounds.Width -= (image.Width + imageTextSeparation);

                IndependentText.DrawText(g, text, Font, bounds, textFormat, foreColor);
            }
        }
    }
}
