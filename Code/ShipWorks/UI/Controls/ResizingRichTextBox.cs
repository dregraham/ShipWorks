using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// RichTextBox that mimics a label and will resize itself vertically when the text wraps
    /// </summary>
    public partial class ResizingRichTextBox : UserControl
    {
        private int previousLines;

        /// <summary>
        /// Constructor
        /// </summary>
        public ResizingRichTextBox()
        {
            InitializeComponent();

            // Get the height of a line of text that will be used for sizing the control and hit testing links
            LineHeight = GetLineHeight().Height;

            // Store how many lines of text there are now, which we'll use for resizing
            previousLines = questionsTextBox.GetLineFromCharIndex(questionsTextBox.Text.Length + 1);
            UpdateSize();
        }

        /// <summary>
        /// Gets and sets the text of the control
        /// </summary>
        [Browsable(true)]
        public new string Text
        {
            get
            {
                return InternalTextBox.Text;
            }
            set
            {
                InternalTextBox.Text = value;
                UpdateSize();
            }
        }

        /// <summary>
        /// Defines the height of a single line of text
        /// </summary>
        protected float LineHeight { get; private set; }

        /// <summary>
        /// Gets the internal rich text box used to actually display text
        /// </summary>
        protected RichTextBox InternalTextBox
        {
            get
            {
                return questionsTextBox;
            }
        }

        /// <summary>
        /// Handle resizing of the control
        /// </summary>
        protected virtual void OnResize(object sender, EventArgs e)
        {
            UpdateSize();
        }

        /// <summary>
        /// Update the size of the control
        /// </summary>
        private void UpdateSize()
        {
            int lines = questionsTextBox.GetLineFromCharIndex(questionsTextBox.Text.Length + 1);

            if (lines != previousLines)
            {
                previousLines = lines;

                Height = ((int) LineHeight*(lines + 1)) + Margin.Bottom;
            }
        }

        /// <summary>
        /// Get the height of a line of text
        /// </summary>
        private SizeF GetLineHeight()
        {
            Graphics g = Graphics.FromHwnd(questionsTextBox.Handle);
            SizeF f = g.MeasureString("Sample Text", questionsTextBox.Font);
            return f;
        }

        /// <summary>
        /// RichTextBox that behaves more like a label
        /// </summary>
        private class DisabledCursorRichTextBox : RichTextBox
        {
            /// <summary>
            /// Handle Windows messages
            /// </summary>
            protected override void WndProc(ref Message m)
            {
                // We need to stop cursor changes because it causes flickering when the containing control changes cursors
                // We also need to stop focus so the caret doesn't display
                if (m.Msg == NativeMethods.WM_SETCURSOR || m.Msg == NativeMethods.WM_SETFOCUS)
                {
                    return;
                }

                base.WndProc(ref m);
            }
        }
    }
}
