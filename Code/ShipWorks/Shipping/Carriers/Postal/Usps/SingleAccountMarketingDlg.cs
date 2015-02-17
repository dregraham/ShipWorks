using System;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
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

            string express1TargetedText = string.Empty;

            if (EndiciaAccountManager.Express1Accounts.Any() || UspsAccountManager.Express1Accounts.Any())
            {
                express1TargetedText = "No more switching between accounts to get the lowest rates!";
            }

            this.signUpForExpeditedControl.DiscountText = "You can now save up to 46% on USPS Priority Mail and Priority Mail Express Shipments with ShipWorks and " +
                                                          "IntuiShip, all through one single Stamps.com account. " + express1TargetedText 
                                                          + Environment.NewLine + Environment.NewLine + "To get these discounts, you " +
                                                          "just need to open a free Stamps.com account which will enable you to easily print both USPS Priority Mail " +
                                                          "and Priority Mail Express labels and First Class shipping labels, all within one account.";

            this.convertToExpeditedControl.DescriptionText = "You can now save up to 46% on USPS Priority Mail and Priority Mail Express Shipments with ShipWorks and " +
                                                             "IntuiShip, all through one single Stamps.com account. No " +
                                                             "more switching between accounts to get the lowest rates!" + Environment.NewLine + Environment.NewLine + 
                                                             "ShipWorks offers these discounted rates through IntuiShip, a partner of the USPS.";

        }
    }
}
