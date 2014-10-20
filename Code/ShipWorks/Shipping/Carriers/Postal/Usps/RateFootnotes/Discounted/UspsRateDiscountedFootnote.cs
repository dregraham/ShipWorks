using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Discounted
{
    public partial class UspsRateDiscountedFootnote : RateFootnoteControl
    {
        private readonly List<RateResult>  originalRates;
        private readonly List<RateResult> discountedRates;

        public UspsRateDiscountedFootnote(List<RateResult> originalRates, List<RateResult> discountedRates)
        {
            InitializeComponent();
            this.originalRates = originalRates;
            this.discountedRates = discountedRates;
        }

        /// <summary>
        /// View the detailed Endicia vs. Express1 savings
        /// </summary>
        private void OnLinkViewSavings(object sender, EventArgs e)
        {
            using (UspsActualSavingsDlg dlg = new UspsActualSavingsDlg(originalRates, discountedRates))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Associated with check mark.
        /// </summary>
        public override bool AssociatedWithAmountFooter
        {
            get { return true; }
        }
    }
}
