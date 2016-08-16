using System;
using System.Collections;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.UI.Controls.Html.Core;

namespace ShipWorks.UI.Controls.Html.Tables
{
	/// <summary>
	/// Class that provides table editing functionality
	/// </summary>
	public class TableEditor
	{
        // The owner HtmlControl
        HtmlControl htmlControl;

        // The layout info for the table the current element is in
        TableLayout layout;

        /// <summary>
        /// Constructor
        /// </summary>
		public TableEditor(HtmlControl htmlControl)
		{
            if (htmlControl == null)
            {
                throw new ArgumentNullException("htmlControl");
            }

            this.htmlControl = htmlControl;

            UpdateLayoutInfo();

            // Listen for changes to the current element
            htmlControl.CurrentElementChanged += new EventHandler(OnCurrentElementChanged);
        }

        /// <summary>
        /// The current element in the html control has changed
        /// </summary>
        private void OnCurrentElementChanged(object sender, EventArgs e)
        {
            // Capture the current table layout
            UpdateLayoutInfo();
        }

        /// <summary>
        /// Update our TableLayout to match the currently selected element
        /// </summary>
        private void UpdateLayoutInfo()
        {
            // Capture the current table layout
            layout = TableLayout.Create(htmlControl.CurrentElement);
        }

        /// <summary>
        /// Merge the selected cell with the one to its left
        /// </summary>
        public void MergeLeft()
        {
            if (!CanMergeLeft)
            {
                return;
            }

            HtmlApi.IHTMLTableCell right = layout.ActiveTd;

            int row = 0;
            int col = 0;
            layout.GetCellLocation(right, out row, out col);

            // Get the cell to the left of this one
            HtmlApi.IHTMLTableCell left = layout.GetCell(row, col - 1);

            MergeLeftRight(left, right, row);
        }

        /// <summary>
        /// Merge the selected cell with the one to its right
        /// </summary>
        public void MergeRight()
        {
            if (!CanMergeRight)
            {
                return;
            }

            HtmlApi.IHTMLTableCell left = layout.ActiveTd;

            int row = 0;
            int col = 0;
            layout.GetCellLocation(left, out row, out col);

            // Get the cell to the left of this one
            HtmlApi.IHTMLTableCell right = layout.GetCell(row, col + left.colSpan);

            MergeLeftRight(left, right, row);
        }

        /// <summary>
        /// Do a left right cell merge
        /// </summary>
        private void MergeLeftRight(HtmlApi.IHTMLTableCell left, HtmlApi.IHTMLTableCell right, int row)
        {
            // Get the elements
            HtmlApi.IHTMLElement rightElement = (HtmlApi.IHTMLElement) right;
            HtmlApi.IHTMLElement leftElement = (HtmlApi.IHTMLElement) left;

            // Copy the content
            if (IsValueSet(rightElement.InnerText))
            {
                if (IsValueSet(leftElement.InnerText))
                {
                    leftElement.InnerHTML += "<br />";
                }

                leftElement.InnerHTML += rightElement.InnerHTML;
            }

            // Increase the colspan of the left
            left.colSpan += right.colSpan;

            // Make sure the caret is positioned in the existing cell
            MoveCaretToCell(left);

            // Delete the right
            layout.GetHtmlRow(row).DeleteCell(right.cellIndex);

            // The current element will have changed
            if (!htmlControl.UpdateCurrentElement())
            {
                UpdateLayoutInfo();
            }

            // Check to see if any rows, or the whole table, is empty
            CheckForEmpties();
        }

        /// <summary>
        /// Merge the selected cell with the one above it
        /// </summary>
        public void MergeUp()
        {
            if (!CanMergeUp)
            {
                return;
            }

            HtmlApi.IHTMLTableCell bottom = layout.ActiveTd;

            int row = 0;
            int col = 0;
            layout.GetCellLocation(bottom, out row, out col);

            // Get the cell to the top of this one
            HtmlApi.IHTMLTableCell top = layout.GetCell(row - 1, col);

            MergeTopBottom(top, bottom, row);
        }

