namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Identical to the base class except the text of signup/converstion control differs.
    /// </summary>
    public partial class SingleAccountMarketingDlg : UspsActivateDiscountDlg
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleAccountMarketingDlg"/> class.
        /// </summary>
        public SingleAccountMarketingDlg()
        {
            // Text of signup control differs from that of the base class (see designer), but
            // all other behavior is the same
            InitializeComponent();
        }
    }
}
