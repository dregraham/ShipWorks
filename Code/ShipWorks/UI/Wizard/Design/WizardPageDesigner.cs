using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ShipWorks.UI.Wizard.Design
{
	/// <summary>
	/// Desiger for the WizardPage control
	/// </summary>
	internal class WizardPageDesigner : ParentControlDesigner
	{
        // For drawing
        private static SolidBrush brushBack = new SolidBrush( Color.White );
        private static SolidBrush brushText = new SolidBrush( Color.Gray );
        private static Pen border           = new Pen(Color.Gray);

        // Indicates if guidelines should be displayed
        bool guideLines = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public WizardPageDesigner()
        {

        }

        /// <summary>
        /// Indicates whether guidelines should be drawn on the pages
        /// </summary>
        [DefaultValue(true)]
        public bool GuideLines
        {
            get
            {
                return guideLines;
            }
            set
            {
                if (guideLines == value)
                    return;

                guideLines = value;
          
                if (Control != null)
                {
                    Control.Invalidate();
                }
            }
        }

        /// <summary>
        /// Remove properties from the designer property grid
        /// </summary>
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            base.DrawGrid = false;
        }

        /// <summary>
        /// Custom drawing of the Wizard Page in the designer
        /// </summary>
        protected override void OnPaintAdornments(System.Windows.Forms.PaintEventArgs pe)
        {
            base.OnPaintAdornments(pe);
      
            Graphics g = pe.Graphics;

            WizardPage page = (WizardPage) Control;
            WizardForm wizard = page.Wizard;
                        
            // Draw the guidelines
            if (page != null && guideLines)
            {
                Rectangle rect = page.ClientRectangle;

                if( rect.Width % 8 > 0 )
                    rect.Width = (int)( rect.Width / 8 ) * 8;

                if( rect.Height % 8 > 0 )
                    rect.Height = (int)( rect.Height / 8 ) * 8;

                // Draw guidelines per Wizard97 standards
                Point[] linePoints = new Point[]
                    {
                        new Point( new Size( rect.X + 24, rect.Y + 8 ) ),
                        new Point( new Size( rect.X + 24, rect.Bottom - 8 ) ),
                        new Point( new Size( rect.X + 40, rect.Bottom - 8 ) ),
                        new Point( new Size( rect.X + 40, rect.Y + 8 ) ),
                        new Point( new Size( rect.X + 56, rect.Y + 8 ) ),
                        new Point( new Size( rect.X + 56, rect.Bottom - 8 ) ),
                        
                        new Point( new Size( rect.Right - 56, rect.Bottom - 8 ) ),
                        new Point( new Size( rect.Right - 56, rect.Y + 8 ) ),
                        new Point( new Size( rect.Right - 40, rect.Y + 8 ) ),
                        new Point( new Size( rect.Right - 40, rect.Bottom - 8 ) ),
                        new Point( new Size( rect.Right - 24, rect.Bottom - 8 ) ),
                        new Point( new Size( rect.Right - 24, rect.Y + 8 ) ),
                        
                        new Point( new Size( rect.X + 24, rect.Y + 8 ) ),
                        new Point( new Size( rect.X + 24, rect.Bottom - 8 ) ),
                        new Point( new Size( rect.Right - 24, rect.Bottom - 8 ) )
                    };

                // Draw the lines
                using (Pen pen = new Pen( Brushes.Gray ))
                {
                    pen.DashStyle = DashStyle.Dot;
                    g.DrawLines(pen, linePoints );
                }
            }

            // Location where the index number will be drawn
            Rectangle indexRect = new Rectangle(0, page.Height - 20, 32, 16);
            
            string index;
      
            // Get the index of this page in the wizard
            if (page != null && wizard != null)
            {
                index = page.Site.Name;
            }
            else
            {
                index = "?";
            }

            indexRect.Width = (int) g.MeasureString(index, Control.Font).Width + 4;

            // Fill the background
            g.FillRectangle(brushBack, indexRect);
            g.DrawRectangle(border, indexRect);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Draw the index number
            g.DrawString(index, Control.Font, brushText, indexRect, format);
        }
    }
}