        /// <summary>
        /// Merge the selected cell with the one below it
        /// </summary>
        public void MergeDown()
        {
            if (!CanMergeDown)
            {
                return;
            }

            HtmlApi.IHTMLTableCell top = layout.ActiveTd;

            int row = 0;
            int col = 0;
            layout.GetCellLocation(top, out row, out col);

            // Get the cell to the left of this one
            HtmlApi.IHTMLTableCell bottom = layout.GetCell(row + top.rowSpan, col);

            MergeTopBottom(top, bottom, row + top.rowSpan);
        }

        /// <summary>
        /// Do a up down cell merge
        /// </summary>
        private void MergeTopBottom(HtmlApi.IHTMLTableCell top, HtmlApi.IHTMLTableCell bottom, int bottomRow)
        {
            // Get the elements
            HtmlApi.IHTMLElement topElement = (HtmlApi.IHTMLElement) top;
            HtmlApi.IHTMLElement bottomElement = (HtmlApi.IHTMLElement) bottom;

            // Copy the content
            if (IsValueSet(bottomElement.InnerText))
            {
                if (IsValueSet(topElement.InnerText))
                {
                    topElement.InnerHTML += "<br />";
                }

                topElement.InnerHTML += bottomElement.InnerHTML;
            }

            int bottomRowSpan = bottom.rowSpan;

            // Make sure the caret is positioned in the existing cell
            MoveCaretToCell(top);

            // Delete tht bottom td
            layout.GetHtmlRow(bottomRow).DeleteCell(bottom.cellIndex);

            // Increase the rowspan of the top
            top.rowSpan += bottomRowSpan;

            // The current element will have changed
            if (!htmlControl.UpdateCurrentElement())
            {
                UpdateLayoutInfo();
            }

            // Check to see if any rows, or the whole table, is empty
            CheckForEmpties();
        }

        /// <summary>
        /// Inserts a row into the active table
        /// </summary>
        [NDependIgnoreLongMethod]
        public void InsertRow(bool after)
        {
            if (!CanInsertRow)
            {
                return;
            }

            HtmlApi.IHTMLTableCell cell = layout.ActiveTd;

            int row = 0;
            int col = 0;
            layout.GetCellLocation(cell, out row, out col);

            int insertIndex = row + (after ? 1 : 0);

            // The HTML row currently selected
            HtmlApi.IHTMLTableRow selectedRow = layout.GetHtmlRow(row);

            // Get a collection of all columns in the row or that span into the row
            ArrayList columns = layout.GetRowCells(row);

            // Create the row
            HtmlApi.IHTMLTableRow newRow = (HtmlApi.IHTMLTableRow) layout.Table.InsertRow(insertIndex);

            // List if indexes of td's to delete form the selected row. Needed in rowspan cases.
            ArrayList indexesToDelete = new ArrayList();

            // Now we have to popuplate it with cells.  Use the same cell colspans as the selected row
            for (int i = 0; i < columns.Count; i++)
            {
                // Determines if we are supposed to copy the cell content of the existing cell.
                bool rowSpanMerging = false;

                HtmlApi.IHTMLTableCell existing = (HtmlApi.IHTMLTableCell) columns[i];

               // If the existing cell is apart of a rowspan
                if (existing.rowSpan > 1)
                {
                    // Get the starting row of the this rowspanned cell
                    int rowSpanStartRow;
                    int unused;
                    layout.GetCellLocation(existing, out rowSpanStartRow, out unused);

                    // If we are inserting a row under the start of the row span, we have to update
                    // the rowspan count (but not insert a td)
                    if (after || rowSpanStartRow != row)
                    {
                        existing.rowSpan += 1;

                        // Move on
                        continue;
                    }
                    // If the selected row started the rowspan, and we are supposed to insert before it, 
                    // then we have to add a td, and update its rowspan.
                    else
                    {
                        rowSpanMerging = true;
                        indexesToDelete.Add(existing.cellIndex);
                    }
                }

                // Create the new cell
                HtmlApi.IHTMLTableCell newCell = (HtmlApi.IHTMLTableCell) newRow.InsertCell(-1);

                // Get the element interface
                HtmlApi.IHTMLElement newElement = (HtmlApi.IHTMLElement) newCell;
                HtmlApi.IHTMLElement oldElement = (HtmlApi.IHTMLElement) existing;

                // If we are merging, we need to copy the content of what we are being merged from
                if (rowSpanMerging)
                {
                    newCell.rowSpan = existing.rowSpan + 1;
                    newElement.InnerHTML = oldElement.InnerHTML;
                }

                // Copy the colspan
                newCell.colSpan = existing.colSpan;

                // Copy all the properties
                CopyTdProperties(newElement, oldElement);
            }

            // Delete any td's we dont need anymore due to rowSpan merging
            for (int i = indexesToDelete.Count - 1; i >= 0; i--)
            {
                selectedRow.DeleteCell((int) indexesToDelete[i]);
            }

            // Update the table layout to reflect changes
            UpdateLayoutInfo();
        }

