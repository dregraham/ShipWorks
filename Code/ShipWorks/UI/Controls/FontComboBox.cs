using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Divelements.SandGrid.Rendering;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// ComboBox for displaying fonts in there actual font
    /// </summary>
    public class FontComboBox : ComboBox
    {
        TextFormattingInformation tfi = new TextFormattingInformation();

        /// <summary>
        /// Constructor
        /// </summary>
        public FontComboBox()
        {
            this.DropDownWidth = 200;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = 15;

            tfi.StringFormat = new StringFormat(StringFormatFlags.NoWrap);
        }

        /// <summary>
        /// The Win32 handle of the control is being created
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            
            PopulateFontList();
        }

        /// <summary>
        /// Popuplate the ComboBox with installed fonts
        /// </summary>
        private void PopulateFontList()
        {
            string currentFont = this.Text;
            Items.Clear();

            Dictionary<string, string> uniqueFamilies = new Dictionary<string, string>();

            // If we are not doing only fixed width fonts, just add all font families
            foreach (FontFamily family in FontFamily.Families)
            {
                uniqueFamilies[family.Name.ToLowerInvariant()] = family.Name;
            }

            Items.AddRange(uniqueFamilies.Values.OrderBy(s => s).ToArray());

            if (currentFont.Length != 0)
            {
                this.Text = currentFont;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ComboBox.ObjectCollection Items 
        {
            get { return base.Items; }
        }       

        /// <summary>
        /// Determine the height of each item in the control
        /// </summary>
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            if (e.Index > 0)
            {
                e.ItemHeight = 18;
            }

            base.OnMeasureItem(e);
        }

        /// <summary>
        /// Draw the font item
        /// </summary>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (!Enabled)
            {
                e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
            }
            else
            {
                e.DrawBackground();

                string fontName = null;
                if (e.Index == -1)
                {
                    // Text shown in the combobox itself
                    fontName = this.Text;
                }
                else
                {
                    fontName = (string) this.Items[e.Index];
                }

                using (Font font = CreateFont(fontName, e.Bounds))
                {
                    Rectangle textBounds = new Rectangle(e.Bounds.Left + 2, e.Bounds.Top, e.Bounds.Width - 2, e.Bounds.Height);

                    IndependentText.DrawText(e.Graphics, fontName, (font != null) ? font : e.Font, textBounds, tfi, e.ForeColor);
                }
            }
        }

        /// <summary>
        /// Create a font based on the given name.  If it cant be created null is returned.
        /// </summary>
        private Font CreateFont(string fontName, Rectangle itemBounds)
        {
            if (!string.IsNullOrEmpty(fontName))
            {
                try
                {
                    using (FontFamily fontFamily = new FontFamily(fontName))
                    {
                        FontStyle fontStyle = FontStyle.Regular;

                        // If Regular doesnt exist, move on to Italic
                        if (!fontFamily.IsStyleAvailable(fontStyle))
                        {
                            fontStyle = FontStyle.Italic;

                            // Then try bold
                            if (!fontFamily.IsStyleAvailable(fontStyle))
                            {
                                fontStyle = FontStyle.Bold;
                                if (!fontFamily.IsStyleAvailable(fontStyle))
                                {
                                    return null;
                                }
                            }
                        }

                        return new Font(fontName, (float) ((itemBounds.Height - 2) / 1.2), fontStyle, GraphicsUnit.Pixel);
                    }
                }
                // Unable to create font family
                catch (ArgumentException)
                {
                    return null;
                }
            }

            return null;
        }
    }
}
