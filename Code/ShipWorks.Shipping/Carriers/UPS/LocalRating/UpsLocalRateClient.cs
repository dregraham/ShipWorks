using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
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
    /// <seealso cref="ShipWorks.Shipping.Carriers.UPS.IUpsRateClient" />
    public class UpsLocalRateClient : IUpsRateClient
    {
        private readonly IApiLogEntry apiLog;
        private readonly IUpsLocalRateTableRepository rateRepository;
        private readonly IEnumerable<IServiceFilter> serviceFilters;
        private readonly IUpsSurchargeFactory surchargeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateClient"/> class.
        /// </summary>
        public UpsLocalRateClient(IEnumerable<IServiceFilter> serviceFilters,
            IUpsLocalRateTableRepository rateRepository,
            IUpsSurchargeFactory surchargeFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.serviceFilters = serviceFilters;
            this.rateRepository = rateRepository;
            this.surchargeFactory = surchargeFactory;
            apiLog = apiLogEntryFactory(ApiLogSource.UpsLocalRating, "Rate");
        }

        /// <summary>
        /// Gets rates for the given <paramref name="shipment" />
        /// </summary>
        public List<UpsServiceRate> GetRates(ShipmentEntity shipment)
        {
            IEnumerable<UpsServiceType> serviceTypes = GetEligibleServices(shipment);

            IEnumerable<UpsLocalServiceRate> upsLocalServiceRates =
                rateRepository.GetServiceRates(shipment.Ups, serviceTypes).ToList();
            
            ApplyRateSurcharges(shipment, upsLocalServiceRates);
            return upsLocalServiceRates.Cast<UpsServiceRate>().ToList();
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        private void ApplyRateSurcharges(ShipmentEntity shipment, IEnumerable<UpsLocalServiceRate> upsLocalServiceRates)
        {
            IDictionary<UpsSurchargeType, double> surcharges = rateRepository.GetSurcharges(shipment.Ups.UpsAccountID);
            StringBuilder sb = new StringBuilder();

            foreach (UpsLocalServiceRate serviceRate in upsLocalServiceRates)
            {
                IEnumerable<IUpsSurcharge> upsSurcharges = surchargeFactory.Get(surcharges);

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