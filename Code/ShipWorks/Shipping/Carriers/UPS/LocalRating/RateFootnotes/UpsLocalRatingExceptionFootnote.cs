using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes
{
    public partial class UpsLocalRatingExceptionFootnote : RateFootnoteControl
    {
        private readonly string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingExceptionFootnote"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public UpsLocalRatingExceptionFootnote(string errorMessage)
        {
            this.errorMessage = errorMessage;
            InitializeComponent();
        }

        /// <summary>
        /// Called when more info link clicked.
        /// </summary>
        private void OnMoreInfoClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageHelper.ShowError(this, errorMessage);
        }
    }
}
