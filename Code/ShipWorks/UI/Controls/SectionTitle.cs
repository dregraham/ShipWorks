using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Divelements.SandRibbon.Rendering;
using ShipWorks.ApplicationCore.Appearance;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Control that draws a section-title similar to those in the Office 2007 options windows.
    /// </summary>
    public partial class SectionTitle : ContainerControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SectionTitle()
        {
            Height = 22;

            InitializeComponent();

            this.TextChanged += new EventHandler(OnTextChanged);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Opaque, true);
        }

        /// <summary>
        /// We have to redraw whenever the Text changes.
        /// </summary>
        void OnTextChanged(object sender, EventArgs e)
        {
            if (IsHandleCreated)
            {
                Invalidate();
            }
        }

        /// <summary>
        /// Drawing
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            using (RibbonRenderer renderer = AppearanceHelper.CreateRibbonRenderer())
            {
                // Draw the background
                using (SolidBrush backBrush = new SolidBrush(Color.FromArgb(238, 238, 238)))
                {
                    e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
                }

                // Draw the bottom line
                using (Pen pen = new Pen(renderer.ColorTable.MenuSeparator))
                {
                    e.Graphics.DrawLine(pen, 0, Height - 1, Width, Height - 1);
                }

                using (RenderingContext context = AppearanceHelper.CreateRibbonRenderingContext(e.Graphics, renderer, Font, false, Color.FromArgb(90, 90, 90), renderer.ColorTable.RibbonDisabledText))
                {
                    using (Font boldFont = new Font(Font, FontStyle.Bold))
                    {
                        Rectangle textRect = ClientRectangle;
                        textRect.Offset(10, 0);

                        // Draw the title
                        renderer.DrawText(context, textRect, Text, boldFont, context.StandardNearsideText, DrawState.Normal, context.DefaultTextColor);
                    }
                }
            }

            // Calling the base class OnPaint
            base.OnPaint(e);
        }
    }
}
