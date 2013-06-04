using System;
using System.Drawing;
using System.Collections;
using ShipWorks.UI.Controls.Html.Core;

namespace ShipWorks.UI.Controls.Html.Tables
{
	/// <summary>
	/// Class that maintains full information about the layout of a table
	/// </summary>
	public class TableLayout
	{
        HtmlApi.IHTMLTable table;
        HtmlApi.IHTMLTableCell activeTd;

        // Keeps track of which cells are in which positions of the table.
        // A single cell can occupy more than one position if it has rowSpan or colSpan > 1.
        // Each entry in the array is another ArrayList of the TableCell objects
        ArrayList rowList;

        // The original order of the html rows when we were created
        ArrayList htmlRows;

        /// <summary>
        /// Create a table layout instance based on the given table element
        /// </summary>
        public static TableLayout Create(HtmlApi.IHTMLElement element)
        {
            HtmlApi.IHTMLTable table = null;
            HtmlApi.IHTMLTableCell activeTd = null;

            while (table == null && element != null)
            {
                string tagName = element.TagName.ToLower();

                // See if we have found the active td element
                if (tagName == "td")
                {
                    activeTd = (HtmlApi.IHTMLTableCell) element;
                }

                // See if we have found the table element
                else if (tagName == "table")
                {
                    table = (HtmlApi.IHTMLTable) element;
                    break;
                }

                // Move up to the parent
                element = element.ParentElement;
            }

            if (table == null || activeTd == null)
            {
                return null;
            }

            return new TableLayout(table, activeTd);
        }