        /// <summary>
        /// Inserts a column into the active table
        /// </summary>
        public void InsertColumn(bool after)
        {
            if (!CanInsertColumn)
            {
                return;
            }

            HtmlApi.IHTMLTableCell cell = layout.ActiveTd;

            int row = 0;
            int col = 0;
            layout.GetCellLocation(cell, out row, out col);

            // Get a collection of all columns in the row or that span into the row
            ArrayList cells = layout.GetColumnCells(col);

            // Go through each row
            for (int i = 0; i < cells.Count; i++)
            {
                // Determines if we are supposed to copy the cell content of the existing cell.
                bool colSpanMerging = false;

                // Get the cell in this row in the same column that is selected
                HtmlApi.IHTMLTableCell existing = (HtmlApi.IHTMLTableCell) cells[i];

                // Get the starting col of the this colspanned cell
                int colSpanStartCol;
                int currentRow;
                layout.GetCellLocation(existing, out currentRow, out colSpanStartCol);

                // If the colspan is already greater than 1, just add on to it
                if (existing.colSpan > 1)
                {
                    existing.colSpan += 1;

                    // Move on
                    continue;
                }

                int insertIndex = existing.cellIndex + (after ? 1 : 0);

                // Create the new cell
                HtmlApi.IHTMLTableCell newCell = (HtmlApi.IHTMLTableCell) layout.GetHtmlRow(currentRow).InsertCell(insertIndex);

                // Get the element interface
                HtmlApi.IHTMLElement newElement = (HtmlApi.IHTMLElement) newCell;
                HtmlApi.IHTMLElement oldElement = (HtmlApi.IHTMLElement) existing;

                // If we are merging, we need to copy the content of what we are being merged from
                if (colSpanMerging)
                {
                    newCell.colSpan = existing.colSpan + 1;
                    newElement.InnerHTML = oldElement.InnerHTML;
                }

                // Copy the rowspan
                newCell.rowSpan = existing.rowSpan;

                // Copy all the properties
                CopyTdProperties(newElement, oldElement);
            }

            // Update the table layout to reflect changes
            UpdateLayoutInfo();
        }

        /// <summary>
        /// Copy the properties of the old element to the new element
        /// </summary>
        private void CopyTdProperties(HtmlApi.IHTMLElement newElement, HtmlApi.IHTMLElement oldElement)
        {
            // Get the style
            HtmlApi.IHTMLStyle newStyle = newElement.Style;
            HtmlApi.IHTMLStyle oldStyle = oldElement.Style;

            // Make sure its not empty
            //if (newElement.InnerHTML == "" || newElement.InnerHTML == null)
            //{
            //    newElement.InnerHTML = "&nbsp;";
            //}

            // Copy important properties
            if (IsValueSet(oldElement.ID)) newElement.ID = oldElement.ID;
            if (IsValueSet(oldElement.ClassName)) newElement.ClassName = oldElement.ClassName;

            // Copy any style set
            newStyle.SetCssText(oldStyle.GetCssText());

            // Copy alignment
            string align = oldElement.GetAttribute("align", 2) as string;
            if (IsValueSet(align)) newElement.SetAttribute("align", align, 0);


            // Get the row elements
            HtmlApi.IHTMLElement oldTr = (HtmlApi.IHTMLElement) oldElement.ParentElement;
            HtmlApi.IHTMLElement newTr = (HtmlApi.IHTMLElement) newElement.ParentElement;

            // Copy important properties
            if (IsValueSet(oldTr.ID)) newTr.ID = oldTr.ID;
            if (IsValueSet(oldTr.ClassName)) newTr.ClassName = oldTr.ClassName;
        }

