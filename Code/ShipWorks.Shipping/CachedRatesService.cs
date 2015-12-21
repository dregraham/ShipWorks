using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// CachedRatesService
    /// </summary>
    public class CachedRatesService : ICachedRatesService
    {
        private readonly Func<ShipmentTypeCode, IRateHashingService> rateHashingServiceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRatesService"/> class.
        /// </summary>
        public CachedRatesService(Func<ShipmentTypeCode, IRateHashingService> rateHashingServiceFactory)
        {
            this.rateHashingServiceFactory = rateHashingServiceFactory;
        }

        /// <summary>
        /// Gets rates, retrieving them from the cache if possible
        /// </summary>
        /// <typeparam name="T">Type of exception that the carrier will throw on an error</typeparam>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        /// <param name="getRatesFunction">Function to retrieve the rates from the carrier if not in the cache</param>
        /// <returns></returns>
        public RateGroup GetCachedRates<T>(ShipmentEntity shipment, Func<ShipmentEntity, RateGroup> getRatesFunction) where T : Exception
        {
            string rateHash = rateHashingServiceFactory((ShipmentTypeCode)shipment.ShipmentType).GetRatingHash(shipment);

            if (RateCache.Instance.Contains(rateHash))
            {
                RateGroup rateGroup = RateCache.Instance.GetRateGroup(rateHash);

                // If we are getting a cached amazon rate send the AmazonRatesRetrievedMessage
                // to update the Amazon shipping service control
                if ((ShipmentTypeCode)shipment.ShipmentType == ShipmentTypeCode.Amazon)
                {
                    Messenger.Current.Send(new AmazonRatesRetrievedMessage(this, rateGroup));
                }

                return rateGroup;
            }

            try
            {
                RateGroup rateGroup = getRatesFunction(shipment);
                RateCache.Instance.Save(rateHash, rateGroup);

                return rateGroup;
            }
            catch (T ex)
            {
                // This is a bad configuration on some level, so cache an empty rate group
                // before throwing throwing the exceptions
                RateGroup invalidRateGroup = CacheInvalidRateGroup(shipment, ex);
                throw new InvalidRateGroupShippingException(invalidRateGroup, ex.Message, ex);
            }
        }

        /// <summary>
        /// This is intended to be used when there is (most likely) a bad configuration
        /// with the shipment on some level, so an empty rate group with a exception footer
        /// is cached.
        /// </summary>
        /// <param name="shipment">The shipment that generated the given exception.</param>
        /// <param name="exception">The exception</param>
        public RateGroup CacheInvalidRateGroup(ShipmentEntity shipment, Exception exception)
        {
            RateGroup rateGroup = new InvalidRateGroup((ShipmentTypeCode) shipment.ShipmentType, exception);

            RateCache.Instance.Save(rateHashingServiceFactory((ShipmentTypeCode)shipment.ShipmentType).GetRatingHash(shipment), rateGroup);

            return rateGroup;
        }
    }
}
