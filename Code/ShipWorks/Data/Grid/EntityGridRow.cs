using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.UI.Controls.SandGrid;
using Divelements.SandGrid;
using System.Drawing;
using ShipWorks.Data.Grid.Columns;
using Divelements.SandGrid.Rendering;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Windows.Forms;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Properties;

namespace ShipWorks.Data.Grid
{
    /// <summary>
    /// Base GridRow class for rows that display entity data.
    /// </summary>
    public abstract class EntityGridRow : GridRow
    {
        // Track which column is currently under the mouse
        Guid? mouseCapturedColumnGuid = null;

        /// <summary>
        /// Returns the Entity that represents the current row.
        /// </summary>
        public abstract EntityBase2 Entity { get; }

        /// <summary>
        /// Returns the key of the entity that represents the current row.  This may be available even if Entity is null, such as
        /// when its a paged grid row, where only the Header is loaded.
        /// </summary>
        public virtual long? EntityID
        {
            get
            {
                EntityBase2 entity = Entity;
                if (entity != null)
                {
                    return (long) entity.Fields.PrimaryKeyFields[0].CurrentValue;
                }

                return null;
            }
        }

        // This is a valid only while within a call to DrawRowForeground
        int cellsDesiredHeight = 0;

        /// <summary>
        /// Draw the foreground of the row.  This is where the detail view processing takes place.
        /// </summary>
        [NDependIgnoreLongMethod]
        protected override void DrawRowForeground(RenderingContext context, Rectangle bounds, GridColumn[] columns, TextFormattingInformation[] textFormats)
        {
            if (Grid == null)
            {
                return;
            }

            EntityGrid grid = Grid.SandGrid as EntityGrid;
            if (grid == null)
            {
                throw new InvalidOperationException("Cannot use EntityGridRow outside of EntityGrid.");
            }

            EntityBase2 entity = Entity;

            // Consider a deleted entity non-existant
            if (entity != null && entity.Fields.State == EntityState.Deleted)
            {
                entity = null;
            }

            // Reset the desired height of the cells before drawing
            int totalDesiredHeight = 0;

            // Draw header
            int headerHeight = DrawRowExtraHeaderContent(context, bounds, columns, textFormats);

            // Update bounds info
            totalDesiredHeight += headerHeight;
            RemoveTop(ref bounds, headerHeight);

            // No need to autosize based on the cells desired height, or to display nested errors, if we are in detail only mode.
            if (grid.DetailViewSettings == null || grid.DetailViewSettings.DetailViewMode != DetailViewMode.DetailOnly)
            {
                // Start out assuming the cells want to be a standard row height.  The may adjust themselves while drawing
                cellsDesiredHeight = DetailViewSettings.SingleRowHeight;

                // Capture the old clipping info
                Region oldClipping = null; 

                // If we drew a header, we want to clip drawing to not draw on top of it (such as column lines)
                if (headerHeight > 0)
                {
                    oldClipping = context.Graphics.Clip;
                    context.Graphics.ExcludeClip(new Rectangle(bounds.X, bounds.Y - headerHeight, bounds.Width, headerHeight));
                }

                // Draw the foreground - which will cause all the virtual cells to draw.
                base.DrawRowForeground(context, bounds, columns, textFormats);

                if (headerHeight > 0)
                {
                    context.Graphics.SetClip(oldClipping, System.Drawing.Drawing2D.CombineMode.Replace);
                }

                // Update bounds info
                totalDesiredHeight += cellsDesiredHeight;
                RemoveTop(ref bounds, cellsDesiredHeight);
            }

            // Draw footer
            int footerHeight = DrawRowExtraFooterContent(context, bounds, columns, textFormats);

            // Update bounds info
            totalDesiredHeight += footerHeight;
            RemoveTop(ref bounds, footerHeight);

            // See if detail view is on
            if (grid.DetailViewSettings != null && grid.DetailViewSettings.DetailViewMode != DetailViewMode.Normal)
            {
                // Use null for the key if there is no entity
                long? entityID = (entity == null) ? (long?) null : (long) entity.Fields.PrimaryKeyFields[0].CurrentValue;

                // Draw the details and determine what the new height should be
                int detailHeight = grid.DetailViewRenderer.Draw(entityID, grid.DetailViewSettings, this, context, bounds);

                // Update bounds info
                totalDesiredHeight += detailHeight;
                RemoveTop(ref bounds, detailHeight);
            }

            // See if we need to display nested errors
            if (entity != null && grid.ErrorProvider != null)
            {
                string error = grid.ErrorProvider.GetError(entity);

                if (!string.IsNullOrEmpty(error))
                {
                    // Draw error
                    int errorHeight = DrawNestedError(context, bounds, error);

                    // Update bounds info
                    totalDesiredHeight += errorHeight;
                    RemoveTop(ref bounds, errorHeight);
                }
            }

            // Ensure we are the correct height.  May force a redraw, which will cause us to be called again - but hopefully we'll calculate the same
            // height the next time around and not end up in an endless loop.
            if (Height != totalDesiredHeight)
            {
                Height = totalDesiredHeight;
            }
        }

