using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for Cached Rates
    /// </summary>
    public interface ICachedRatesService
    {
        /// <summary>
        /// Gets rates, retrieving them from the cache if possible
        /// </summary>
        /// <typeparam name="T">Type of exception that the carrier will throw on an error</typeparam>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        /// <param name="getRatesFunction">Function to retrieve the rates from the carrier if not in the cache</param>
        /// <returns></returns>
        RateGroup GetCachedRates<T>(ShipmentEntity shipment, Func<ShipmentEntity, RateGroup> getRatesFunction) where T : Exception;

        /// <summary>
        /// This is intended to be used when there is (most likely) a bad configuration
        /// with the shipment on some level, so an empty rate group with a exception footer
        /// is cached.
        /// </summary>
        /// <param name="shipment">The shipment that generated the given exception.</param>
        /// <param name="exception">The exception</param>
        RateGroup CacheInvalidRateGroup(ShipmentEntity shipment, Exception exception);
    }
}