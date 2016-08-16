using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using Divelements.SandGrid.Specialized;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Grid.Columns;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Grid
{
    /// <summary>
    /// Custom grid column used by ShipWorks
    /// </summary>
    public class EntityGridColumn : TypedGridColumn
    {
        GridColumnDefinition definition;

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityGridColumn(GridColumnDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            this.definition = definition;
            this.HeaderText = definition.HeaderText;

            ForeColorSource = CellForeColorSource.RowCell;
        }

        /// <summary>
        /// The ShipWorks identifier of the column.
        /// </summary>
        public Guid ColumnGuid
        {
            get
            {
                return definition.ColumnGuid;
            }
        }

        /// <summary>
        /// The definition that defines the properties that created the column
        /// </summary>
        public GridColumnDefinition Definition
        {
            get { return definition; }
        }

        /// <summary>
        /// The display type used for value formatting and display
        /// </summary>
        public GridColumnDisplayType DisplayType
        {
            get { return definition.DisplayType; }
        }

        /// <summary>
        /// Create default text formatting information for cells and columns
        /// </summary>
        protected override TextFormattingInformation CreateTextFormat(GridColumnTextFormatType textFormatType)
        {
            TextFormattingInformation tfi = base.CreateTextFormat(textFormatType);

            if (textFormatType == GridColumnTextFormatType.Header)
            {
                tfi.TextFormatFlags &= ~TextFormatFlags.EndEllipsis;
                tfi.TextFormatFlags &= ~TextFormatFlags.VerticalCenter;
            }

            return tfi;
        }

        /// <summary>
        /// Override drawing of the cell
        /// </summary>
        [NDependIgnoreTooManyParams]
        protected override void DrawCell(RenderingContext context, GridRow row, object value, Font cellFont, Image image, Rectangle bounds, bool selected, TextFormattingInformation textFormat, Color cellForeColor)
        {
            // This is because right now SandGrid does not work with the outlook summary thing unless we override the bounds here.  If its ever fixed, we can override the bounds
            // in a GridRow.DrawRowForeground override.
            bounds.Height = DetailViewSettings.SingleRowHeight;
            
            base.DrawCell(context, row, value, cellFont, image, bounds, selected, textFormat, cellForeColor);
        }

        /// <summary>
        /// Overide the default size of the column header
        /// </summary>
        protected override Size MeasureCore(Graphics graphics, TextFormattingInformation textFormat, bool rtl)
        {
            Size size = base.MeasureCore(graphics, textFormat, rtl);

            if (Definition.ShowStoreTypeInHeader)
            {
                // 6 is what SandGrid adds as margin (as seen in Reflector). We don't need to double the margin, so subtract out the doubling.
                size.Height = (size.Height * 2) - 6;
            }

            return size;
        }

        /// <summary>
        /// Customized drawing of the column header to show store type
        /// </summary>
        protected override void DrawHeader(RenderingContext context, TextFormattingInformation textFormat)
        {
            base.DrawHeader(context, textFormat);

            // See if we are showing store types, and it also has to be wider than "iconized"
            if (Definition.ShowStoreTypeInHeader && Bounds.Width > 28)
            {
                Rectangle bounds = new Rectangle(new Point(Bounds.Left, Bounds.Top + (Bounds.Height / 2)), new Size(Bounds.Width, Bounds.Height / 2));
                bounds.Inflate(-3, 0);

                TextFormatFlags oldFlags = textFormat.TextFormatFlags;
                textFormat.TextFormatFlags |= TextFormatFlags.EndEllipsis;

                IndependentText.DrawText(context.Graphics, string.Format("({0})", EnumHelper.GetDescription(Definition.StoreTypeCode)), context.Font, bounds, textFormat, SystemColors.GrayText);

                textFormat.TextFormatFlags = oldFlags;
            }
        }

        /// <summary>
        /// Can be overridden by derived types for special preview processing
        /// </summary>
        [NDependIgnoreTooManyParams]
        protected virtual void DrawPreview(RenderingContext context, GridRow row, object exampleValue, Font cellFont, Image image, Rectangle bounds, bool selected, TextFormattingInformation textFormat, Color cellForeColor)
        {
            GridColumnFormattedValue formattedValue = DisplayType.FormatPreview(exampleValue);

            // Use the formatted color if provided
            cellForeColor = formattedValue.ForeColor != null ? formattedValue.ForeColor.Value : cellForeColor;

            bool disposeFont = false;

            // Modify the font if needed
            if (formattedValue.FontStyle != null)
            {
                disposeFont = true;
                cellFont = new Font(cellFont, formattedValue.FontStyle.Value);
            }

            DrawCell(context, row, formattedValue.Text, cellFont, formattedValue.Image, bounds, selected, textFormat, cellForeColor);

            // If we created our own font we have to clean up
            if (disposeFont)
            {
                cellFont.Dispose();
            }
        }

        /// <summary>
        /// Draw a preview of what the column data will look like with the given example value.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public void DrawPreview(RenderingContext context, GridRow row, object exampleValue, Font cellFont, Image image, Rectangle bounds, bool selected, Color cellForeColor)
        {
            // Need text format based on the preview column
            using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(
                Grid.RightToLeft,
                AllowWrap,
                CellHorizontalAlignment,
                CellVerticalAlignment,
                ClipText))
            {
                DrawPreview(context, row, exampleValue, cellFont, image, bounds, selected, textFormat, cellForeColor);
            }
        }

        /// <summary>
        /// The mouse is moving over a cell defined by this column for the given row.
        /// </summary>
        internal virtual void OnCellMouseMove(EntityGridRow row, MouseEventArgs e)
        {
            DisplayType.OnCellMouseMove(row, this, e);
        }

        /// <summary>
        /// The mouse is being pressed in a cell defined by this column.  Return false to cancel the press.
        /// </summary>
        internal virtual bool OnCellMouseDown(EntityGridRow row, MouseEventArgs e)
        {
            return DisplayType.OnCellMouseDown(row, this, e);
        }

        /// <summary>
        /// The mouse is being released in a cell defined by this column.
        /// </summary>
        internal virtual void OnCellMouseUp(EntityGridRow row, MouseEventArgs e)
        {
            DisplayType.OnCellMouseUp(row, this, e);
        }

        /// <summary>
        /// The mouse has left the area of the cell for the given row
        /// </summary>
        internal virtual void OnCellMouseLeave(EntityGridRow row)
        {
            DisplayType.OnCellMouseLeave(row, this);
        }

        /// <summary>
        /// Get the bounds of the cell represented by the given row and this column
        /// </summary>
        protected Rectangle GetCellBounds(EntityGridRow row)
        {
            Rectangle bounds = new Rectangle(this.Bounds.Left, row.Bounds.Top, this.Bounds.Width, row.Bounds.Height);
            return bounds;
        }

        /// <summary>
        /// Get the bounds of the text displayed within the given row in this column
        /// </summary>
        public Rectangle GetTextBounds(EntityGridRow row, GridColumnFormattedValue formattedValue)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            if (formattedValue == null)
            {
                throw new ArgumentNullException("formattedValue");
            }

            Rectangle cellBounds = GetCellBounds(row);

            // Start out assuming empty bounds
            Rectangle textBounds = cellBounds;
            textBounds.Width = 0;

            if (Grid == null || Grid.SandGrid == null)
            {
                throw new InvalidOperationException(string.Format("GetTextBounds: {0} is null.", (Grid == null) ? "Grid" : "Grid.SandGrid"));
            }

            // No need to measure if the text is empty
            if (!string.IsNullOrWhiteSpace(formattedValue.Text))
            {
                string text = formattedValue.Text ?? "";
                bool hasImage = formattedValue.Image != null;

                using (Graphics g = Grid.SandGrid.CreateGraphics())
                {
                    using (TextFormattingInformation tfi = new TextFormattingInformation())
                    {
                        Size size = IndependentText.MeasureText(g, text, Grid.Font, tfi);

                        // Update the bounds
                        textBounds.Width = size.Width;
                        textBounds.Height = Math.Min(cellBounds.Height, size.Height + 4);

                        if (CellHorizontalAlignment == StringAlignment.Center)
                        {
                            int freeSpace = Math.Max(0, cellBounds.Width - size.Width);

                            textBounds.X += (freeSpace / 2);
                        }

                        if (CellHorizontalAlignment == StringAlignment.Far)
                        {
                            int freeSpace = Math.Max(0, cellBounds.Width - size.Width);

                            textBounds.X += freeSpace;
                        }

                        if (hasImage && CellHorizontalAlignment == StringAlignment.Near)
                        {
                            textBounds.X += 24;
                        }

                        if (hasImage && CellHorizontalAlignment == StringAlignment.Far)
                        {
                            textBounds.X -= 16;
                        }

                    }
                }
            }

            return textBounds;
        }

        /// <summary>
        /// Gets the tooltip text.
        /// </summary>
        public string GetTooltipText(EntityGridRow row)
        {
            return DisplayType.GetTooltipText(row, this);
        }
    }
}
