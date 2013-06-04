using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Appearance;
using Divelements.SandRibbon.Rendering;
using ShipWorks.UI;

namespace ShipWorks.UI.Controls.SandGrid
{
    /// <summary>
    /// Custom renderer used to draw special trees, like the Template Tree and Filter Tree
    /// </summary>
    class SandGridThemedSelectionRenderer : WindowsXPRenderer
    {
        /// <summary>
        /// The color used to draw the selection background when focused
        /// </summary>
        public static Color GetSelectionColor(bool focused)
        {
            if (focused)
            {
                using (RibbonRenderer colorProvider = AppearanceHelper.CreateRibbonRenderer())
                {
                    return colorProvider.ColorTable.InformationBackground;
                }
            }
            else
            {
                return Color.FromArgb(225, 225, 225);
            }
        }

        /// <summary>
        /// Draw the background selection
        /// </summary>
        public override void DrawSelectionRectangle(Graphics graphics, Rectangle bounds, bool selected, bool focused, bool focusRectangle)
        {
            if (selected)
            {
                Color color = GetSelectionColor(focused);

                using (SolidBrush brush = new SolidBrush(color))
                {
                    graphics.FillRectangle(brush, bounds);
                }
            }
        }
    }
}