        /// <summary>
        /// Draw any extra content that goes above the row, like header\group information
        /// </summary>
        protected virtual int DrawRowExtraHeaderContent(RenderingContext context, Rectangle bounds, GridColumn[] columns, TextFormattingInformation[] textFormats)
        {
            return 0;
        }

        /// <summary>
        /// Draw any extra content for the row.  Like the attributes for the Order Item grid
        /// </summary>
        protected virtual int DrawRowExtraFooterContent(RenderingContext context, Rectangle bounds, GridColumn[] columns, TextFormattingInformation[] textFormats)
        {
            return 0;
        }

        /// <summary>
        /// Draw the content of a cell for the specified column.
        /// </summary>
        [NDependIgnoreTooManyParams]
        protected override void DrawVirtualCell(RenderingContext context, GridColumn baseColumn, object value, Font font, Image image, Rectangle bounds, bool selected, TextFormattingInformation textFormat, Color foreColor)
        {
            Checked = selected;

            GridColumnFormattedValue formattedValue = GetFormattedValue(baseColumn);

            bool disposeFont = false;

            string displayText = formattedValue.Text;
            image = formattedValue.Image;

            if (formattedValue.ForeColor != null)
            {
                foreColor = formattedValue.ForeColor.Value;
            }

            if (formattedValue.FontStyle != null)
            {
                font = new Font(font, formattedValue.FontStyle.Value);
                disposeFont = true;
            }

            // Determine what size we want to be if we are autowrapping
            if (!string.IsNullOrEmpty(displayText))
            {
                if (((EntityGridColumn) baseColumn).Definition.AutoWrap)
                {
                    int height = DetailViewSettings.SingleRowHeight;

                    Size size = IndependentText.MeasureText(context.Graphics, displayText, Font, bounds.Width - 8, textFormat);
                    height = Math.Max(height, size.Height);

                    // Round up a little to account for leading
                    if (height > DetailViewSettings.SingleRowHeight)
                    {
                        height += 3;
                    }

                    // Update the desired height of cells to be at least this high
                    cellsDesiredHeight = Math.Max(cellsDesiredHeight, height);
                }
            }

            // Let the base do the drawing with the values we provide
            base.DrawVirtualCell(
                context,
                baseColumn,
                displayText, 
                font,
                image, 
                bounds, 
                selected, 
                textFormat,
                foreColor);

            // We created a new font that needs to be disposed
            if (disposeFont)
            {
                font.Dispose();
            }
        }


        /// <summary>
        /// Get the formatted value to use for display of the given column in this row.  
        /// </summary>
        public GridColumnFormattedValue GetFormattedValue(GridColumn column)
        {
            EntityBase2 entity = Entity;

            if (entity != null)
            {
                EntityGridColumn entityColumn = column as EntityGridColumn;

                if (entityColumn != null)
                {
                    return entityColumn.DisplayType.FormatValue(entity);
                }
            }

            return new GridColumnFormattedValue(null, null, null);
        }

        /// <summary>
        /// Draw's a nested error in the bounds given.  The ideal height of the error content is returned.
        /// </summary>
        private int DrawNestedError(RenderingContext context, Rectangle bounds, string text)
        {
            Graphics g = context.Graphics;
            Size textSize;

            // Use the small of the row width or actual grid width - so that if the grid is scrolled, the whole error is still displayed.
            bounds.Width = Math.Min(bounds.Width, Grid.SandGrid.Width);

            Padding margin = new Padding(25, 1, 10, 6);
            int spaceAboveText = margin.Top;

            // Determine how big of an area we need for text
            using (TextFormattingInformation tfi = TextFormattingInformation.CreateFormattingInformation(false, true, StringAlignment.Near, StringAlignment.Near, true))
            {
                textSize = IndependentText.MeasureText(g, text, context.Font, bounds.Width - (margin.Left + margin.Right), tfi);

                // See if there is any space at all in which to draw
                if (bounds.Height > 0)
                {
                    // The space we have in which to draw the text
                    Rectangle textBounds = new Rectangle(
                        bounds.Left + margin.Left,
                        bounds.Top + spaceAboveText,
                        Math.Max(0, bounds.Width - (margin.Left + margin.Right)),
                        Math.Max(0, bounds.Height - (spaceAboveText + margin.Bottom)));

                    if (textBounds.Height > 0 && textBounds.Width > 0)
                    {
                        IndependentText.DrawText(g, text, context.Font, textBounds, tfi, Color.Red);

                        // The space we have to draw the image
                        Rectangle imageBounds = new Rectangle(
                            textBounds.Left - 18,
                            bounds.Top,
                            16,
                            16);

                        g.DrawImage(Resources.error_arrow, imageBounds);
                    }
                }
            }

            return spaceAboveText + textSize.Height + margin.Bottom;
        }

