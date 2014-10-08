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

            this.signUpForExpeditedControl.DiscountText = "You can now get your discounted postage rates along with address verification " +
                                                          "through a single account. No more switching between accounts!  There are no additional " +
                                                          "monthly fees and the service, tracking, and labels are exactly the same.";

            this.convertToExpeditedControl.DescriptionText = "You can now get your discounted postage rates along with address verification through a" +
                                                             " single account. No more switching between accounts! There are no additional monthly fees " +
                                                             "and the service, tracking, and labels are exactly the same.";

        }
    }
}
