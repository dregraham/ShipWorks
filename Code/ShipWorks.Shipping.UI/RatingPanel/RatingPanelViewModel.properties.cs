using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Shipping.UI.RatingPanel
{
    /// <summary>
    /// Properties for the rating panel view model
    /// </summary>
    public partial class RatingPanelViewModel
    {
        private bool showShipping;
        private bool showTaxes;
        private bool showDuties;
        private IEnumerable<RateResultDisplay> rates;

        /// <summary>
        /// List of rates that should be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<RateResultDisplay> Rates
        {
            get { return rates; }
            set { handler.Set(nameof(Rates), ref rates, value); }
        }

        [Obfuscation(Exclude = true)]
        public bool ShowShipping
        {
            get { return showShipping; }
            set { handler.Set(nameof(ShowShipping), ref showShipping, value); }
        }

        [Obfuscation(Exclude = true)]
        public bool ShowTaxes
        {
            get { return showTaxes; }
            set { handler.Set(nameof(ShowTaxes), ref showTaxes, value); }
        }

        [Obfuscation(Exclude = true)]
        public bool ShowDuties
        {
            get { return showDuties; }
            set { handler.Set(nameof(ShowDuties), ref showDuties, value); }
        }
    }
}
