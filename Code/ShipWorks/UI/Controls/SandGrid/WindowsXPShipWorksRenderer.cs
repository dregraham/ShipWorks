using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using Divelements.SandGrid;

namespace ShipWorks.UI.Controls.SandGrid
{
    /// <summary>
    /// Specialized renderer for custom drawing how we want it in ShipWorks
    /// </summary>
    class WindowsXPShipWorksRenderer : WindowsXPRenderer
    {
        /// <summary>
        /// Draw the column header
        /// </summary>
        public override void DrawColumnHeader(Graphics graphics, GridColumn column, Rectangle bounds, TextFormattingInformation textFormat, DrawItemState state)
        {
            // If its less don't show the colum text.  We do this with a huge kludge since SandGrid is a whore and doesn't 
            // let you send in blank text to draw.
            if (bounds.Width <= 28)
            {
                column = null;
            }

            base.DrawColumnHeader(graphics, column, bounds, textFormat, state);
        }

        /// <summary>
        /// Draw the background selection
        /// </summary>
        public override void DrawSelectionRectangle(Graphics graphics, Rectangle bounds, bool selected, bool focused, bool focusRectangle)
        {
            if (selected && !focused)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(220, 230, 240)))
                {
                    graphics.FillRectangle(brush, bounds);
                }
            }
            else
            {
                base.DrawSelectionRectangle(graphics, bounds, selected, focused, focusRectangle);
            }
        }
    }
}