        /// <summary>
        /// Delete the current table row
        /// </summary>
        public void DeleteRow()
        {
            if (!CanDeleteRow)
            {
                return;
            }

            ArrayList tdsToDelete = GetTopMostSelectedCells();

            ArrayList rowIndices = new ArrayList();

            // Now build a list of all rows that need delete.
            foreach (HtmlApi.IHTMLTableCell cell in tdsToDelete)
            {
                int row = 0;
                int col = 0;
                layout.GetCellLocation(cell, out row, out col);

                if (!rowIndices.Contains(row))
                {
                    rowIndices.Add(row);
                }
            }

            DeleteTableRows(layout, rowIndices);

            // The current element will have changed
            htmlControl.UpdateCurrentElement();
            UpdateLayoutInfo();

            // Check to see if any rows, or the whole table, is empty
            CheckForEmpties();
        }

        /// <summary>
        /// Get all table cells that are siblings and are topmost in the current element selection.
        /// </summary>
        [NDependIgnoreLongMethod]
        private ArrayList GetTopMostSelectedCells()
        {
            ArrayList selectedCells = new ArrayList();

            object range = htmlControl.HtmlDocument.Selection.CreateRange();
            HtmlApi.IHTMLTxtRange textRange = range as HtmlApi.IHTMLTxtRange;
               
            // First build a list of all td's that are selected
            if (textRange != null)
            {
                HtmlApi.IHTMLElementCollection elements = htmlControl.HtmlDocument.Body.All;

                for (int i = 0; i < elements.Length; i++)
                {
                    HtmlApi.IHTMLElement element = (HtmlApi.IHTMLElement) elements.Item(i);

                    // Only look at tds.
                    if (string.Compare(element.TagName, "td", true) == 0)
                    {
                        // Make a new selection that selects the td
                        HtmlApi.IHTMLTxtRange testRange = ((HtmlApi.IHTMLBodyElement) htmlControl.HtmlDocument.Body).CreateTextRange();
                        testRange.MoveToElementText(element);

                        // See if this td is selected
                        if (textRange.InRange(testRange))
                        {
                            HtmlApi.IHTMLTableCell cell = (HtmlApi.IHTMLTableCell) element;

                            // If the table of this cell is a descendant of the current target table, then we have
                            // to find a parent td that is a descendant of the current target table.
                            if (IsDescendantOf((HtmlApi.IHTMLElement) TableLayout.GetCellTable(cell), (HtmlApi.IHTMLElement) layout.Table))
                            {
                                // Find the parent cell that is the direct child of the target table
                                while (element != null)
                                {
                                    // Move up to the parent
                                    element = element.ParentElement;            
                
                                    // See if we have found the table element
                                    if (string.Compare(element.TagName, "td", true) == 0)
                                    {
                                        HtmlApi.IHTMLTableCell testCell = (HtmlApi.IHTMLTableCell) element;

                                        if (TableLayout.GetCellTable(testCell) == layout.Table)
                                        {
                                            // Now we are at the element we need to add
                                            cell = testCell;
                                            break;
                                        }
                                    }
                                }
                            }

                                // If the current target table is a descendant of this td, then we can
                                // clear evertying we got so far b\c it will all get deleted with this td.
                            else if (IsDescendantOf((HtmlApi.IHTMLElement) layout.Table, (HtmlApi.IHTMLElement) TableLayout.GetCellTable(cell)))
                            {
                                selectedCells.Clear();

                                // This wil be our new active td, and its table will be our new active table
                                layout = TableLayout.Create(element);
                            }

                            // At this point, the cell still may be from a different table
                            if (layout.Table == TableLayout.GetCellTable(cell))
                            {
                                Debug.WriteLine("Found one: " + testRange.Text);
                                selectedCells.Add(element);
                            }
                            else
                            {
                                Debug.WriteLine("Not in the target table: " + testRange.Text);
                            }
                        }
                    }
                }
            }

            // If we didnt find any full selected td's, just use the current active one
            if (selectedCells.Count == 0)
            {
                selectedCells.Add(layout.ActiveTd);
            }

            return selectedCells;
        }

