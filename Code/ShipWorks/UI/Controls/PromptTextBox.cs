using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Interapptive.Shared;
using Interapptive.Shared.Win32;
using System.Reflection;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// A TextBox that displays a prompt message when not focuses and has no text.
    /// </summary>
    public class PromptTextBox : MultiValueTextBox
    {
        Color promptColor = SystemColors.GrayText;
        string promptText;

        /// <summary>
        /// Constructor
        /// </summary>
        public PromptTextBox()
        {

        }

        [Obfuscation(Exclude = true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The prompt text to display when there is nothing in the Text property.")]
        public string PromptText
        {
            get 
            { 
                return promptText; 
            }
            set
            {
                promptText = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The color to use when displaying the PromptText.")]
        public Color PromptColor
        {
            get 
            { 
                return promptColor; 
            }
            set 
            { 
                promptColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Overridden for custom drawing of the prompt
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (!MultiValued && Enabled)
            {
                // Only draw the prompt on the WM_PAINT event and when the Text property is empty
                if (m.Msg == NativeMethods.WM_PAINT && !Focused && Text.Length == 0)
                {
                    using (Graphics g = CreateGraphics())
                    {
                        DrawTextPrompt(g);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the PromptText in the TextBox.ClientRectangle using the PromptFont and PromptForeColor
        /// </summary>
        /// <param name="g">The Graphics region to draw the prompt on</param>
        protected virtual void DrawTextPrompt(Graphics g)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.EndEllipsis;
            Rectangle rect = this.ClientRectangle;

            // Offset the rectangle based on the HorizontalAlignment, 
            // otherwise the display looks a little strange
            switch (this.TextAlign)
            {
                case HorizontalAlignment.Center:
                    flags = flags | TextFormatFlags.HorizontalCenter;
                    rect.Offset(0, 1);
                    break;
                case HorizontalAlignment.Left:
                    flags = flags | TextFormatFlags.Left;
                    rect.Offset(1, 1);
                    break;
                case HorizontalAlignment.Right:
                    flags = flags | TextFormatFlags.Right;
                    rect.Offset(0, 1);
                    break;
            }

            // Draw the prompt text using TextRenderer
            TextRenderer.DrawText(g, promptText, Font, rect, promptColor, this.BackColor, flags);
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
