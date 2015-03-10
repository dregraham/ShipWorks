using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.NotQualified
{
    /// <summary>
    /// UserControl for letting the user know their current packaging options didn't qualify for a USPS rate
    /// </summary>
    public partial class UspsRateNotQualifiedFootnote : RateFootnoteControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRateNotQualifiedFootnote"/> class.
        /// </summary>
        public UspsRateNotQualifiedFootnote()
        {
            InitializeComponent();
        }
    }
}
