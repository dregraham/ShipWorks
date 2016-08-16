using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Divelements.SandGrid;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using Interapptive.Shared;
using ShipWorks.UI.Controls.SandGrid;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Custom grid column for displaying the name of an EntityGridColumn.  What this does is allow for also displaying the store
    /// type the column is for next to the name.
    /// 
    /// We derive from SandGridDragDropColumn b\c the GridColumnLayoutEditor needs drag\drop.  If a grid does not support Drag\Drop, then the extra
    /// functionality is just ignored.
    /// 
    /// </summary>
    public class GridColumnDefinitionNameColumn : SandGridDragDropColumn
    {
        /// <summary>
        /// Draw the colunm cell
        /// </summary>
        [NDependIgnoreTooManyParams]
        protected override void DrawCell(RenderingContext context, GridRow row, object value, Font cellFont, Image image, Rectangle bounds, bool selected, TextFormattingInformation textFormat, Color cellForeColor)
        {
            base.DrawCell(context, row, value, cellFont, image, bounds, selected, textFormat, cellForeColor);

            GridColumnDefinitionRow definitionRow = (GridColumnDefinitionRow) row;
            GridColumnDefinition definition = definitionRow.Definition;

            if (definition.ShowStoreTypeInHeader)
            {
                // How big the text is drawn by default
                Size size = IndependentText.MeasureText(context.Graphics, value.ToString(), cellFont, textFormat);
                size.Width += 8;

                Rectangle extraBounds = bounds;
                extraBounds.X += size.Width;
                extraBounds.Width -= size.Width;

                IndependentText.DrawText(context.Graphics, string.Format("({0})", EnumHelper.GetDescription(definition.StoreTypeCode)), cellFont, extraBounds, textFormat, SystemColors.GrayText);
            }
        }
    }
}
