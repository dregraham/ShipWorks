using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Identical to the base class except the text of signup control differs.
    /// </summary>
    public partial class SingleAccountMarketingDlg : UspsActivateDiscountDlg
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleAccountMarketingDlg"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public SingleAccountMarketingDlg(ShipmentEntity shipment)
            :base (shipment)
        {
            // Text of signup control differs from that of the base class (see designer), but
            // all other behavior is the same
            InitializeComponent();
        }
    }
}
