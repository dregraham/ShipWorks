using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Interapptive.Shared;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Colors
{
	/// <summary>
	/// A ComboBox for selecting color
	/// </summary>
	public class ColorComboBox : ComboBox
	{
        Color color = Color.Honeydew;

        // Determins if the color name will be shown or not
        bool showColorName = false;

        // Tooltip for displaying name of the selected color
        ToolTip toolTip = new ToolTip();

        /// <summary>
        /// Constructor
        /// </summary>
		public ColorComboBox()
		{
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        /// <summary>
        /// The Color that is the current value of the ComboBox
        /// </summary>
        [Category("Appearance")]
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;

                toolTip.SetToolTip(this, ColorPickerPopup.GetColorName(color));

                Invalidate();
            }
        }

        /// <summary>
        /// Determines if the name of the color is shown within the ComboBox
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool ShowColorName
        {
            get
            {
                return showColorName;
            }
            set
            {
                showColorName = value;

                Invalidate();
            }
        }

        /// <summary>
        /// Intercept the mouse down
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            // On mouse down, we show our color chooser
            if (m.Msg == NativeMethods.WM_LBUTTONDOWN)
            {
                if (GetStyle(ControlStyles.Selectable))
                {
                    Focus();
                }

                Point location = Parent.PointToScreen(new Point(Left, Bottom));

                ColorPickerPopup popup = new ColorPickerPopup(color);
                if (popup.ShowPopup((Form) TopLevelControl, location) == DialogResult.OK)
                {
                    Color = popup.Color;
                }
            }

            else
            {
                base.WndProc (ref m);
            }
        }

        /// <summary>
        /// Draw the color in the main area of the combo
        /// </summary>
        protected override void OnDrawItem(DrawItemEventArgs e) 
        {
            // Get the graphics object
            Graphics g = e.Graphics;

            using (SolidBrush brush = new SolidBrush(SystemColors.Window))
            {
                g.FillRectangle(brush, e.Bounds);
            }

            e.DrawFocusRectangle();

            // Determine where we are going to draw the color
            Rectangle colorArea = new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, e.Bounds.Width - 4, e.Bounds.Height - 4);

            // If we are displaying the name of the color
            if (showColorName)
            {
                // Reduce the color area to make room for the text
                colorArea.Width = colorArea.Height;

                Rectangle textArea = new Rectangle(colorArea.Right + 2, e.Bounds.Top, e.Bounds.Width - (colorArea.Right + 2), e.Bounds.Height);

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisCharacter;
                format.FormatFlags = StringFormatFlags.NoWrap;

                using (SolidBrush brush = new SolidBrush(ForeColor))
                {
                    g.DrawString(ColorPickerPopup.GetColorName(color), e.Font, brush, textArea, format);
                }
            }

            // Draw the color
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, colorArea);
            }

            // Draw the border around the color
            using (Pen pen = new Pen(Color.FromArgb(180, 180, 180)))
            {
                Rectangle border = colorArea;
                border.Width -= 1;
                border.Height -= 1;

                g.DrawRectangle(pen, border);
            }
        }
	}
}
