using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Interapptive.Shared;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// A class that lets a textbox represent multiple values
    /// </summary>
    public class MultiValueTextBox : TextBox
    {
        private bool isMultiValued = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiValueTextBox()
        {

        }

        /// <summary>
        /// Get \ set whether the box represents a null value.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public virtual bool MultiValued
        {
            get
            {
                return isMultiValued;
            }
            set
            {
                if (value == isMultiValued)
                {
                    return;
                }

                if (value)
                {
                    base.Text = "";
                }

                isMultiValued = value;
                Invalidate();
            }
        }

        /// <summary>
        /// When the text changes, we are no longer null.  We do need this in addition to the handler.  The handler is required
        /// for when the user is typing.  This is required for cases where the underlying Text property is "", and its being set to "", in 
        /// which case the TextChanged handler would not have fired.
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;

                if (isMultiValued)
                {
                    isMultiValued = false;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// When the text changes, we are no longer null.
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
 	        base.OnTextChanged(e);

            if (isMultiValued)
            {
                isMultiValued = false;
                Invalidate();
            }
        }

        /// <summary>
        /// Overridden for custom drawing of the prompt
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Only draw the prompt on the WM_PAINT event and its multivalued
            if (m.Msg == NativeMethods.WM_PAINT && !Focused && isMultiValued)
            {
                using (Graphics g = CreateGraphics())
                {
                    DrawMultiValuePrompt(g);
                }
            }
        }

        /// <summary>
        /// Draws the PromptText in the TextBox.ClientRectangle using the PromptFont and PromptForeColor
        /// </summary>
        private void DrawMultiValuePrompt(Graphics g)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.EndEllipsis| TextFormatFlags.Left;
            Rectangle rect = this.ClientRectangle;
            rect.Offset(1, 1);

            // Draw the prompt text using TextRenderer
            TextRenderer.DrawText(g, MultiValueExtensions.MultiText, Font, rect, MultiValueExtensions.MultiColor, Enabled ? BackColor : Color.Transparent, flags);
        }

        /// <summary>
        /// Focus is entering the control
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            Invalidate();
        }

        /// <summary>
        /// Control is losing focus
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            Invalidate();
        }
    }
}