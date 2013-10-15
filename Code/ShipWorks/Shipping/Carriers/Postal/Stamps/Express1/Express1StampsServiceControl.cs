

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
        }
    }
}
