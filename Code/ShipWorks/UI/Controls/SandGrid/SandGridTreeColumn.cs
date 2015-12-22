using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Interapptive.Shared;

namespace ShipWorks.UI.Controls.SandGrid
{
    /// <summary>
    /// Custom column base class to be used in conjunction with the SandGridTreeRenderer for proper drawing
    /// </summary>
    public class SandGridTreeColumn : SandGridDragDropColumn
    {
        /// <summary>
        /// Draw a single cell
        /// </summary>
        [NDependIgnoreTooManyParams]
        protected override void DrawCell(RenderingContext context, GridRow row, object value, Font cellFont, Image image, Rectangle bounds, bool selected, TextFormattingInformation textFormat, Color cellForeColor)
        {
            SandGridTreeRow treeRow = (SandGridTreeRow) row;
            SandGridTree tree = treeRow.Grid.SandGrid as SandGridTree;

            // Checking for null allows use outside of SandGridTree.  Added this to use FilterTreeGridRow outside of the FilterTree in the Quick Filter chooser
            if (tree != null && tree.SelectFoldersOnly && !treeRow.IsFolder)
            {
                cellForeColor = SystemColors.GrayText;

                if (ForeColorSource != CellForeColorSource.RowCell)
                {
                    ForeColorSource = CellForeColorSource.RowCell;
                }
            }

            base.DrawCell(context, row, value, cellFont, image, bounds, selected, textFormat, cellForeColor);
        }
    }
}
