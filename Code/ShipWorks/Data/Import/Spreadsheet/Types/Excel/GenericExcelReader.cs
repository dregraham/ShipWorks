using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using SpreadsheetGear;
using System.IO;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel
{
    /// <summary>
    /// Class for reading excel files with mappings
    /// </summary>
    public sealed class GenericExcelReader : GenericSpreadsheetReader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GenericExcelReader));

        IWorkbook workbook;
        IWorksheet worksheet;

        Dictionary<string, int> headers;
        int currentRow = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelReader(GenericExcelMap map, byte[] excelContents)
            : base(map)
        {
            if (excelContents == null)
            {
                throw new ArgumentNullException("excelContents");
            }

            this.workbook = OpenWorkbook(excelContents);

            // Get the worksheet
            worksheet = workbook.Worksheets[map.SourceSchema.SheetName];
            if (worksheet == null)
            {
                throw new GenericSpreadsheetException(string.Format("Sheet '{0}' does not exist in the Excel file.", map.SourceSchema.SheetName));
            }

            // Get the headers and their column indexes
            headers = GenericExcelUtility.DetermineHeaders(worksheet, map.SourceSchema.StartAddress);

            // Get the first header address.  
            IRange firstHeader = worksheet.Cells[map.SourceSchema.StartAddress];
            currentRow = firstHeader.Row;
        }

        /// <summary>
        /// Open an IWorkbook from the given reader
        /// </summary>
        private static IWorkbook OpenWorkbook(byte[] excelContents)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(excelContents))
                {
                    return SpreadsheetGear.Factory.GetWorkbookSet().Workbooks.OpenFromStream(stream);
                }
            }
            catch (InvalidDataException ex)
            {
                throw new GenericSpreadsheetException("ShipWorks could not read the input file:\n\n" + ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new GenericSpreadsheetException("ShipWorks could not read the input file:\n\n" + ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new GenericSpreadsheetException("ShipWorks could not read the input file.\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Disposable
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (workbook != null)
                {
                    workbook.Close();
                    workbook = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Advance the reader to the next record.  Returns false and does not advance if there are no more records
        /// </summary>
        public override bool NextRecord()
        {
            if (RowHasData(currentRow + 1))
            {
                currentRow++;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Indicates if the specified row has any data
        /// </summary>
        private bool RowHasData(int row)
        {
            foreach (int column in headers.Values)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, column].Text))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Rewind the reader to the previous record.  Returns false if already at the beginning.
        /// </summary>
        public override bool PreviousRecord()
        {
            if (currentRow <= 0)
            {
                return false;
            }

            currentRow--;

            return true;
        }

        /// <summary>
        /// Create the value of the current for for the given column name
        /// </summary>
        public override string ReadColumnText(string columnName)
        {
            int columnIndex;
            if (!headers.TryGetValue(columnName, out columnIndex))
            {
                throw new GenericSpreadsheetException(string.Format("The column '{0}' does not exist in the input file.", columnName));
            }

            return worksheet.Cells[currentRow, columnIndex].Text ?? string.Empty;
        }
    }
}
