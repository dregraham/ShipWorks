﻿using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Discounted
{
    /// <summary>
    /// Usps discounted rate footnote view model
    /// </summary>
    public class UspsRateDiscountedFootnoteViewModel : IUspsRateDiscountedFootnoteViewModel
    {
        private readonly IWin32Window owner;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"></param>
        public UspsRateDiscountedFootnoteViewModel(IWin32Window owner)
        {
            this.owner = owner;
            ViewSavings = new RelayCommand(ViewSavingsAction);
        }

        /// <summary>
        /// List of discounted rates
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<RateResult> DiscountedRates { get; set; }

        /// <summary>
        /// List or original rates
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<RateResult> OriginalRates { get; set; }

        /// <summary>
        /// View available savings
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ViewSavings { get; }

        /// <summary>
        /// View the detailed Endicia vs. Express1 savings
        /// </summary>
        private void ViewSavingsAction()
        {
            using (UspsActualSavingsDlg dlg = new UspsActualSavingsDlg(OriginalRates, DiscountedRates))
            {
                dlg.ShowDialog(owner);
            }
        }
    }
}
