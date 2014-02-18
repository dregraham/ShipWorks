using ShipWorks.Shipping.Editing;
namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    public partial class Express1StampsServiceControl : StampsServiceControl
    {
        /// <summary>
        /// Initializes a new <see cref="Express1StampsServiceControl"/> instance.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public Express1StampsServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.Express1Stamps, true, rateControl)
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
