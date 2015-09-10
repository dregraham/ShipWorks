using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using System.Drawing;
using System.ComponentModel;
using Interapptive.Shared.Win32;
using System.Reflection;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// DateTimePicker control that allows for display "Multiple Value"
    /// </summary>
    public class MultiValueDateTimePicker : DateTimePicker
    {
        bool isMultiValued = false;

        /// <summary>
        /// Get \ set whether the box represents a null value.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Obfuscation(Exclude = true)]
        public bool MultiValued
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
                    Value = DateTime.Now;
                }

                isMultiValued = value;
                Invalidate();
            }
        }

        /// <summary>
        /// When the value changes, we are not multi value anymore
        /// </summary>
        protected override void OnValueChanged(EventArgs e)
        {
            base.OnValueChanged(e);

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
                    DrawTextPrompt(g);
                }
            }
        }

        /// <summary>
        /// Draws the PromptText in the TextBox.ClientRectangle using the PromptFont and PromptForeColor
        /// </summary>
        protected virtual void DrawTextPrompt(Graphics g)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.EndEllipsis | TextFormatFlags.Left;
            Rectangle rect = this.ClientRectangle;
            rect.Offset(3, 3);

            // Have to cover up all the date stuff
            Rectangle backRect = this.ClientRectangle;
            
            // Make room for borders, and the button to pick the date time
            backRect.Offset(2, 2);
            backRect.Height -= 4;
            backRect.Width -= 40;

            using (SolidBrush brush = new SolidBrush(BackColor))
            {
                g.FillRectangle(brush, backRect);
            }

            // Draw the prompt text using TextRenderer
            TextRenderer.DrawText(g, MultiValueExtensions.MultiText, Font, rect, MultiValueExtensions.MultiColor, Color.Transparent, flags);
        }
    }
}
