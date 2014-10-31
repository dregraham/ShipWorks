using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.Win32;

namespace ShipWorks.Shipping.Editing.Rating
{
    public partial class ExceptionsRateFootnoteErrorControl : UserControl
    {
        private Point topLeft;
        private Point bottomRight;

        private bool wasInLink;
        private int learnMoreLocation;
        private int previousLines;
        private float lineHeight;

        private string helpUrl;

        public const string LearnMore = "Learn more...";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionsRateFootnoteErrorControl"/> class.
        /// </summary>
        public ExceptionsRateFootnoteErrorControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the specified error message text.
        /// </summary>
        /// <param name="errorMessageText">The error message text.</param>
        /// <param name="helpLink">The help link.</param>
        public void Initialize(string errorMessageText, string helpLink)
        {
            errorMessage.Text = errorMessageText + " " + LearnMore;
            helpUrl = helpLink;

            // Get the height of a line of text that will be used for sizing the control and hit testing links
            lineHeight = GetLineHeight().Height;

            // Store how many lines of text there are now, which we'll use for resizing
            previousLines = errorMessage.GetLineFromCharIndex(errorMessage.Text.Length + 1);

            learnMoreLocation = errorMessage.Text.IndexOf(LearnMore, StringComparison.OrdinalIgnoreCase);

            FormatHelpLink();
            UpdateLinkLocation();


            Height = errorMessage.Lines.Count() * (int)lineHeight + (int)Math.Round(lineHeight + 1);
        }

        /// <summary>
        /// Color and underline "Learn more..." text.
        /// </summary>
        private void FormatHelpLink()
        {
            errorMessage.Select(learnMoreLocation, LearnMore.Length);
            errorMessage.SelectionFont = new Font(errorMessage.Font, FontStyle.Underline);
            errorMessage.SelectionColor = Color.Blue;
            errorMessage.DeselectAll();
        }

        /// <summary>
        /// Handle resizing of the control
        /// </summary>
        private void OnResize(object sender, EventArgs e)
        {
            UpdateLinkLocation();

            int lines = errorMessage.GetLineFromCharIndex(errorMessage.Text.Length + 1);

            if (lines != previousLines)
            {
                previousLines = lines;

                Height = (int)lineHeight * (lines + 1);
            }
        }

        /// <summary>
        /// Handle clicks to see if the help link has been clicked
        /// </summary>
        private void OnErrorMessageClick(object sender, MouseEventArgs e)
        {
            if (IsInLink(e.Location))
            {
                WebHelper.OpenUrl(helpUrl, this);
            }
        }

        /// <summary>
        /// Handle mouse movement to see if we need to update the cursor
        /// </summary>
        private void OnErrorMessageMouseMove(object sender, MouseEventArgs e)
        {
            bool isInLink = IsInLink(e.Location);

            if (wasInLink ^ isInLink)
            {
                Cursor = isInLink ? Cursors.Hand : Cursors.Default;
                wasInLink = isInLink;
            }
        }

        /// <summary>
        /// Get the height of a line of text
        /// </summary>
        private SizeF GetLineHeight()
        {
            Graphics g = Graphics.FromHwnd(errorMessage.Handle);
            SizeF f = g.MeasureString(errorMessage.Text, errorMessage.Font);
            return f;
        }

        /// <summary>
        /// Update where in the text the email address currently is
        /// </summary>
        private void UpdateLinkLocation()
        {
            topLeft = errorMessage.GetPositionFromCharIndex(learnMoreLocation);
            bottomRight = errorMessage.GetPositionFromCharIndex(learnMoreLocation + LearnMore.Length);
        }

        /// <summary>
        /// Finds whether the point is within the link hitbox
        /// </summary>
        private bool IsInLink(Point point)
        {
            int charIndex = errorMessage.GetCharIndexFromPosition(point);
            return (learnMoreLocation <= charIndex) && (charIndex <= learnMoreLocation + LearnMore.Length) && 
                (point.X <= bottomRight.X && point.X >= topLeft.X) && point.Y >= topLeft.Y && point.Y <= bottomRight.Y + lineHeight;
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
