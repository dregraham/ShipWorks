using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Editing.Rating
{
    public partial class InvalidPackageDimensionsRateFootnoteControl : RateFootnoteControl
    {
        private readonly string detailMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPackageDimensionsRateFootnoteControl"/> class.
        /// </summary>
        /// <param name="detailMessage">The message to display when the "More info" link is clicked.</param>
        public InvalidPackageDimensionsRateFootnoteControl(string detailMessage)
        {
            this.detailMessage = detailMessage;

            InitializeComponent();
        }

        /// <summary>
        /// Handles the <see cref="E:ClickExceptionsLink" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void OnClickExceptionsLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageHelper.ShowInformation(this, detailMessage);
        }
    }
}
