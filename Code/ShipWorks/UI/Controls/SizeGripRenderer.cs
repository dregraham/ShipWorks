using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// This class is for drawing the SizeGrip on a Form.  The default Form implementation causes flicker when our HtmlControl
    /// is on the form.  This does not.
    /// </summary>
    public class SizeGripRenderer
    {
        Form form;

        VisualStyleRenderer sizeGripRenderer;
        Rectangle gripLocation = new Rectangle();

        /// <summary>
        /// Install the renderer for the given form
        /// </summary>
        public SizeGripRenderer(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }

            this.form = form;

            form.Paint += new PaintEventHandler(OnPaint);
            form.Resize += new EventHandler(OnResize);
            form.FormClosed += new FormClosedEventHandler(OnClosed);

            UpdateAndInvalidate();
        }

        /// <summary>
        /// Paint the size grip
        /// </summary>
        void OnPaint(object sender, PaintEventArgs e)
        {
            if (Application.RenderWithVisualStyles)
            {
                if (sizeGripRenderer == null)
                {
                    sizeGripRenderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
                }

                sizeGripRenderer.DrawBackground(e.Graphics, gripLocation);
            }
            else
            {
                ControlPaint.DrawSizeGrip(e.Graphics, form.BackColor, gripLocation.Left, gripLocation.Top, 16, 16);
            }

        }

        /// <summary>
        /// The form is resizing
        /// </summary>
        void OnResize(object sender, EventArgs e)
        {
            UpdateAndInvalidate();
        }

        /// <summary>
        /// Invalidate and update
        /// </summary>
        private void UpdateAndInvalidate()
        {
            form.Invalidate(gripLocation);
            gripLocation = new Rectangle(form.ClientSize.Width - 16, form.ClientSize.Height - 16, 16, 16);
            form.Invalidate(gripLocation);
        }

        /// <summary>
        /// Form is being closed
        /// </summary>
        void OnClosed(object sender, FormClosedEventArgs e)
        {
            form.Paint -= new PaintEventHandler(OnPaint);
            form.Resize -= new EventHandler(OnResize);
            form.FormClosed -= new FormClosedEventHandler(OnClosed);
        }
    }
}