        /// <summary>
        /// Delete all the rows in the given table layout with the specified row numbers
        /// </summary>
        private void DeleteTableRows(TableLayout targetLayout, ArrayList rowIndices)
        {
            // Sort the rows
            rowIndices.Sort();

            // Go through and delete each row
            for (int rowIndex = rowIndices.Count - 1; rowIndex >= 0; rowIndex--)
            {
                int row = (int) rowIndices[rowIndex];

                Debug.WriteLine("Deleting row: " + row);

                // Get a collection of all columns in the row or that span into the row
                ArrayList columns = targetLayout.GetRowCells(row);

                // We have to cleanup rowspans as we delete
                for (int i = 0; i < columns.Count; i++)
                {
                    HtmlApi.IHTMLTableCell existing = (HtmlApi.IHTMLTableCell) columns[i];

                    // If the existing cell is apart of a rowspan
                    if (existing.rowSpan > 1)
                    {
                        // Get the starting row of the this rowspanned cell
                        int rowSpanStartRow;
                        int unused;
                        targetLayout.GetCellLocation(existing, out rowSpanStartRow, out unused);

                        // If we are deletoing a row under the start of the row span, we have to update
                        // the rowspan count
                        if (rowSpanStartRow != row)
                        {
                            existing.rowSpan -= 1;

                        }
                            // If the selected row started the rowspan, we need to create a td below it now starts the rowspan
                        else
                        {
                            if (row + 1 < targetLayout.Table.rows.Length)
                            {
                                int insertIndex = 0;

                                if (i > 0)
                                {
                                    HtmlApi.IHTMLTableCell insertAfter = targetLayout.GetCell(row + 1, i - 1);
                                    insertIndex = insertAfter.cellIndex + 1;
                                }

                                HtmlApi.IHTMLTableCell newSpanStart = (HtmlApi.IHTMLTableCell) targetLayout.GetHtmlRow(row + 1).InsertCell(insertIndex);
                                newSpanStart.rowSpan = existing.rowSpan - 1;
                                newSpanStart.colSpan = existing.colSpan;

                                // Copy the content
                                string oldContent = ((HtmlApi.IHTMLElement) existing).InnerHTML;

                                if (!string.IsNullOrEmpty(oldContent))
                                {
                                    ((HtmlApi.IHTMLElement) newSpanStart).InnerHTML = oldContent;
                                }
                            }
                        }
                    }
                }

                targetLayout.Table.deleteRow(row);
            }
        }

