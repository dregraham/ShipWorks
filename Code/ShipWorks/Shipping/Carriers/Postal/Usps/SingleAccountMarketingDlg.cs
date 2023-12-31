﻿using System;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
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

            this.signUpForExpeditedControl.DiscountText = "You can now save up to 54% off Retail prices on USPS Priority Mail and Priority Mail Express Shipments " + 
                                                            "with ShipWorks, all through one single USPS account. " + Environment.NewLine + Environment.NewLine +
                                                            "To get these discounts, you just need to open a free USPS account which will enable you to easily " +
                                                            "print USPS Priority Mail, Priority Mail Express and First Class shipping labels, all within one account.";

            this.convertToExpeditedControl.DescriptionText = "You can now save up to 54% off Retail prices on USPS Priority Mail and Priority Mail Express Shipments " + 
                                                            "with ShipWorks, all through one single USPS account. No more switching between accounts to get the lowest rates!";

        }
    }
}
