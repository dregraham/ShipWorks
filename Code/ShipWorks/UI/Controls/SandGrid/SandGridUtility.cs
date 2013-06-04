using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Divelements.SandGrid;
using System.Drawing;
using Divelements.SandGrid.Rendering;

namespace ShipWorks.UI.Controls.SandGrid
{
    public static class SandGridUtility
    {
        /// <summary>
        /// Get the colors to use for the given row
        /// </summary>
        public static void GetGridRowColors(GridRow row, RenderingContext context, out Color foreColor, out Color backColor)
        {
            foreColor = Color.Black;
            backColor = Color.White;

            if (row.Grid == null || row.Grid.SandGrid == null)
            {
                return;
            }

            Divelements.SandGrid.SandGrid grid = (Divelements.SandGrid.SandGrid) row.Grid.SandGrid;


            Color alternatingBackground;
            Color selectionFocusedBackground;
            Color selectionFocusedForeground;
            Color selectionUnfocusedBackground;
            Color selectionUnfocusedForeground;

            // We need the renderer to determine background color
            Office2007Renderer officeRenderer = context.Renderer as Office2007Renderer;
            WindowsXPRenderer xpRenderer = context.Renderer as WindowsXPRenderer;

            if (officeRenderer != null)
            {
                alternatingBackground = officeRenderer.AlternateRowBackgroundColor;

                selectionFocusedBackground = officeRenderer.SelectionFocusedBackgroundColor;
                selectionFocusedForeground = officeRenderer.SelectionFocusedForegroundColor;

                selectionUnfocusedBackground = officeRenderer.SelectionUnfocusedBackgroundColor;
                selectionUnfocusedForeground = officeRenderer.SelectionUnfocusedForegroundColor;
            }
            else if (xpRenderer != null)
            {
                alternatingBackground = Color.FromArgb(xpRenderer.AlternateRowOpacity, xpRenderer.AlternateRowBackgroundColor);

                selectionFocusedBackground = SystemColors.Highlight;
                selectionFocusedForeground = xpRenderer.GetSelectedTextColor(true);

                selectionUnfocusedBackground = SystemColors.Control;
                selectionUnfocusedForeground = xpRenderer.GetSelectedTextColor(false);
            }
            else
            {
                throw new InvalidOperationException("Unhandled grid renderer type: " + context.Renderer.GetType());
            }

            // Normally its just the grid colors
            foreColor = grid.ForeColor;
            backColor = grid.BackColor;

            // If its alternating, use the alternate color
            if (row.IndexInGrid % 2 == 1 && grid.ShadeAlternateRows)
            {
                backColor = alternatingBackground;
            }

            // See if it needs selection drawn
            if (((row.Selected || (row.Grid.SandGrid.FocusedElement == row)) && ((row.Grid.RowHighlightType == RowHighlightType.Partial) || (row.Grid.RowHighlightType == RowHighlightType.Full))) && !context.HideSelection)
            {
                if (row.Selected)
                {
                    backColor = context.ContainsFocus ? selectionFocusedBackground : selectionUnfocusedBackground;
                    foreColor = context.ContainsFocus ? selectionFocusedForeground : selectionUnfocusedForeground;
                }
            }            
        }

        /// <summary>
        /// Show a message as a nested row under the given row
        /// </summary>
        public static void ShowNestedMessage(GridRow row, string message, Color foreColor)
        {
            if (row.NestedRows.Count == 0)
            {
                NestedGridRow nestedRow = new NestedGridRow();
                nestedRow.NestedGrid.ShowColumnHeaders = false;
                nestedRow.NestedGrid.ShowRowHeaders = false;
                nestedRow.NestedGrid.RowHighlightType = RowHighlightType.None;
                nestedRow.NestedGrid.ColumnClickBehavior = ColumnClickBehavior.None;

                GridColumn messageColumn = new GridColumn("Detail");
                messageColumn.ForeColor = foreColor;
                nestedRow.NestedGrid.Columns.Add(messageColumn);
                messageColumn.AllowWrap = true;
                messageColumn.Width = 375;

                row.NestedRows.Add(nestedRow);
                row.Expanded = true;

                GridRow messageRow = new GridRow(message);
                messageRow.Height = 0;
                nestedRow.NestedGrid.Rows.Add(messageRow);
            }
            else
            {
                NestedGridRow nestedRow = (NestedGridRow) row.NestedRows[0];

                GridRow messageRow = nestedRow.NestedGrid.Rows[0];
                messageRow.Cells[0].Text = message;

                nestedRow.NestedGrid.Columns[0].ForeColor = foreColor;
            }
        }
    }
}
