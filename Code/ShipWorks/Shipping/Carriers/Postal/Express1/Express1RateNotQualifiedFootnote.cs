using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// UserControl for letting the user know their current packging options didn't qualify for an Express1 rate
    /// </summary>
    public partial class Express1RateNotQualifiedFootnote : RateFootnoteControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1RateNotQualifiedFootnote()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adds the carrier name text to the text of the control
        /// </summary>
        public override void SetCarrierName(string carrierName)
        {
            AddCarrierNameText(carrierName, label, null);
        }
    }
}
