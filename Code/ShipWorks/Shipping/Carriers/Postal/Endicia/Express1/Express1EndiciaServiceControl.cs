using ShipWorks.Shipping.Editing;
namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Express1-specific Service Control.
    /// </summary>
    public partial class Express1EndiciaServiceControl : EndiciaServiceControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1EndiciaServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public Express1EndiciaServiceControl(RateControl rateControl)
            : base (ShipmentTypeCode.Express1Endicia, EndiciaReseller.Express1, rateControl)
        {
            InitializeComponent();
        }
    }
}
