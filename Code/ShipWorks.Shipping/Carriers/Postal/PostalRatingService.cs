using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// PostalRatingService
    /// </summary>
    public abstract class PostalRatingService : IRatingService
    {
        protected readonly IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory;
        private readonly IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostalRatingService"/> class.
        /// </summary>
        protected PostalRatingService(IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory, IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory)
        {
            this.shipmentTypeFactory = shipmentTypeFactory;
            this.ratingServiceFactory = ratingServiceFactory;
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public abstract RateGroup GetRates(ShipmentEntity shipment);

        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected virtual RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            try
            {
                CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(shipmentTypeFactory[(ShipmentTypeCode)shipment.ShipmentType]));
                return errorRates;
            }

            RateGroup rates = new RateGroup(new List<RateResult>());

            if (!shipmentTypeFactory[(ShipmentTypeCode)shipment.ShipmentType].IsShipmentTypeRestricted)
            {
                // Only get counter rates if the shipment type has not been restricted
                rates = ratingServiceFactory[ShipmentTypeCode.PostalWebTools].GetRates(shipment);
                rates.Rates.ForEach(x =>
                {
                    if (x.ProviderLogo != null)
                    {
                        // Only change existing logos; don't set logos for rates that don't have them
                        x.ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode)shipment.ShipmentType);
                    }
                });
            }

            return rates;
        }

        /// <summary>
        /// Gets the filtered rates based on any excluded services configured for this postal shipment type.
        /// </summary>
        protected virtual List<RateResult> FilterRatesByExcludedServices(ShipmentEntity shipment, List<RateResult> rates)
        {
            List<PostalServiceType> availableServiceTypes = shipmentTypeFactory[(ShipmentTypeCode)shipment.ShipmentType].GetAvailableServiceTypes().Select(s => (PostalServiceType)s).Union(new List<PostalServiceType> { (PostalServiceType)shipment.Postal.Service }).ToList();
            return rates.Where(r => r.Tag is PostalRateSelection && availableServiceTypes.Contains(((PostalRateSelection)r.Tag).ServiceType)).ToList();
        }

        /// <summary>
        /// Builds a RateGroup from a list of express 1 rates
        /// </summary>
        /// <param name="rates">List of rates that should be filtered and added to the group</param>
        /// <param name="express1ShipmentType">Express1 shipment type</param>
        /// <param name="baseShipmentType">Base type of the shipment</param>
        /// <returns></returns>
        protected RateGroup BuildExpress1RateGroup(IEnumerable<RateResult> rates, ShipmentTypeCode express1ShipmentType, ShipmentTypeCode baseShipmentType)
        {
            // Express1 rates - return rates filtered by what is available to the user
            List<PostalServiceType> availabelServiceTypes =
                PostalUtility.GetDomesticServices(express1ShipmentType)
                    .Concat(PostalUtility.GetInternationalServices(express1ShipmentType))
                    .ToList();

            var validExpress1Rates = rates
                .Where(e => availabelServiceTypes.Contains(((PostalRateSelection)e.OriginalTag).ServiceType))
                .ToList();

            validExpress1Rates.ForEach(e => {
                e.ShipmentType = baseShipmentType;
                e.ProviderLogo = e.ProviderLogo != null ? EnumHelper.GetImage(express1ShipmentType) : null;
            });

            return new RateGroup(validExpress1Rates);
        }
    }
}