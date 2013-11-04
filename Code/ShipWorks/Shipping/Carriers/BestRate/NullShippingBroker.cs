using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// An implementation of the IBestRateShippingBroker interface that is an application of the 
    /// null object pattern. This is intended to be used for those shipping providers where 
    /// best rate is not applicable and/or not yet supported. 
    /// </summary>
    public class NullShippingBroker : IBestRateShippingBroker
    {
        /// <summary>
        /// Gets the single best rate for each of the accounts of a specific shipping provider based
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An empty list of RateResult objects.</returns>
        public List<RateResult> GetBestRates(ShipmentEntity shipment)
        {
            return new List<RateResult>();
        }
    }
}
