using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using System.IO;
using Interapptive.Shared.UI;
using Divelements.SandGrid;
using ShipWorks.Properties;
using System.Collections;
using ShipWorks.Data.Import.Spreadsheet;
using SpreadsheetGear;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing
{
    /// <summary>
    /// UserControl allow the user to read and load a sample Excel document for headings
    /// </summary>
    public partial class GenericExcelSourceSchemaControl : UserControl
    {
        List<GenericSpreadsheetSourceColumn> sourceColumns = new List<GenericSpreadsheetSourceColumn>();

        IWorkbook workbook;
        bool suspendLoadSample = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelSourceSchemaControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The sample file used to load the schema
        /// </summary>
        public string SampleFilename
        {
            get { return sampleFileName.Text; }
        }

        /// <summary>
        /// Gets the currently configured schema, or null if in error or not configured
        /// </summary>
        public GenericExcelSourceSchema CurrentSchema
        {
            get
            {
                if (sourceColumns.Count == 0)
                {
                    return null;
                }

                return new GenericExcelSourceSchema(
                    sourceColumns,
                    comboStartingCell.SelectedCell.Sheet,
                    comboStartingCell.SelectedCell.Address);
            }
        }

        /// <summary>
        /// Browse for the sample Excel file
        /// </summary>
        private void OnBrowse(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Excel Files (*.xl*)|*.xl*";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    try
                    {
                        workbook = SpreadsheetGear.Factory.GetWorkbook(dlg.FileName);
                    }
                    catch (IOException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                        return;
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                        return;
                    }
                    catch (InvalidDataException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                        return;
                    }

                    suspendLoadSample = true;
                    comboStartingCell.SetWorkbook(workbook);
                    suspendLoadSample = false;

                    LoadExcelColumns();

                    sampleFileName.Text = dlg.FileName;
                }
            }
        }

        /// <summary>
        /// The starting cell has changed
        /// </summary>
        private void OnChangeStartingCell(object sender, EventArgs e)
        {
            if (workbook != null)
            {
                LoadExcelColumns();
            }
        }

        /// <summary>
        /// Load the Excel sample columns
        /// </summary>
        private void LoadExcelColumns()
        {
            if (suspendLoadSample)
            {
                return;
            }

            panelOptions.Visible = true;
            sourceColumns.Clear();

            try
            {
                sourceColumns = LoadColumnData(comboStartingCell.SelectedCell, 3);
            }
            catch (GenericSpreadsheetException ex)
            {
                MessageHelper.ShowError(this, "There was an error reading the sample file:\n\n" + ex.Message);

                sourceColumns.Clear();
            }

            // Load the grid
            columnGrid.LoadColumns(sourceColumns);
        }

        /// <summary>
        /// Load the column data from teh given file and spreadsheet
        /// </summary>
        private List<GenericSpreadsheetSourceColumn> LoadColumnData(GenericExcelCell cell, int maxSamples)
        {
            IWorksheet sheet = workbook.Worksheets[cell.Sheet];
            if (sheet == null)
            {
                return new List<GenericSpreadsheetSourceColumn>();
            }

            List<GenericSpreadsheetSourceColumn> columns = new List<GenericSpreadsheetSourceColumn>();
            IRange startCell = sheet.Cells[cell.Address];

            if (startCell != null)
            {
                Dictionary<string, int> headers = GenericExcelUtility.DetermineHeaders(sheet, cell.Address);
                columns = headers.OrderBy(p => p.Value).Select(p => new GenericSpreadsheetSourceColumn(p.Key)).ToList();

                // Add in some sample data
                int counter = 0;
                int row = startCell.Row + 1;

                while (counter++ < maxSamples)
                {
                    foreach (var sourceColumn in columns)
                    {
                        string value = sheet.Cells[row, headers[sourceColumn.Name]].Text;

                        if (value != null && value.Length > 0)
                        {
                            sourceColumn.Samples.Add(value);
                        }
                    }

                    row++;
                }

            }

            return columns;
        }

    }
}
