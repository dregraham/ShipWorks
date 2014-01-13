namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    public partial class Express1StampsServiceControl : StampsServiceControl
    {
        /// <summary>
        /// Initializes a new <see cref="Express1StampsServiceControl"/> instance.
        /// </summary>
        public Express1StampsServiceControl()
            : base(ShipmentTypeCode.Express1Stamps, true)
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
