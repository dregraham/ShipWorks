using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared.Win32;

namespace ShipWorks.Shipping.Insurance
{
    public partial class InsureShipQuestionsControl : UserControl
    {
        private Point topLeft;
        private Point bottomRight;

        private bool wasInLink;
        private readonly int emailAddressLocation;

        public const string EmailAddress = "claims@insureship.com";

        public InsureShipQuestionsControl()
        {
            InitializeComponent();

            questionsTextBox.Find(EmailAddress, RichTextBoxFinds.MatchCase);

            emailAddressLocation = questionsTextBox.SelectionStart;

            questionsTextBox.SelectionFont = new Font(questionsTextBox.Font, FontStyle.Underline);
            questionsTextBox.SelectionColor = Color.Blue;
            questionsTextBox.DeselectAll();

            UpdateLinkLocation();

            questionsTextBox.MouseMove += OnQuestionsTextBoxMouseMove;
            questionsTextBox.ContentsResized += (sender, args) => UpdateLinkLocation();
            questionsTextBox.Resize += (sender, args) => UpdateLinkLocation();
            questionsTextBox.MouseUp += OnQuestionsTextBoxClick;
        }

        private void OnQuestionsTextBoxClick(object sender, MouseEventArgs e)
        {
            if (IsInLink(e.Location))
            {
                Process.Start("mailto:" + EmailAddress);
            }
        }

        private void UpdateLinkLocation()
        {
            topLeft = questionsTextBox.GetPositionFromCharIndex(emailAddressLocation);
            bottomRight = questionsTextBox.GetPositionFromCharIndex(emailAddressLocation + EmailAddress.Length);
        }

        private void OnQuestionsTextBoxMouseMove(object sender, MouseEventArgs e)
        {
            bool isInLink = IsInLink(e.Location);

            if (wasInLink ^ isInLink)
            {
                Cursor = isInLink ? Cursors.Hand : Cursors.Default;
                wasInLink = isInLink;
            }
        }

        private bool IsInLink(Point e)
        {
            return e.X > topLeft.X && e.X < bottomRight.X && e.Y > topLeft.Y && e.Y < bottomRight.Y + 16;
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