        /// <summary>
        /// Get the table that is the direct contain of the given cell.
        /// </summary>
        public static HtmlApi.IHTMLTable GetCellTable(HtmlApi.IHTMLTableCell cell)
        {
            HtmlApi.IHTMLTable table = null;
            HtmlApi.IHTMLElement element = (HtmlApi.IHTMLElement) cell;

            while (table == null && element != null)
            {                
                // See if we have found the table element
                if (string.Compare(element.TagName, "table", true) == 0)
                {
                    table = (HtmlApi.IHTMLTable) element;
                    break;
                }

                // Move up to the parent
                element = element.ParentElement;            
            }

            return table;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private TableLayout(HtmlApi.IHTMLTable table, HtmlApi.IHTMLTableCell activeTd)
        {
            this.table = table;
            this.activeTd = activeTd;

            Initialize();
        }

        /// <summary>
        /// The table that we represent
        /// </summary>
        public HtmlApi.IHTMLTable Table
        {
            get
            {
                return table;
            }
        }

        /// <summary>
        /// Get the Td cell that is currently active
        /// </summary>
        public HtmlApi.IHTMLTableCell ActiveTd
        {
            get
            {
                return activeTd;
            }
        }

        /// <summary>
        /// Initialize the layout information
        /// </summary>
        private void Initialize()
        {
            // Create the rowList
            rowList = new ArrayList();
            htmlRows = new ArrayList();

            // Create a sub-arraylist to hold the cells of each row
            for (int rowIndex = 0; rowIndex < table.rows.Length; rowIndex++)
            {
                htmlRows.Add(table.rows.Item(rowIndex));
                rowList.Add(new ArrayList());
            }

            // Go through each row in the table
            for (int rowIndex = 0; rowIndex < table.rows.Length; rowIndex++)
            {
                HtmlApi.IHTMLTableRow row = GetHtmlRow(rowIndex);

                // Go through each cell in the row
                for (int cellIndex = 0; cellIndex < row.Cells.Length; cellIndex++)
                {
                    HtmlApi.IHTMLTableCell cell = (HtmlApi.IHTMLTableCell) row.Cells.Item(cellIndex);

                    int currentColumn = -1;

                    ArrayList columnList = (ArrayList) rowList[rowIndex];

                    do
                    {
                        // Ensure this position exists
                        EnsureIndex(rowIndex, ++currentColumn);
                    }
                    // If its not null, its already been filled in by rowSpan above us
                    while (columnList[currentColumn] != null);

                    // Go through every column in which the cell spans
                    for (int cellCol = currentColumn; cellCol <= currentColumn + (cell.colSpan - 1); cellCol++)
                    {
                        // Go through every row in which the cell spans
                        for (int cellRow = rowIndex; cellRow <= rowIndex + (cell.rowSpan - 1); cellRow++)
                        {
                            columnList = (ArrayList) rowList[cellRow];

                            EnsureIndex(cellRow, cellCol);

                            // Set the column value
                            columnList[cellCol] = cell;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Number of rows in the table
        /// </summary>
        public int RowCount
        {
            get
            {
                return rowList.Count;
            }
        }

        /// <summary>
        /// Number of columns in the table
        /// </summary>
        public int ColCount
        {
            get
            {
                if (rowList.Count == 0)
                {
                    return 0;
                }

                return ((ArrayList) rowList[0]).Count;                
            }
        }

        /// <summary>
        /// Get an ArrayList that contains a cell for each row in the table, no duplicates (rowSpan honored)
        /// </summary>
        public ArrayList GetColumnCells(int colIndex)
        {
            // Create the column list to hold the results.  We filter out duplicates for colSpans
            ArrayList columnList = new ArrayList();

            // Go through each row
            for (int i = 0; i < rowList.Count; )
            {
                ArrayList row = (ArrayList) rowList[i];

                // Get the column requested
                HtmlApi.IHTMLTableCell cell = (HtmlApi.IHTMLTableCell) row[colIndex];

                // Add to the column list
                columnList.Add(cell);

                // See how far to advance
                i += cell.rowSpan;
            }

            return columnList;
        }

        /// <summary>
        /// Get an ArrayList that contains a cell for each column in the table, no duplicates (colSpan honored)
        /// </summary>
        public ArrayList GetRowCells(int rowIndex)
        {
            // Create the column list to hold the results.  We filter out duplicates for colSpans
            ArrayList columnList = new ArrayList();

            // Get the row requested
            ArrayList row = (ArrayList) rowList[rowIndex];

            for (int i = 0; i < row.Count; )
            {
                HtmlApi.IHTMLTableCell cell = (HtmlApi.IHTMLTableCell) row[i];

                // Add to the column list
                columnList.Add(cell);

                // See how far to advance
                i += cell.colSpan;
            }

            return columnList;
        }

        /// <summary>
        /// Get the table row at the specified index
        /// </summary>
        public HtmlApi.IHTMLTableRow GetHtmlRow(int index)
        {
            return (HtmlApi.IHTMLTableRow) htmlRows[index];
        }

        /// <summary>
        /// Get the cell at the given location
        /// </summary>
        public HtmlApi.IHTMLTableCell GetCell(int rowIndex, int colIndex)
        {
            EnsureIndex(rowIndex, colIndex);

            ArrayList row = (ArrayList) rowList[rowIndex];

            if (row[colIndex] == null)
            {
                return null;
            }
            else
            {
                return (HtmlApi.IHTMLTableCell) row[colIndex];
            }
        }

        /// <summary>
        /// Get the coordinates of the given cell in the table.  Returns the left-upper most
        /// coordinates of it has a colSpan or rowSpan > 1
        /// </summary>
        public void GetCellLocation(HtmlApi.IHTMLTableCell cell, out int row, out int col)
        {
            row = 0;
            col = 0;

            for (int rowIndex = 0; rowIndex < rowList.Count; rowIndex++)
            {
                ArrayList rowArray = (ArrayList) rowList[rowIndex];

                for (int colIndex = 0; colIndex < rowArray.Count; colIndex++)
                {
                    if (rowArray[colIndex] == cell)
                    {
                        row = rowIndex;
                        col = colIndex;

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Ensure the given 2 dimensional index exists in our arrays
        /// </summary>
        private void EnsureIndex(int rowIndex, int colIndex)
        {
            ArrayList row = (ArrayList) rowList[rowIndex];

            int columnsNeeded = colIndex - (row.Count - 1);

            while (columnsNeeded > 0)
            {
                row.Add(null);

                columnsNeeded--;
            }
        }
	}
}
