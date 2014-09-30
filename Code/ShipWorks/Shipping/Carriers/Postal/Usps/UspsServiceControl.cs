using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public partial class UspsServiceControl : StampsServiceControl
    {
        /// <summary>
        /// Initializes a new <see cref="UspsServiceControl"/> instance.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public UspsServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.Usps, StampsResellerType.StampsExpedited, rateControl)
        {
            InitializeComponent();

            // Express1 for Stamps.com required that postage is hidden, so we want
            // to hide this option and adjust the insurance control accordingly.
            hidePostalLabel.Visible = false;
            hidePostage.Visible = false;

            insuranceControl.Top = hidePostage.Top;
        }
    }
}