        /// <summary>
        /// Returns true if the given element is a parent of any element in the given collection.
        /// </summary>
        private bool IsDescendantOf(HtmlApi.IHTMLElement testChild, HtmlApi.IHTMLElement testParent)
        {
            HtmlApi.IHTMLElementCollection descendants = testParent.All;

            for (int i = 0; i < descendants.Length; i++)
            {
                if ((HtmlApi.IHTMLElement) descendants.Item(i) == testChild)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Delete the current table column
        /// </summary>
        public void DeleteColumn()
        {
            if (!CanDeleteColumn)
            {
                return;
            }

            HtmlApi.IHTMLTableCell cell = layout.ActiveTd;

            int row = 0;
            int col = 0;
            layout.GetCellLocation(cell, out row, out col);

            // Get a collection of all cells in the column or that span into the column
            ArrayList cells = layout.GetColumnCells(col);

            // Go through each row
            for (int i = 0; i < cells.Count; i++)
            {
                HtmlApi.IHTMLTableCell existing = (HtmlApi.IHTMLTableCell) cells[i];

                // If the existing cell is apart of a colspan
                if (existing.colSpan > 1)
                {
                    // We just have to decrease the colspan
                    existing.colSpan -= 1;
                }
                // We have to delete the actual column
                else
                {
                    int unused;
                    int currentRow;
                    layout.GetCellLocation(existing, out currentRow, out unused);

                    layout.GetHtmlRow(currentRow).DeleteCell(existing.cellIndex);
                }
            }

            // The current element will have changed
            if (!htmlControl.UpdateCurrentElement())
            {
                UpdateLayoutInfo();
            }

            // Check to see if any rows, or the whole table, is empty
            CheckForEmpties();
        }

        /// <summary>
        /// Check to see if there are any empty rows, or if the table itself is empty, and 
        /// delete anything found.
        /// </summary>
        private void CheckForEmpties()
        {
            if (layout == null)
            {
                return;
            }

            if (layout.ColCount > 0)
            {
                // Get all the cells in the last row
                ArrayList colCells = layout.GetColumnCells(layout.ColCount - 1);

                // The smallest colSpan of all the cells in the last row
                int smallestColSpan = int.MaxValue;

                // Go through each cell and make sure its colspan is not set too high
                foreach (HtmlApi.IHTMLTableCell colCell in colCells)
                {
                    // What the colspan needs to be for it to make sense
                    smallestColSpan = Math.Min(smallestColSpan, colCell.colSpan);
                }

                // Reduce all cospans by the same amount, making the smallest one exactly 1
                int reduceBy = Math.Max(smallestColSpan - 1, 0);

                if (reduceBy > 0)
                {
                    // Go through each cell and make sure its colspan is not set too high
                    foreach (HtmlApi.IHTMLTableCell colCell in colCells)
                    {
                        colCell.colSpan -= reduceBy;
                    }
                }
            }

            // For check for any empty rows
            for (int i = layout.Table.rows.Length - 1; i >= 0; i--)
            {
                // See if there are no physical td's in this row.
                if (((HtmlApi.IHTMLTableRow) layout.Table.rows.Item(i)).Cells.Length == 0)
                {
                    // Get all the cells that span into this row
                    ArrayList rowCells = layout.GetRowCells(i);

                    // Decrease each of there span counts, as the row is being removed
                    foreach (HtmlApi.IHTMLTableCell rowCell in rowCells)
                    {
                        rowCell.rowSpan -= 1;
                    }

                    // Delete the row
                    layout.Table.deleteRow(i);
                }
            }
        }
 
        /// <summary>
        /// Determines if a table column can currently be deleted
        /// </summary>
        public bool CanDeleteColumn
        {
            get
            {
                return layout != null;
            }
        }

        /// <summary>
        /// Determines if a table row can currently be deleted
        /// </summary>
        public bool CanDeleteRow
        {
            get
            {
                return layout != null;
            }
        }

        /// <summary>
        /// Determines if a table column can be inserted
        /// </summary>
        public bool CanInsertColumn
        {
            get
            {
                return layout != null;
            }
        }

        /// <summary>
        /// Determines if a table row can be inserted
        /// </summary>
        public bool CanInsertRow
        {
            get
            {
                return layout != null;
            }
        }

        /// <summary>
        /// Determines if the current cell can be merged with the one to its left
        /// </summary>
        public bool CanMergeLeft
        {
            get
            {
                if (layout == null)
                {
                    return false;
                }

                HtmlApi.IHTMLTableCell td = layout.ActiveTd;

                int row;
                int col;
                layout.GetCellLocation(td, out row, out col);

                ArrayList rowCells = layout.GetRowCells(row);

                if (rowCells.IndexOf(td) > 0)
                {
                    HtmlApi.IHTMLTableCell target = layout.GetCell(row, col - 1);

                    int targetRow;
                    int targetCol;
                    layout.GetCellLocation(target, out targetRow, out targetCol);

                    if (targetRow == row)
                    {
                        return (target.rowSpan == td.rowSpan);
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Determines if the current cell can be merged with the one to its right
        /// </summary>
        public bool CanMergeRight
        {
            get
            {
                if (layout == null)
                {
                    return false;
                }

                HtmlApi.IHTMLTableCell td = layout.ActiveTd;

                int row;
                int col;
                layout.GetCellLocation(td, out row, out col);

                ArrayList rowCells = layout.GetRowCells(row);

                // Its supposed to be a one.  The rowCells collection eliminates duplicates based on colspan.
                if (rowCells.IndexOf(td) + 1 < rowCells.Count)
                {
                    HtmlApi.IHTMLTableCell target = layout.GetCell(row, col + td.colSpan);

                    int targetRow;
                    int targetCol;
                    layout.GetCellLocation(target, out targetRow, out targetCol);

                    if (targetRow == row)
                    {
                        return (target.rowSpan == td.rowSpan);
                    }
                }

                return false;
            }            
        }

        /// <summary>
        /// Determines if the current cell can be merged with the one above it
        /// </summary>
        public bool CanMergeUp
        {
            get
            {
                if (layout == null)
                {
                    return false;
                }

                HtmlApi.IHTMLTableCell td = layout.ActiveTd;

                int row;
                int col;
                layout.GetCellLocation(td, out row, out col);

                ArrayList colCells = layout.GetColumnCells(col);

                if (colCells.IndexOf(td) > 0)
                {
                    HtmlApi.IHTMLTableCell target = layout.GetCell(row - 1, col);

                    int targetRow;
                    int targetCol;
                    layout.GetCellLocation(target, out targetRow, out targetCol);

                    if (targetCol == col)
                    {
                        return (target.colSpan == td.colSpan);
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Determines if the current cell can be merged with the one below it
        /// </summary>
        public bool CanMergeDown
        {
            get
            {
                if (layout == null)
                {
                    return false;
                }

                HtmlApi.IHTMLTableCell td = layout.ActiveTd;

                int row;
                int col;
                layout.GetCellLocation(td, out row, out col);

                ArrayList colCells = layout.GetColumnCells(col);

                // Its supposed to be a one.  The colCells collection eliminates duplicates based on rowSpan.
                if (colCells.IndexOf(td) + 1 < colCells.Count)
                {
                    HtmlApi.IHTMLTableCell target = layout.GetCell(row + td.rowSpan, col);

                    int targetRow;
                    int targetCol;
                    layout.GetCellLocation(target, out targetRow, out targetCol);

                    if (targetCol == col)
                    {
                        return (target.colSpan == td.colSpan);
                    }
                }

                return false;
            }            
        }

        /// <summary>
        /// Determines if the given valud is set for an element
        /// </summary>
        private bool IsValueSet(string attValue)
        {
            return attValue != null && attValue.Trim().Length != 0;
        }

        /// <summary>
        /// Moves the caret to the given table cell
        /// </summary>
        private bool MoveCaretToCell(HtmlApi.IHTMLTableCell nextCell)
        {
            HtmlApi.IDisplayServices ds = (HtmlApi.IDisplayServices) htmlControl.HtmlDocument;
            HtmlApi.IMarkupServices ms = (HtmlApi.IMarkupServices) htmlControl.HtmlDocument;

            HtmlApi.IMarkupPointer pointer1;
            ms.CreateMarkupPointer(out pointer1);
            pointer1.MoveAdjacentToElement((HtmlApi.IHTMLElement) nextCell, HtmlApi.ELEMENT_ADJACENCY.ELEM_ADJ_AfterBegin);

            HtmlApi.IDisplayPointer pointer2;
            ds.CreateDisplayPointer(out pointer2);
            pointer2.MoveToMarkupPointer(pointer1, null);

            HtmlApi.IHTMLCaret caret;
            ds.GetCaret(out caret);

            caret.MoveCaretToPointer(pointer2, true, HtmlApi.CARET_DIRECTION.CARET_DIRECTION_SAME);

            return true;
        }
    }
}
