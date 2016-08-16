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
    /// Custom column base class for rendering drag\drop state
    /// </summary>
    public class SandGridDragDropColumn : GridColumn
    {
        /// <summary>
        /// Draw a single cell
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreTooManyParams]
        protected override void DrawCell(RenderingContext context, GridRow row, object value, Font cellFont, Image image, Rectangle bounds, bool selected, TextFormattingInformation textFormat, Color cellForeColor)
        {
            SandGridDragDropRow gridRow = row as SandGridDragDropRow;
            SandGridDragDrop grid = null;

            if (gridRow != null)
            {
                grid = gridRow.Grid.SandGrid as SandGridDragDrop;
            }

            // For design-time, when the designer adds regular rows. - And to allow row\column to be used outside of supporting grid
            if (grid == null)
            {
                base.DrawCell(context, row, value, cellFont, image, bounds, selected, textFormat, cellForeColor);
                return;
            }

            int imageWidth = image != null ? image.Width : 0;

            // Have to draw it is drag highlighted
            if (gridRow.DragDropState == DropTargetState.DropInside)
            {
                Rectangle textBounds = bounds;
                textBounds.Inflate(-4, 0);
                textBounds.X += imageWidth + Grid.ImageTextSeparation;

                DrawDragHighlight(context, textBounds, value.ToString(), Font, textFormat);
            }

            // Keeps the text black
            if (grid.ThemedSelection)
            {
                selected = false;
            }

            base.DrawCell(context, row, value, cellFont, image, bounds, selected, textFormat, cellForeColor);

            // Need to draw the above\below marker
            if (gridRow.DragDropState == DropTargetState.DropAbove || gridRow.DragDropState == DropTargetState.DropBelow)
            {
                int lineCenter;

                if (gridRow.DragDropState == DropTargetState.DropAbove)
                {
                    lineCenter = bounds.Top + 2;
                }
                else
                {
                    lineCenter = bounds.Bottom - 5;
                }

                Size size = IndependentText.MeasureText(context.Graphics, value.ToString(), cellFont, textFormat);

                int imageAndTextWidth = size.Width + imageWidth + Grid.ImageTextSeparation + 3;

                int triangleTop = lineCenter - 2;
                int lineLeft = bounds.Left;
                int lineRight = bounds.Left + imageAndTextWidth;

                using (Pen lightest = new Pen(Color.FromArgb(100, 0, 50, 255)))
                using (Pen light = new Pen(Color.FromArgb(130, 0, 50, 255)))
                using (Pen dark = new Pen(Color.FromArgb(180, 0, 50, 255)))
                {
                    context.Graphics.DrawLine(lightest, lineLeft + 2, lineCenter, lineRight - 2, lineCenter);
                    context.Graphics.DrawLine(lightest, lineLeft + 2, lineCenter + 1, lineRight - 2, lineCenter + 1);
                    context.Graphics.DrawLine(lightest, lineLeft + 2, lineCenter + 2, lineRight - 2, lineCenter + 2);

                    // Left arrow
                    context.Graphics.DrawLine(dark, lineLeft, triangleTop, lineLeft, triangleTop + 6);
                    context.Graphics.DrawLine(light, lineLeft + 1, triangleTop + 1, lineLeft + 1, triangleTop + 5);

                    // Right arrow
                    context.Graphics.DrawLine(dark, lineRight, triangleTop, lineRight, triangleTop + 6);
                    context.Graphics.DrawLine(light, lineRight - 1, triangleTop + 1, lineRight - 1, triangleTop + 5);
                }
            }
        }

        /// <summary>
        /// Draw the highlight for when we are dragging into a folder
        /// </summary>
        private void DrawDragHighlight(RenderingContext context, Rectangle rectangle, string text, Font font, TextFormattingInformation textFormat)
        {
            Rectangle bounds = rectangle;

            Size size = IndependentText.MeasureText(context.Graphics, text, font, textFormat);
            bounds.Width = Math.Min(bounds.Width, size.Width);
            bounds.Inflate(3, -(bounds.Height - size.Height) / 2);
            bounds.Inflate(0, 1);

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(80, 0, 120, 255)))
            {
                context.Graphics.FillRectangle(brush, bounds);
            }
        }
    }
}
