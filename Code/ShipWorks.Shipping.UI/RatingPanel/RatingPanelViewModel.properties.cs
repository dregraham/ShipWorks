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
        private bool showFootnotes;
        private IEnumerable<RateResultDisplay> rates;
        private IEnumerable<object> footnotes;

        /// <summary>
        /// List of rates that should be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<RateResultDisplay> Rates
        {
            get { return rates; }
            set { handler.Set(nameof(Rates), ref rates, value); }
        }

        /// <summary>
        /// Should the shipping column be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowShipping
        {
            get { return showShipping; }
            set { handler.Set(nameof(ShowShipping), ref showShipping, value); }
        }

        /// <summary>
        /// Should the taxes column be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowTaxes
        {
            get { return showTaxes; }
            set { handler.Set(nameof(ShowTaxes), ref showTaxes, value); }
        }

        /// <summary>
        /// Should the duties column be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowDuties
        {
            get { return showDuties; }
            set { handler.Set(nameof(ShowDuties), ref showDuties, value); }
        }

        /// <summary>
        /// Should footnotes be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowFootnotes
        {
            get { return showDuties; }
            set { handler.Set(nameof(ShowFootnotes), ref showFootnotes, value); }
        }

        /// <summary>
        /// List of footnotes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<RateResultDisplay> Footnotes
        {
            get { return rates; }
            set { handler.Set(nameof(Footnotes), ref footnotes, value); }
        }
    }
}
