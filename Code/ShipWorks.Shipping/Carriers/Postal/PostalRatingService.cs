using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// PostalRatingService
    /// </summary>
    public abstract class PostalRatingService : IRatingService
    {
        protected readonly IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeManager;
        private readonly IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostalRatingService"/> class.
        /// </summary>
        protected PostalRatingService(IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory, IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeManager)
        {
            this.shipmentTypeManager = shipmentTypeManager;
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
                RateGroup errorRates = new RateGroup(Enumerable.Empty<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(shipment.ShipmentTypeCode));
                return errorRates;
            }

            RateGroup rates = new RateGroup(Enumerable.Empty<RateResult>());

            if (!shipmentTypeManager[shipment.ShipmentTypeCode].IsShipmentTypeRestricted)
            {
                // Only get counter rates if the shipment type has not been restricted
                rates = ratingServiceFactory[ShipmentTypeCode.PostalWebTools].GetRates(shipment);

                foreach (RateResult rate in rates.Rates.Where(rate => rate.ProviderLogo != null))
                {
                    rate.ProviderLogo = EnumHelper.GetImage(shipment.ShipmentTypeCode);
                }
            }

            return rates;
        }

        /// <summary>
        /// Gets the filtered rates based on any excluded services configured for this postal shipment type.
        /// </summary>
        protected virtual List<RateResult> FilterRatesByExcludedServices(ShipmentEntity shipment, List<RateResult> rates)
        {
            IEnumerable<PostalServiceType> availableServiceTypes1 = shipmentTypeManager[shipment.ShipmentTypeCode]
                    .GetAvailableServiceTypes()
                    .Cast<PostalServiceType>();
            IEnumerable<PostalServiceType> unionWithSelected = availableServiceTypes1.Union(new List<PostalServiceType> {(PostalServiceType) shipment.Postal.Service});
            List<RateResult> results = rates.Where(r => r.Tag is PostalRateSelection && unionWithSelected.Contains(((PostalRateSelection)r.Tag).ServiceType)).ToList();

            return results;
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
                .Where(e => availabelServiceTypes.Contains(((PostalRateSelection) e.OriginalTag).ServiceType))
                .ToList();

            validExpress1Rates.ForEach(e =>
            {
                e.ShipmentType = baseShipmentType;
                e.ProviderLogo = e.ProviderLogo != null ? EnumHelper.GetImage(express1ShipmentType) : null;
            });

            return new RateGroup(validExpress1Rates);
        }
    }
}