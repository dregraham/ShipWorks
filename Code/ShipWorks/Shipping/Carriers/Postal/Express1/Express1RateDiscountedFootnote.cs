﻿using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// User control for displaying footnote about the savings obtained by using Express1
    /// </summary>
    public partial class Express1RateDiscountedFootnote : RateFootnoteControl
    {
        List<RateResult> originalRates;
        List<RateResult> discountedRates;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1RateDiscountedFootnote(List<RateResult> originalRates, List<RateResult> discountedRates)
        {
            InitializeComponent();

            this.originalRates = originalRates;
            this.discountedRates = discountedRates;
        }

        /// <summary>
        /// Adds the carrier name text to the text of the control
        /// </summary>
        public override void SetCarrierName(string carrierName)
        {
            AddCarrierNameText(carrierName, label, linkControl);
        }

        /// <summary>
        /// View the detailed Endicia vs. Express1 savings
        /// </summary>
        private void OnLinkViewSavings(object sender, EventArgs e)
        {
            using (Express1ActualSavingsDlg dlg = new Express1ActualSavingsDlg(originalRates, discountedRates))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
