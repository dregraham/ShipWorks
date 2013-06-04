using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using Divelements.SandGrid.Rendering;
using System.Diagnostics;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Custom control for selecting the starting label of a label sheet
    /// </summary>
    public partial class LabelPositionControl : UserControl
    {
        /// <summary>
        /// Raised when the size of the table changes
        /// </summary>
        public event EventHandler LabelPositionChanged;

        LabelSheetEntity labelSheet;
        LabelPosition position = new LabelPosition { Row = 1, Column = 1 };

        Rectangle[,] labelCellBounds;
        Rectangle labelSheetBounds;
        Rectangle statusAreaBounds;

        Color selectedCellColor = Color.FromArgb(100, 140, 231);
        Color outlineColor = Color.FromArgb(210, 210, 210);
        Color textColor = Color.DimGray;
        Color sheetBackgroundColor = Color.White;

        TextFormattingInformation centeredTextFormat;

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelPositionControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            centeredTextFormat = new TextFormattingInformation();
            centeredTextFormat.StringFormat = new StringFormat(StringFormatFlags.NoWrap);
            centeredTextFormat.TextFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

            selectedCellColor = DisplayHelper.LightenColor(selectedCellColor, .4);
        }

        /// <summary>
        /// Gets or sets the label sheet that the control is allowing the user to pick from
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LabelSheetEntity LabelSheet
        {
            get
            {
                return labelSheet;
            }
            set
            {
                labelSheet = value;

                UpdateLabelCellBounds();
                UpdateCurrentPosition(LabelPosition);

                Invalidate();
            }
        }

        /// <summary>
        /// The table size the user has selected
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LabelPosition LabelPosition
        {
            get
            {
                return position;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                UpdateCurrentPosition(value);
            }
        }

        /// <summary>
        /// Ensure the window is update when the mouse leaves
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            Invalidate();
        }

        /// <summary>
        /// Ensure things are recalculated and redrawn when the mouse moves
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if ((Control.MouseButtons & (MouseButtons.Right | MouseButtons.Left)) == 0)
            {
                return;
            }

            LabelPosition position = GetPositionFromPoint(e.Location);

            if (position != null)
            {
                UpdateCurrentPosition(position);
            }
        }

        /// <summary>
        /// User has pressed the mouse, see if they have selected something
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            LabelPosition position = GetPositionFromPoint(e.Location);

            if (position != null)
            {
                UpdateCurrentPosition(position);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            UpdateLabelCellBounds();
        }

        /// <summary>
        /// Custom painting
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            using (Pen outlinePen = new Pen(outlineColor))
            {
                using (Brush backgroundBrush = new SolidBrush(sheetBackgroundColor))
                {
                    if (labelSheet == null)
                    {
                        Rectangle outlineRect = ClientRectangle;
                        outlineRect.Height--;
                        outlineRect.Width--;

                        g.FillRectangle(backgroundBrush, outlineRect);
                        g.DrawRectangle(outlinePen, outlineRect);
                        IndependentText.DrawText(g, "No label sheet selected.", Font, ClientRectangle, centeredTextFormat, textColor);
                    }
                    else
                    {
                        // Draw the label sheet outline
                        g.FillRectangle(backgroundBrush, labelSheetBounds);
                        g.DrawRectangle(outlinePen, labelSheetBounds);

                        // Draw the status text
                        g.FillRectangle(backgroundBrush, statusAreaBounds);
                        g.DrawRectangle(outlinePen, statusAreaBounds);
                        IndependentText.DrawText(g, GetStatusText(), Font, statusAreaBounds, centeredTextFormat, textColor);

                        // Go through each row and colum in the table
                        for (int col = 1; col <= labelCellBounds.GetLength(0); col++)
                        {
                            for (int row = 1; row <= labelCellBounds.GetLength(1); row++)
                            {
                                Rectangle cellRect = labelCellBounds[col - 1, row - 1];
                                DrawLabelCell(g, row, col, cellRect);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draw the cell
        /// </summary>
        private void DrawLabelCell(Graphics g, int row, int col, Rectangle cellRect)
        {
            if (row == position.Row && col == position.Column)
            {
                using (SolidBrush selectedBrush = new SolidBrush(selectedCellColor))
                {
                    g.FillRectangle(selectedBrush, cellRect);
                }
            }

            else if (((row == position.Row) && (col >= position.Column)) ||(row > position.Row))
            {
                using (SolidBrush lightBrush = new SolidBrush(DisplayHelper.LightenColor(selectedCellColor, .5)))
                {
                    g.FillRectangle(lightBrush, cellRect);
                }
            }

            using (Pen outlinePen = new Pen(outlineColor))
            {
                g.DrawRectangle(outlinePen, cellRect);
            }
        }

        /// <summary>
        /// Get the text to display in the status area of the control
        /// </summary>
        private string GetStatusText()
        {
            string text = "";

            if (position.Row >= 1 && position.Column >= 1)
            {
                text = string.Format("Row {0:d}, Column {1:d}", position.Row, position.Column);
            }

            return text;
        }

        /// <summary>
        /// Gets the cell that is under the given point
        /// </summary>
        private LabelPosition GetPositionFromPoint(Point pt)
        {
            if (labelSheet != null)
            {
                for (int column = 1; column <= labelSheet.Columns; column++)
                {
                    for (int row = 1; row <= labelSheet.Rows; row++)
                    {
                        Rectangle cellBounds = labelCellBounds[column - 1, row - 1];

                        if (cellBounds.Contains(pt))
                        {
                            return new LabelPosition(column, row);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Update the bounds information for all of the label cells
        /// </summary>
        private void UpdateLabelCellBounds()
        {
            if (labelSheet == null)
            {
                labelCellBounds = null;
                labelSheetBounds = new Rectangle();
                statusAreaBounds = new Rectangle();

                return;
            }

            int textAreaHeight = (int) (Font.GetHeight() * 1.6);

            // The size available to the actual sheet excludes the status text area
            Size sheetArea = new Size(ClientRectangle.Width, ClientRectangle.Height - textAreaHeight);

            // Determine the ratio to use to scale the sheet values
            double xPixelsPerInch = sheetArea.Width / labelSheet.PaperSizeWidth;
            double yPixelsPerInch = sheetArea.Height / labelSheet.PaperSizeHeight;
            double pixlesPerInch = Math.Min(xPixelsPerInch, yPixelsPerInch);

            // Create and center the sheet rectangle
            labelSheetBounds = new Rectangle(0, 0, (int) (pixlesPerInch * labelSheet.PaperSizeWidth), (int) (pixlesPerInch * labelSheet.PaperSizeHeight));
            labelSheetBounds.X = (ClientRectangle.Width - labelSheetBounds.Width) / 2;
            labelSheetBounds.Y = (ClientRectangle.Height - (labelSheetBounds.Height + textAreaHeight)) / 2;

            // Determine spot for status area
            statusAreaBounds = new Rectangle(labelSheetBounds.X, labelSheetBounds.Bottom, labelSheetBounds.Width, textAreaHeight - 1);

            // Create the multi-dimensional array to hold the sheet cell bounds
            labelCellBounds = new Rectangle[labelSheet.Columns, labelSheet.Rows];

            // Go through each row and colum in the table
            for (int row = 1; row <= labelSheet.Rows; row++)
            {
                // Go through each colum in the row
                for (int col = 1; col <= labelSheet.Columns; col++)
                {
                    // Get the position of this label
                    double leftInches = labelSheet.MarginLeft + (col - 1) * (labelSheet.HorizontalSpacing + labelSheet.LabelWidth);
                    double topInches = labelSheet.MarginTop + (row - 1) * (labelSheet.VerticalSpacing + labelSheet.LabelHeight);

                    // Translate to pixels
                    int left = labelSheetBounds.X + (int) (leftInches * pixlesPerInch);
                    int top = labelSheetBounds.Y + (int) (topInches * pixlesPerInch);
                    int width = (int) (labelSheet.LabelWidth * pixlesPerInch);
                    int height = (int) (labelSheet.LabelHeight * pixlesPerInch);

                    Rectangle cellRect = new Rectangle(left, top, width, height);

                    labelCellBounds[col - 1, row - 1] = cellRect;
                }
            }
        }

        /// <summary>
        /// The current position has changed
        /// </summary>
        private void UpdateCurrentPosition(LabelPosition newPosition)
        {
            if (labelSheet == null)
            {
                newPosition = new LabelPosition(1, 1);
            }
            else
            {
                newPosition.Row = Math.Max(1, Math.Min(labelSheet.Rows, newPosition.Row));
                newPosition.Column = Math.Max(1, Math.Min(labelSheet.Columns, newPosition.Column));
            }

            if (newPosition.Column == position.Column && newPosition.Row == position.Row)
            {
                return;
            }

            position = newPosition;

            OnLabelPositionChanged();
            Invalidate();
        }

        /// <summary>
        /// Raises the LabelPositionChanged event
        /// </summary>
        protected virtual void OnLabelPositionChanged()
        {
            if (LabelPositionChanged != null)
            {
                LabelPositionChanged(this, EventArgs.Empty);
            }
        }
    }
}
