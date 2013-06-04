using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.UI.Controls;
using ShipWorks.UI;
using SpreadsheetGear.Windows.Forms;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using SpreadsheetGear;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing
{
    /// <summary>
    /// ComboBox for choosing a column from an Excel spreadsheet
    /// </summary>
    public class GenericExcelCellChooserComboBox : PopupComboBox
    {
        PopupController popupController;

        WorkbookView workbookView;

        GenericExcelCell selectedCell;

        TextFormattingInformation textFormat;

        public event EventHandler SelectionChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelCellChooserComboBox()
        {
            textFormat = new TextFormattingInformation();
            textFormat.StringFormat = new StringFormat(StringFormatFlags.NoWrap);
            textFormat.TextFormatFlags = TextFormatFlags.Top;

            // Create the template tree that will be popped up
            workbookView = new WorkbookView();
            workbookView.ContextMenuStrip = null;
            workbookView.CellBeginEdit += new CellBeginEditEventHandler(OnCellBeginEdit);

            // When it becomes visible we want to move the selected item into view
            workbookView.VisibleChanged += new EventHandler(OnWorkbookVisibleChanged);

            // Create the drop-down
            popupController = new PopupController(workbookView);
            popupController.Animate = PopupAnimation.System;

            // The filter is what we are going to be dropping down
            this.PopupController = popupController;
        }

        /// <summary>
        /// Make sure cells cant be edited
        /// </summary>
        private void OnCellBeginEdit(object sender, CellBeginEditEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// Set the workbook to use in the control
        /// </summary>
        public void SetWorkbook(IWorkbook workbook)
        {
            workbookView.ActiveWorkbook = workbook;
            workbookView.BackgroundCalculation = false;

            if (selectedCell == null || workbookView.ActiveWorkbook.Worksheets[selectedCell.Sheet] == null)
            {
                SelectedCell = new GenericExcelCell { 
                    Sheet = workbook.Worksheets[0].Name, 
                    Address = "A1" };
            }
        }

        /// <summary>
        /// The currently selected column
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GenericExcelCell SelectedCell
        {
            get
            {
                return selectedCell;
            }
            set
            {
                if (selectedCell == value)
                {
                    return;
                }

                selectedCell = value;

                Invalidate();

                if (SelectionChanged != null)
                {
                    SelectionChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Get the ideal size the dropdown area should be
        /// </summary>
        protected override Size GetIdealPopupSize()
        {
            return new Size(400, 400);
        }

        /// <summary>
        /// The visibility of the grid has changed
        /// </summary>
        void OnWorkbookVisibleChanged(object sender, EventArgs e)
        {
            if (workbookView.Visible)
            {
                IWorksheet activeSheet = null;
                if (selectedCell != null)
                {
                    activeSheet = workbookView.ActiveWorkbook.Worksheets[selectedCell.Sheet];
                    if (activeSheet != null)
                    {
                        workbookView.ActiveWorksheet = activeSheet;
                        workbookView.RangeSelection = activeSheet.Cells[selectedCell.Address];
                    }
                }

                workbookView.RangeSelectionChanged += this.OnWorkbookRangeChanged;
            }
            else
            {
                workbookView.RangeSelectionChanged -= this.OnWorkbookRangeChanged;
            }
        }

        /// <summary>
        /// The selected workbook range has changed
        /// </summary>
        private void OnWorkbookRangeChanged(object sender, RangeSelectionChangedEventArgs e)
        {
            IRange activeCell = workbookView.ActiveCell;

            if (activeCell != null)
            { 
                SelectedCell = new GenericExcelCell { 
                    Sheet = activeCell.Worksheet.Name, 
                    Address = activeCell.GetAddress(false, false, ReferenceStyle.A1, false, null)
                };
            }
            else
            {
                SelectedCell = null;
            }
        }

        /// <summary>
        /// Draw the selected filter
        /// </summary>
        protected override void OnDrawSelectedItem(Graphics g, Color foreColor, Rectangle bounds)
        {
            string text = null;

            if (selectedCell != null)
            {
                text = string.Format("{0} (on '{1}')", selectedCell.Address.Replace("$", ""), selectedCell.Sheet);
            }

            if (text != null)
            {
                bounds.Offset(1, 0);

                IndependentText.DrawText(g, text, Font, bounds, textFormat, foreColor);
            }
        }
    }
}
