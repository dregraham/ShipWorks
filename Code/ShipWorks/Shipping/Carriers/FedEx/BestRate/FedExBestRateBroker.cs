using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.FedEx.BestRate
{
    /// <summary>
    /// An implementation of the IBestRateShippingBroker that Rate broker that 
    /// finds the best rates for FedEx accounts.
    /// </summary>
    public class FedExBestRateBroker : IBestRateShippingBroker
    {
        /// <summary>
        /// Gets the rates for each of the accounts of a specific shipping provider based
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>
        /// A list of RateResults for each account of a specific shipping provider (i.e. if two accounts 
        /// are registered for a single provider, the list of rates would have two entries if both 
        /// accounts returned rates).
        /// </returns>
        public List<RateResult> GetBestRates(ShipmentEntity shipment)
        {
            // We're just stubbing this out for now, so we have another carrier/broker
            // for testing purposes
            return new List<RateResult>
            {
                new RateResult("Stubbed FedEx", "1", 0.01M, FedExServiceType.FirstOvernight)
            };
        }
    }
}
