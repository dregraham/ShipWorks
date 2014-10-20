using System;
using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Displays information about InsureShip insurance
    /// </summary>
    public class InsureShipQuestionsControl : ResizingRichTextBox
    {
        private Point topLeft;
        private Point bottomRight;

        private bool wasInLink;
        private int emailAddressLocation;

        public const string EmailAddress = "claims@insureship.com";

        /// <summary>
        /// Constructors
        /// </summary>
        public InsureShipQuestionsControl()
        {
            Text = "Thanks for using ShipWorks insurance! ShipWorks Insurance is managed by InsureShip. If you have any questions about your claim, please contact InsureShip directly at claims@insureship.com or call (866) 701-3654.";
            ParseTextForLink();

            InternalTextBox.MouseUp += OnInternalTextBoxMouseUp;
            InternalTextBox.MouseMove += OnQuestionsTextBoxMouseMove;
            InternalTextBox.TextChanged += OnInternalTextBoxTextChanged;
        }

        /// <summary>
        /// Find the link in the text
        /// </summary>
        private void ParseTextForLink()
        {
            emailAddressLocation = InternalTextBox.Text.IndexOf(EmailAddress, StringComparison.OrdinalIgnoreCase);

            FormatEmailAddresses();
            UpdateLinkLocation();
        }

        /// <summary>
        /// Handle text changes in the control
        /// </summary>
        private void OnInternalTextBoxTextChanged(object sender, EventArgs eventArgs)
        {
            ParseTextForLink();
        }

        /// <summary>
        /// Handle control resizes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            UpdateLinkLocation();

            base.OnResize(e);
        }

        /// <summary>
        /// Handle clicks to see if the email address has been clicked
        /// </summary>
        private void OnInternalTextBoxMouseUp(object sender, MouseEventArgs e)
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
        /// Color and underline the email address
        /// </summary>
        private void FormatEmailAddresses()
        {
            InternalTextBox.Select(emailAddressLocation, EmailAddress.Length);
            InternalTextBox.SelectionFont = new Font(InternalTextBox.Font, FontStyle.Underline);
            InternalTextBox.SelectionColor = Color.Blue;
            InternalTextBox.DeselectAll();
        }

        /// <summary>
        /// Update where in the text the email address currently is
        /// </summary>
        private void UpdateLinkLocation()
        {
            topLeft = InternalTextBox.GetPositionFromCharIndex(emailAddressLocation);
            bottomRight = InternalTextBox.GetPositionFromCharIndex(emailAddressLocation + EmailAddress.Length);
        }

        /// <summary>
        /// Finds whether the point is within the link hitbox
        /// </summary>
        private bool IsInLink(Point point)
        {
            return point.X > topLeft.X && point.X < bottomRight.X &&
                   point.Y > topLeft.Y && point.Y < bottomRight.Y + LineHeight;
        }
    }
}