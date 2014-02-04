using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Event arguments for the counter rates processing event
    /// </summary>
    public class CounterRatesProcessingEventArgs : EventArgs
    {
        /// <summary>
        /// Constructur
        /// </summary>
        /// <param name="allRates">A list of all available rates</param>
        /// <param name="filteredRates">A list of rates that have been filtered as they would exist in the main rates grid</param>
        public CounterRatesProcessingEventArgs(List<RateResult> allRates, List<RateResult> filteredRates)
        {
            this.AllRates = allRates;
            this.FilteredRates = filteredRates;
        }

        /// <summary>
        /// Gets all the rates that can be applied to the shipment
        /// </summary>
        public List<RateResult> AllRates
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the filtered list of rates
        /// </summary>
        public List<RateResult> FilteredRates
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the rate that was selected by the event
        /// </summary>
        public RateResult SelectedRate
        {
            get; 
            set;
        }
    }
}