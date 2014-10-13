using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.Win32;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Displays information about InsureShip insurance
    /// </summary>
    public partial class InsureShipQuestionsControl : UserControl
    {
        private Point topLeft;
        private Point bottomRight;

        private bool wasInLink;
        private readonly int emailAddressLocation;
        private int previousLines;
        private readonly float lineHeight;

        public const string EmailAddress = "claims@insureship.com";

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipQuestionsControl()
        {
            InitializeComponent();

            // Get the height of a line of text that will be used for sizing the control and hit testing links
            lineHeight = GetLineHeight().Height;

            // Store how many lines of text there are now, which we'll use for resizing
            previousLines = questionsTextBox.GetLineFromCharIndex(questionsTextBox.Text.Length + 1);

            emailAddressLocation = questionsTextBox.Text.IndexOf(EmailAddress, StringComparison.OrdinalIgnoreCase);

            FormatEmailAddresses();
            UpdateLinkLocation();
        }

        /// <summary>
        /// Color and underline the email address
        /// </summary>
        private void FormatEmailAddresses()
        {
            questionsTextBox.Select(emailAddressLocation, EmailAddress.Length);
            questionsTextBox.SelectionFont = new Font(questionsTextBox.Font, FontStyle.Underline);
            questionsTextBox.SelectionColor = Color.Blue;
            questionsTextBox.DeselectAll();
        }

        /// <summary>
        /// Handle resizing of the control
        /// </summary>
        private void OnResize(object sender, EventArgs e)
        {
            UpdateLinkLocation();

            int lines = questionsTextBox.GetLineFromCharIndex(questionsTextBox.Text.Length + 1);

            if (lines != previousLines)
            {
                previousLines = lines;

                Height = (int)lineHeight * (lines + 1);
            }
        }

        /// <summary>
        /// Handle clicks to see if the email address has been clicked
        /// </summary>
        private void OnQuestionsTextBoxClick(object sender, MouseEventArgs e)
        {
            if (IsInLink(e.Location))
            {
                WebHelper.OpenMailTo(EmailAddress, this);
            }
        }

        /// <summary>
        /// Handle mouse movement to see if we need to update the cursor
        /// </summary>
        private void OnQuestionsTextBoxMouseMove(object sender, MouseEventArgs e)
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
            Graphics g = Graphics.FromHwnd(questionsTextBox.Handle);
            SizeF f = g.MeasureString(questionsTextBox.Text, questionsTextBox.Font);
            return f;
        }

        /// <summary>
        /// Update where in the text the email address currently is
        /// </summary>
        private void UpdateLinkLocation()
        {
            topLeft = questionsTextBox.GetPositionFromCharIndex(emailAddressLocation);
            bottomRight = questionsTextBox.GetPositionFromCharIndex(emailAddressLocation + EmailAddress.Length);
        }

        /// <summary>
        /// Finds whether the point is within the link hitbox
        /// </summary>
        private bool IsInLink(Point point)
        {
            return point.X > topLeft.X && point.X < bottomRight.X && 
                point.Y > topLeft.Y && point.Y < bottomRight.Y + lineHeight;
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
