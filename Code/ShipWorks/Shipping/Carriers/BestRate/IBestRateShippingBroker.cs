using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// An interface intended to provide a layer of abstraction between the best rate shipment type
    /// and the actual carriers. Implementations of this interface will handle the details for taking 
    /// the criteria of a best rate shipment and building the corresponding carrier specific
    /// shipment. The broker nomenclature stems from a ticket broker metaphor where you would specify
    /// the general parameters of what you need, and the broker will work on your behalf to find the
    /// best tickets given that criteria. Tying the metaphor back to shipping with the best rate, the
    /// criteria would be things like dimensions, weight, etc.
    /// </summary>
    public interface IBestRateShippingBroker
    {
        /// <summary>
        /// Gets the rates for each of the accounts of a specific shipping provider based 
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="exceptionHandler"></param>
        /// <returns>A list of RateResults for each account of a specific shipping provider (i.e. if 
        /// two accounts are registered for a single provider, the list of rates would have two entries
        /// if both accounts returned rates).</returns>
        List<RateResult> GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler);

        /// <summary>
        /// Gets a value indicating whether there any accounts available to a broker.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the broker [has accounts]; otherwise, <c>false</c>.
        /// </value>
        bool HasAccounts { get; }
    }
}
