using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Calculate local rates
    /// </summary>
    [KeyedComponent(typeof(IUpsRateClient), UpsRatingMethod.Local)]
    public class UpsLocalRateClient : IUpsRateClient
    {
        private readonly IApiLogEntry apiLog;
        private readonly IUpsLocalRateTableRepository rateRepository;
        private readonly IEnumerable<IServiceFilter> serviceFilters;
        private readonly IUpsSurchargeFactory surchargeFactory;
        private readonly IIndex<UpsRatingMethod, IUpsRateClient> upsRateClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateClient"/> class.
        /// </summary>
        public UpsLocalRateClient(IEnumerable<IServiceFilter> serviceFilters,
            IUpsLocalRateTableRepository rateRepository,
            IUpsSurchargeFactory surchargeFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
            IIndex<UpsRatingMethod, IUpsRateClient> upsRateClientFactory)
        {
            this.upsRateClientFactory = upsRateClientFactory;
            this.serviceFilters = serviceFilters;
            this.rateRepository = rateRepository;
            this.surchargeFactory = surchargeFactory;
            apiLog = apiLogEntryFactory(ApiLogSource.UpsLocalRating, "Rate");
        }


        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        public GenericResult<List<UpsServiceRate>> GetRates(ShipmentEntity shipment)
        {
            List<UpsServiceRate> localRates = GetRatesInternal(shipment);

            return localRates.Any() ? 
                GenericResult.FromSuccess(localRates) : 
                upsRateClientFactory[UpsRatingMethod.Api].GetRates(shipment);
        }

        /// <summary>
        /// Gets rates for the given <paramref name="shipment" />
        /// </summary>
        private List<UpsServiceRate> GetRatesInternal(ShipmentEntity shipment)
        {
            try
            {
                IEnumerable<UpsServiceType> serviceTypes = GetEligibleServices(shipment);

                IEnumerable<UpsLocalServiceRate> upsLocalServiceRates =
                    rateRepository.GetServiceRates(shipment.Ups, serviceTypes).ToList();

                ApplyRateSurcharges(shipment, upsLocalServiceRates);

                return upsLocalServiceRates.Cast<UpsServiceRate>().OrderBy(r => r.Amount).ToList();
            }
            catch (UpsLocalRatingException)
            {
                return new List<UpsServiceRate>();
            }
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        private void ApplyRateSurcharges(ShipmentEntity shipment, IEnumerable<UpsLocalServiceRate> upsLocalServiceRates)
        {
            IDictionary<UpsSurchargeType, double> surcharges = rateRepository.GetSurcharges(shipment.Ups.UpsAccountID);
            UpsLocalRatingZoneFileEntity zoneFile = rateRepository.GetLatestZoneFile();
            IEnumerable<IUpsSurcharge> upsSurcharges = surchargeFactory.Get(surcharges, zoneFile);

            StringBuilder sb = new StringBuilder();

            foreach (UpsLocalServiceRate serviceRate in upsLocalServiceRates)
            {
                foreach (IUpsSurcharge upsSurcharge in upsSurcharges)
                {
                    upsSurcharge.Apply(shipment.Ups, serviceRate);
                }

                serviceRate.Log(sb);
            }

            apiLog.LogResponse(sb.ToString(), "txt");
        }

        /// <summary>
        /// Gets the eligible services.
        /// </summary>
        private IEnumerable<UpsServiceType> GetEligibleServices(ShipmentEntity shipment)
        {
            IEnumerable<UpsServiceType> eligibleServieTypes = new[]
            {
                UpsServiceType.UpsGround,
                UpsServiceType.Ups3DaySelect,
                UpsServiceType.Ups2DayAir,
                UpsServiceType.Ups2DayAirAM,
                UpsServiceType.UpsNextDayAirSaver,
                UpsServiceType.UpsNextDayAir
            };

            foreach (IServiceFilter serviceFilter in serviceFilters)
            {
                eligibleServieTypes = serviceFilter.GetEligibleServices(shipment.Ups, eligibleServieTypes);
            }

            return eligibleServieTypes;
        }
    }
}