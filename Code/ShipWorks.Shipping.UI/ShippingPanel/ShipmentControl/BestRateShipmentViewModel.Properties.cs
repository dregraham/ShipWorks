using System.Collections.Generic;
using System.Reflection;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by BestRateShipmentControl
    /// </summary>
    public partial class BestRateShipmentViewModel
    {
        private int serviceLevel;
        private bool ratesLoaded;
        private RateResult selectedRate;
        private static SortedList<int, string> serviceLevels = new SortedList<int, string>();
        
        /// <summary>
        /// Service level used
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int ServiceLevel
        {
            get { return serviceLevel; }
            set { handler.Set(nameof(ServiceLevel), ref serviceLevel, value); }
        }

        /// <summary>
        /// Service levels
        /// </summary>
        [Obfuscation(Exclude = true)]
        public SortedList<int, string> ServiceLevelTypes
        {
            get
            {
                return serviceLevels;
            }
        }

        /// <summary>
        /// Gets the currently selected rate
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override RateResult SelectedRate
        {
            get { return selectedRate; }
            set { handler.Set(nameof(SelectedRate), ref selectedRate, value, true); }
        }

        /// <summary>
        /// Are rates still retrieving?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool RatesLoaded
        {
            get { return ratesLoaded; }
            set { handler.Set(nameof(RatesLoaded), ref ratesLoaded, value, true); }
        }
    }
}
