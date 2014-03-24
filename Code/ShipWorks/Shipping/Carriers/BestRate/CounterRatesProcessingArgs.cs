using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Event arguments for the counter rates processing event
    /// </summary>
    public class CounterRatesProcessingArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="allRates">A list of all available rates</param>
        /// <param name="filteredRates">A list of rates that have been filtered as they would exist in the main rates grid</param>
        /// <param name="setupShipmentType">The ShipmentType that the process click determined to use.  This will be the used to get the setup wizard.</param>
        /// <param name="shipment">The shipment for this counter rate.</param>
        public CounterRatesProcessingArgs(RateGroup allRates, RateGroup filteredRates, ShipmentEntity shipment)
        {
            AllRates = allRates;
            FilteredRates = filteredRates;
            Shipment = shipment;
        }

        /// <summary>
        /// Gets all the rates that can be applied to the shipment
        /// </summary>
        public RateGroup AllRates
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the filtered list of rates
        /// </summary>
        public RateGroup FilteredRates
        {
            get; 
            private set;
        }
        
        /// <summary>
        /// The ShipmentType that was selected from the wizard. This will be used by preprocessed once the wizard is closed.
        /// </summary>
        public ShipmentType SelectedShipmentType
        {
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the selected rate.
        /// </summary>
        public RateResult SelectedRate
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets the shipment that the rates are for.
        /// </summary>
        public ShipmentEntity Shipment
        {
            get; 
            private set;
        }
    }
}