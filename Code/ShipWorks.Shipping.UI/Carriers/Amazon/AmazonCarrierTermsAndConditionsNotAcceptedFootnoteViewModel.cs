﻿using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// View model for the AmazonCarrierTermsAndConditionsNotAccepted footnote
    /// </summary>
    public class AmazonCarrierTermsAndConditionsNotAcceptedFootnoteViewModel :
        IAmazonCarrierTermsAndConditionsNotAcceptedFootnoteViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCarrierTermsAndConditionsNotAcceptedFootnoteViewModel()
        {
            ShowDialog = new RelayCommand(ShowDialogAction);
        }

        /// <summary>
        /// Names of the carriers to show in the dialog box
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<string> CarrierNames { get; set; }

        /// <summary>
        /// Command to show the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ShowDialog { get; }

        /// <summary>
        /// Show the given dialog
        /// </summary>
        private void ShowDialogAction()
        {
            using (AmazonCarrierTermsAndConditionsNotAcceptedDialog dialog = new AmazonCarrierTermsAndConditionsNotAcceptedDialog(CarrierNames))
            {
                dialog.ShowDialog();
            }
        }
    }
}