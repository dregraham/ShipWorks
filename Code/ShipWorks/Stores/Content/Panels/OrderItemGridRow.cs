using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Paging;
using System.Drawing;
using Divelements.SandGrid;
using Divelements.SandGrid.Rendering;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using System.Diagnostics;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Specialized row for displaying order items
    /// </summary>
    public class OrderItemGridRow : PanelEntityGridRow
    {
        /// <summary>
        /// Draw the foreground of the row
        /// </summary>
        protected override int DrawRowExtraFooterContent(RenderingContext context, Rectangle bounds, GridColumn[] columns, TextFormattingInformation[] textFormats)
        {
            int totalHeight = 0;

            OrderItemEntity orderItem = Entity as OrderItemEntity;
            if (orderItem != null)
            {
                List<EntityBase2> attributes = DataProvider.GetRelatedEntities(orderItem.OrderItemID, EntityType.OrderItemAttributeEntity);

                // Add in enough room for a row for each attribute
                totalHeight += (attributes.Count * DetailViewSettings.SingleRowHeight);

                // Determine what the background color should be
                Color foreColor;
                Color backColor;
                SandGridUtility.GetGridRowColors(this, context, out foreColor, out backColor);

                int index = 0;
                foreach (OrderItemAttributeEntity attribute in attributes)
                {
                    Rectangle attBounds = new Rectangle(
                        bounds.Left,
                        bounds.Top + (DetailViewSettings.SingleRowHeight * index), 
                        bounds.Width, 
                        DetailViewSettings.SingleRowHeight);

                    // Only draw if this is within our allowed row bounds
                    if (attBounds.IntersectsWith(bounds))
                    {
                        // We need to draw the row separator line
                        context.Graphics.DrawLine(context.GridLinePen, attBounds.Left, attBounds.Top, attBounds.Right, attBounds.Top);

                        DrawAttributeContent(attribute, context, attBounds, columns, textFormats);
                    }

                    index++;
                }
            }

            return totalHeight;
        }

        /// <summary>
        /// Draw the attribute within the given bounds
        /// </summary>
        private void DrawAttributeContent(OrderItemAttributeEntity attribute, RenderingContext context, Rectangle attBounds, GridColumn[] columns, TextFormattingInformation[] textFormats)
        {
            GridColumn itemColumn = GetColumn(OrderItemFields.Name, columns);
            if (itemColumn != null)
            {
                string text = attribute.Name;
                if (text.Length > 0 && attribute.Description.Length > 0)
                {
                    text += ": ";
                }

                text += attribute.Description;

                Rectangle nameBounds = new Rectangle(itemColumn.Bounds.Left + 15, attBounds.Top, 500, attBounds.Height);
                IndependentText.DrawText(context.Graphics, text, Font, nameBounds, textFormats[Array.IndexOf(columns, itemColumn)], Color.DimGray);
            }

            GridColumn priceColumn = GetColumn(OrderItemFields.UnitPrice, columns);
            if (priceColumn != null && attribute.UnitPrice != 0)
            {
                Rectangle priceBounds = new Rectangle(priceColumn.Bounds.Left, attBounds.Top, priceColumn.Bounds.Width - 4, attBounds.Height);
                IndependentText.DrawText(context.Graphics, attribute.UnitPrice.ToString("c"), Font, priceBounds, textFormats[Array.IndexOf(columns, priceColumn)], Color.DimGray);
            }
        }

        /// <summary>
        /// Get the visible column that represents the given field
        /// </summary>
        private GridColumn GetColumn(EntityField2 field, GridColumn[] columns)
        {
            foreach (EntityGridColumn column in columns)
            {
                if (EntityUtility.IsSameField(column.Definition.DisplayValueProvider.PrimaryField, field))
                {
                    return column;
                }
            }

            return null;
        }
    }
}
