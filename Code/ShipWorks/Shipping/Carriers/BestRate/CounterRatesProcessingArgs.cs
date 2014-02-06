using System;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Event arguments for the counter rates processing event
    /// </summary>
    public class CounterRatesProcessingArgs
    {
        /// <summary>
        /// Constructur
        /// </summary>
        /// <param name="allRates">A list of all available rates</param>
        /// <param name="filteredRates">A list of rates that have been filtered as they would exist in the main rates grid</param>
        /// <param name="setupShipmentType">The ShipmentType that the process click determined to use.  This will be the used to get the setup wizard.</param>
        /// <param name="shipmentID">The shipment ID for this counter rate.</param>
        public CounterRatesProcessingArgs(RateGroup allRates, RateGroup filteredRates, ShipmentType setupShipmentType, long shipmentID)
        {
            AllRates = allRates;
            FilteredRates = filteredRates;
            SetupShipmentType = setupShipmentType;
            ShipmentID = shipmentID;
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
        /// The ShipmentType that the process click determined to use.  This will be the used to get the setup wizard.
        /// </summary>
        public ShipmentType SetupShipmentType
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
        /// Gets the rate that was selected by the event
        /// </summary>
        public RateResult SelectedRate
        {
            get; 
            set;
        }

        public long ShipmentID
        {
            get; 
            private set;
        }
    }
}