        /// <summary>
        /// Remove the given amount of space from the top of the bounds
        /// </summary>
        public static void RemoveTop(ref Rectangle bounds, int amount)
        {
            amount = Math.Min(amount, bounds.Height);

            bounds.Y += amount;
            bounds.Height -= amount;
        }

        /// <summary>
        /// Get the value to use for the specified column.  This is the value SandGrid uses to sort.
        /// </summary>
        public override object GetCellValue(GridColumn baseColumn)
        {
            EntityBase2 entity = Entity;

            if (entity != null)
            {
                EntityGridColumn entityColumn = baseColumn as EntityGridColumn;

                if (entityColumn != null)
                {
                    return entityColumn.Definition.SortProvider.GetLocalSortValue(entity);
                }
            }

            return base.GetCellValue(baseColumn);
        }

        /// <summary>
        /// The column that was captured as being under the mouse when the mouse went down
        /// </summary>
        private EntityGridColumn MouseCapturedColumn
        {
            get
            {
                return Grid.Columns.OfType<EntityGridColumn>().FirstOrDefault(c => c.ColumnGuid == mouseCapturedColumnGuid);
            }
        }

        /// <summary>
        /// Gets the tooltip text.
        /// </summary>
        protected override string GetTooltipText(Point position)
        {
            EntityGridColumn column = HitTestColumn(position);
            string toolTip = column?.GetTooltipText(this);

            return !string.IsNullOrEmpty(toolTip) ?
                toolTip :
                base.GetTooltipText(position);
        }

        /// <summary>
        /// Moving the mouse over the grid row
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // See what column its in
            EntityGridColumn column = null;
            EntityGridColumn capturedColumn = MouseCapturedColumn;

            if (Grid.SandGrid.Capture)
            {
                column = capturedColumn;
            }
            else
            {
                column = HitTestColumn(new Point(e.X, e.Y));
            }

            // The column under the mouse changed
            if (capturedColumn != null && (column == null || column.ColumnGuid != capturedColumn.ColumnGuid))
            {
                capturedColumn.OnCellMouseLeave(this);
            }

            // There is a new column
            if (column != null)
            {
                column.OnCellMouseMove(this, e);
            }

            // This is now the column under the mouse
            mouseCapturedColumnGuid = (column != null) ? column.ColumnGuid : (Guid?) null;

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Mouse is being clicked on this row.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // See what column its in
            EntityGridColumn column = HitTestColumn(new Point(e.X, e.Y));
            if (column != null)
            {
                // We should have already captured this in the mouse move
                // Debug.Assert(column == columnUnderMouse);

                if (!column.OnCellMouseDown(this, e))
                {
                    return;
                }
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Mouse is being released on this row
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            EntityGridColumn column = null;
            EntityGridColumn capturedColumn = MouseCapturedColumn;

            if (Grid.SandGrid.Capture)
            {
                column = capturedColumn;
            }
            else
            {
                column = HitTestColumn(new Point(e.X, e.Y));
            }

            if (column != null)
            {
                // We should have already captured this in the mouse move
                // Debug.Assert(column == columnUnderMouse);

                column.OnCellMouseUp(this, e);
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Mouse is leaving the area of this row
        /// </summary>
        protected override void OnMouseLeave()
        {
            EntityGridColumn capturedColumn = MouseCapturedColumn;

            if (capturedColumn != null)
            {
                capturedColumn.OnCellMouseLeave(this);
                mouseCapturedColumnGuid = null;
            }

            base.OnMouseLeave();
        }

        /// <summary>
        /// Determine what column the mouse event falls in for this row.  Or null if it doesn't.
        /// </summary>
        private EntityGridColumn HitTestColumn(Point point)
        {
            if (Grid != null)
            {
                foreach (EntityGridColumn column in Grid.Columns.OfType<EntityGridColumn>())
                {
                    if (point.X > column.Bounds.Left && point.X < column.Bounds.Right)
                    {
                        return column;
                    }
                }
            }

            return null;
        }
    }
}
