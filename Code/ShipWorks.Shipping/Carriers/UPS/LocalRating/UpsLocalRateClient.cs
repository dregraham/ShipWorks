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
            StringBuilder logMessage = new StringBuilder();

            List<UpsLocalServiceRate> localRates = CalculateRates(shipment, logMessage).ToList();

            GenericResult<List<UpsServiceRate>> rateResult;
            if (localRates.Any())
            {
                localRates.ForEach(rate=>rate.Log(logMessage));
                List<UpsServiceRate> serviceRates = localRates.Cast<UpsServiceRate>().OrderBy(r => r.Amount).ToList();
                rateResult = GenericResult.FromSuccess(serviceRates);
            }
            else
            {
                logMessage.AppendLine("No local rates available. Delegating to API.");
                rateResult = upsRateClientFactory[UpsRatingMethod.Api].GetRates(shipment);
            }

            apiLog.LogResponse(logMessage.ToString(), "txt");
            
            return rateResult;
        }

        /// <summary>
        /// Gets rates for the given <paramref name="shipment" />
        /// </summary>
        private IEnumerable<UpsLocalServiceRate> CalculateRates(ShipmentEntity shipment, StringBuilder logMessage)
        {
            try
            {
                IEnumerable<UpsLocalServiceRate> upsLocalServiceRates = 
                    rateRepository.GetServiceRates(shipment.Ups).ToList();

                upsLocalServiceRates = ApplyServiceFilters(shipment, upsLocalServiceRates);

                ApplyRateSurcharges(shipment, upsLocalServiceRates);

                return upsLocalServiceRates;
            }
            catch (UpsLocalRatingException ex)
            {
                logMessage.AppendLine($"Error calculating local rates:\n{ex.Message}");
                return new List<UpsLocalServiceRate>();
            }
        }

        private IEnumerable<UpsLocalServiceRate> ApplyServiceFilters(ShipmentEntity shipment,
            IEnumerable<UpsLocalServiceRate> upsLocalServiceRates)
        {
            IEnumerable<UpsServiceType> eligibleServices = upsLocalServiceRates.Select(r => r.Service);

            foreach (IServiceFilter serviceFilter in serviceFilters)
            {
                eligibleServices = serviceFilter.GetEligibleServices(shipment.Ups, eligibleServices);
            }

            return upsLocalServiceRates.Where(r => eligibleServices.Contains(r.Service));
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        private void ApplyRateSurcharges(ShipmentEntity shipment, IEnumerable<UpsLocalServiceRate> upsLocalServiceRates)
        {
            IDictionary<UpsSurchargeType, double> surcharges = rateRepository.GetSurcharges(shipment.Ups.UpsAccountID);
            UpsLocalRatingZoneFileEntity zoneFile = rateRepository.GetLatestZoneFile();
            IEnumerable<IUpsSurcharge> upsSurcharges = surchargeFactory.Get(surcharges, zoneFile);

            foreach (UpsLocalServiceRate serviceRate in upsLocalServiceRates)
            {
                foreach (IUpsSurcharge upsSurcharge in upsSurcharges)
                {
                    upsSurcharge.Apply(shipment.Ups, serviceRate);
                }
            }
        }
    }
}