using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using ShipWorks.UI;
using Interapptive.Shared;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Colors
{
	/// <summary>
	/// A menu-like popup color picker
	/// </summary>
	public class ColorPickerPopup : PopupWindow
	{
		#region Windows Form Designer generated code

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            // 
            // ColorPickerPopup
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(158, 132);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.Name = "ColorPickerPopup";
            this.Text = "ColorPickerPopup";
            this.Load += new System.EventHandler(this.OnLoad);

        }
		#endregion

        #region class Entry

        class Entry
        {
            public string htmlColor;
            public string name;

            public Entry(string htmlColor, string name)
            {
                this.htmlColor = htmlColor;
                this.name = name;
            }
        }

        #endregion

        #region Color Entries

        // All color entries to be displayed
        static Entry[,] colorEntries = new Entry[,]
            {
                // Row 1
                {
                    new Entry("#000000", "Black"),     new Entry("#993300", "Brown"),     new Entry("#333300", "Olive Green"), new Entry("#003300", "Dark Green"),
                    new Entry("#003366", "Dark Teal"), new Entry("#000080", "Dark Blue"), new Entry("#333399", "Indigo"),      new Entry("#333333", "Gray-80%")
                },

                // Row 2
                {
                    new Entry("#800000", "Dark Red"),  new Entry("#FF6600", "Orange"),   new Entry("#808000", "Dark Yellow"),  new Entry("#008000", "Green"),
                    new Entry("#008080", "Teal"),      new Entry("#0000FF", "Blue"),     new Entry("#666699", "Blue-Gray"),    new Entry("#808080", "Gray-50%")
                },

                // Row 3
                {
                    new Entry("#FF0000", "Red"),       new Entry("#FF9900", "Light Orange"), new Entry("#99CC00", "Lime"),      new Entry("#339966", "Sea Green"),
                    new Entry("#33CCCC", "Aqua"),      new Entry("#3366FF", "Light Blue"),   new Entry("#800080", "Violet"),    new Entry("#999999", "Gray-40%")
                },

                // Row 4
                {
                    new Entry("#FF00FF", "Pink"),       new Entry("#FFCC00", "Gold"),         new Entry("#FFFF00", "Yellow"),   new Entry("#00FF00", "Bright Green"),
                    new Entry("#00FFFF", "Turqoise"),   new Entry("#00CCFF", "Sky Blue"),     new Entry("#993366", "Plum"),     new Entry("#C0C0C0", "Gray-25%")
                },

                // Row 5
                {
                    new Entry("#FF99CC", "Rose"),       new Entry("#FFCC99", "Tan"),          new Entry("#FFFF99", "Light Yellow"),  new Entry("#CCFFCC", "Light Green"),
                    new Entry("#CCFFFF", "Light Turquoise"), new Entry("#99CCFF", "Pale Blue"), new Entry("#CC99FF", "Lavender"),    new Entry("#FFFFFF", "White")
                }
            };

        #endregion

        // Setup dimensions for each entry
        int entryWidth = 18;
        int entryHeight = 18;
        int spacing = 0;		
        int padding = 3;
		int border = 5;

        // Height of the "more colors" button
        int moreColorsHeight = 22;

        // Color currently used
        Color color = Color.White;

        /// <summary>
        /// Constructor
        /// </summary>
		public ColorPickerPopup(Color color)
		{
			InitializeComponent();

            this.color = color;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
		}

        /// <summary>
        /// The chosen color
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
        }

        /// <summary>
        /// Get the name of the given color as it is displayed in our ToolTips. If
        /// its not a standard color, the RGB value is returned.
        /// </summary>
        public static string GetColorName(Color color)
        {
            int rows = colorEntries.GetLength(0);
            int cols = colorEntries.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Color entryColor = ColorTranslator.FromHtml(colorEntries[row, col].htmlColor);
                    if (ColorTranslator.ToWin32(entryColor) == ColorTranslator.ToWin32(color))
                    {
                        return colorEntries[row, col].name;
                    }
                }
            }

            return string.Format("RGB({0}, {1}, {2})", color.R, color.G, color.B);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, System.EventArgs e)
        {
            BackColor = DisplayHelper.DarkenColor(SystemColors.Window, .02);

            BuildPalette();

            this.MouseMove += new MouseEventHandler(OnNeedRefresh);
            this.MouseDown += new MouseEventHandler(OnNeedRefresh);
            this.MouseUp += new MouseEventHandler(OnNeedRefresh);
        }

        /// <summary>
        /// Redraw when the mouse moves
        /// </summary>
        private void OnNeedRefresh(object sender, MouseEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Paint a border around ourselves
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint (e);

            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.FromArgb(150, 150, 150), ButtonBorderStyle.Solid);
        }

        /// <summary>
        /// Override CreateParams and clip children for flicker reduction
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.Style |= NativeMethods.WS_CLIPCHILDREN;

                return cp;
            }
        }

        /// <summary>
        /// Build the color panels
        /// </summary>
        [NDependIgnoreLongMethod]
        private void BuildPalette()
        {
            int rows = colorEntries.GetLength(0);
            int cols = colorEntries.GetLength(1);

            int paletteWidth = border + (entryWidth * cols)  + (spacing * (cols - 1)) + border;
            int paletteHeight = border + (entryHeight * rows) + (spacing * (rows - 1)) + border;

            // We are sized based on the entries
            Size = new Size(paletteWidth, paletteHeight + moreColorsHeight + border);

            // Create the tooltop that will display for each color
            ToolTip toolTip = new ToolTip();	
            toolTip.ReshowDelay = 0;
		
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Color entryColor = ColorTranslator.FromHtml(colorEntries[row, col].htmlColor);
                    bool active = (ColorTranslator.ToWin32(entryColor) == ColorTranslator.ToWin32(color));
                    Panel entryPanel = new EntryPanel(entryColor, active, padding);

                    int x = border + (entryWidth * col) + (spacing * col);
                    int y = border + (entryHeight * row) + (spacing * row);

                    entryPanel.Location = new Point(x, y);
                    entryPanel.Size = new Size(entryWidth, entryHeight);			

                    // Set the tooltip
                    toolTip.SetToolTip(entryPanel, colorEntries[row, col].name);

                    // Listen for the choice
                    entryPanel.MouseUp += new MouseEventHandler(OnChooseColor);

                    // Redraw with each mouse movement
                    entryPanel.MouseMove += new MouseEventHandler(OnNeedRefresh);
                    entryPanel.MouseDown += new MouseEventHandler(OnNeedRefresh);
                    entryPanel.MouseUp += new MouseEventHandler(OnNeedRefresh);
        										
                    // Add the panel
                    this.Controls.Add(entryPanel);			
                }
            }

            // Create the moreColors panel
            Panel panel = new MoreColorsPanel();
            panel.Location = new Point(border, Height - border - moreColorsHeight);
            panel.Size = new Size(Width - (2 * border), moreColorsHeight);
            panel.Font = Font;

            // Set the tooltip
            toolTip.SetToolTip(panel, "More Colors");

            // Listen for the choice
            panel.MouseUp += new MouseEventHandler(OnMoreColors);

            // Redraw with each mouse movement
            panel.MouseMove += new MouseEventHandler(OnNeedRefresh);
            panel.MouseDown += new MouseEventHandler(OnNeedRefresh);
            panel.MouseUp += new MouseEventHandler(OnNeedRefresh);
        										
            // Add the panel
            this.Controls.Add(panel);			
        }
	
        /// <summary>
        /// User has made a choice
        /// </summary>
        void OnChooseColor(object sender, MouseEventArgs e)
        {	
            color = ((EntryPanel) sender).Color;
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// User wants to see more colors
        /// </summary>
        void OnMoreColors(object sender, MouseEventArgs e)
        {			
            Hide();

            ColorChooser chooser = new ColorChooser(color);
            if (chooser.ShowDialog(this) == DialogResult.OK)
            {
                color = chooser.Color;
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// A single color palette entry
        /// </summary>
        class EntryPanel : Panel
        {
            // Color we represent
            Color color;

            // If this is the color the user is currently using
            bool active = false;

            // Paddint between our client edge and color drawn
            int padding = 0;

            /// <summary>
            /// Constructor
            /// </summary>
            public EntryPanel(Color color, bool active, int padding)
            {
                this.color = color;
                this.active = active;
                this.padding = padding;

                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.DoubleBuffer, true);
            }

            /// <summary>
            /// The color this panel represents
            /// </summary>
            public Color Color
            {
                get
                {
                    return color;
                }
            }

            /// <summary>
            /// Paint based on the current state
            /// </summary>
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint (e);

                Graphics g = e.Graphics;

                Rectangle colorArea = new Rectangle(
                    padding, padding, Width - (2 * padding), Height - (2 * padding));
				
                // Mouse is within the panel or we are active color
                if (active || ClientRectangle.Contains(PointToClient(Cursor.Position)))
                {
                    Color selectColor = SystemColors.Highlight;

                    // Selection fill color
                    Color selectFillColor = DisplayHelper.LightenColor(selectColor, 0.7);

                    // Mouse pressed, or for active, being hoverd
                    if ((!active && Control.MouseButtons != MouseButtons.None) || 
                        (active && ClientRectangle.Contains(PointToClient(Cursor.Position))))
                    {
                        selectFillColor = DisplayHelper.DarkenColor(selectFillColor, .2);
                    }

                    using (SolidBrush brush = new SolidBrush(selectFillColor))
                    {
                        g.FillRectangle(brush, ClientRectangle);
                    }

                    using (Pen pen = new Pen(selectColor))
                    {
                        Rectangle border = ClientRectangle;
                        border.Width -= 1;
                        border.Height -= 1;

                        g.DrawRectangle(pen, border);
                    }
                }

                using (SolidBrush brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, colorArea);
                }

                using (Pen pen = new Pen(Color.FromArgb(180, 180, 180)))
                {
                    Rectangle border = colorArea;
                    border.Width -= 1;
                    border.Height -= 1;

                    g.DrawRectangle(pen, border);
                }
            }
        }

        /// <summary>
        /// The "More Colors..." button
        /// </summary>
        class MoreColorsPanel : Panel
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public MoreColorsPanel()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.DoubleBuffer, true);
            }

            /// <summary>
            /// Paint based on the current state
            /// </summary>
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint (e);

                Graphics g = e.Graphics;
				
                // Mouse is within the panel
                if (ClientRectangle.Contains(PointToClient(Cursor.Position)))
                {
                    Color selectColor = SystemColors.Highlight;

                    // Selection fill color
                    Color selectFillColor = DisplayHelper.LightenColor(selectColor, 0.7);

                    // No mouse pressed
                    if (Control.MouseButtons == MouseButtons.None)
                    {
                    }
                        // Mouse pressed
                    else
                    {
                        selectFillColor = DisplayHelper.DarkenColor(selectFillColor, .1);
                    }

                    using (SolidBrush brush = new SolidBrush(selectFillColor))
                    {
                        g.FillRectangle(brush, ClientRectangle);
                    }

                    using (Pen pen = new Pen(selectColor))
                    {
                        Rectangle border = ClientRectangle;
                        border.Width -= 1;
                        border.Height -= 1;

                        g.DrawRectangle(pen, border);
                    }
                }
            
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                g.DrawString("More Colors...", Font, Brushes.Black, ClientRectangle, format);            
            }
        }
	}
}
