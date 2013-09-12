namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Express1-specific Service Control.
    /// </summary>
    public partial class Express1ServiceControl : EndiciaServiceControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1ServiceControl()
            : base (ShipmentTypeCode.Express1Endicia, EndiciaReseller.Express1)
        {
            InitializeComponent();
        }
    }
}
