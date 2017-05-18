using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Calculate local rates
    /// </summary>
    [Component(RegistrationType.Self)]
    public class UpsLocalRateClient : IUpsRateClient
    {
        private readonly IApiLogEntry apiLog;
        private readonly IUpsLocalRateTable localRateTable;
        private readonly IIndex<UpsRatingMethod, IUpsRateClient> upsRateClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateClient"/> class.
        /// </summary>
        public UpsLocalRateClient(IUpsLocalRateTable localRateTable,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
            IIndex<UpsRatingMethod, IUpsRateClient> upsRateClientFactory)
        {
            this.localRateTable = localRateTable;
            this.upsRateClientFactory = upsRateClientFactory;
            apiLog = apiLogEntryFactory(ApiLogSource.UpsLocalRating, "Rate");
            LocalRates = Enumerable.Empty<UpsLocalServiceRate>();
        }

        /// <summary>
        /// The UPS service rates from the last rating activity.
        /// </summary>
        /// <value>The UPS service rates that were calculated locally.</value>
        public IEnumerable<UpsLocalServiceRate> LocalRates { get; private set; }

        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        public GenericResult<List<UpsServiceRate>> GetRates(ShipmentEntity shipment)
        {
            GenericResult<IEnumerable<UpsLocalServiceRate>> calculatedRateResult = localRateTable.CalculateRates(shipment);
            GenericResult<List<UpsServiceRate>> rateResult;
            if (calculatedRateResult.Success)
            {
                LocalRates = calculatedRateResult.Value.ToList();
                LogSuccess(LocalRates);
                rateResult = GenerateLocalRateResults(LocalRates);
            }
            else
            {
                LogFailure(calculatedRateResult.Message);
                rateResult = GetRatesFromApi(shipment);
            }

            return rateResult;
        }

        /// <summary>
        /// Generates the return value for GetRates from a list of UpsLocalServiceRates.
        /// </summary>
        private static GenericResult<List<UpsServiceRate>> GenerateLocalRateResults(IEnumerable<UpsLocalServiceRate> localRates)
        {
            List<UpsServiceRate> serviceRates = localRates.Cast<UpsServiceRate>().OrderBy(r => r.Amount).ToList();
            return GenericResult.FromSuccess(serviceRates);
        }

        /// <summary>
        /// Logs Successfully retrieved rates.
        /// </summary>
        private void LogSuccess(IEnumerable<UpsLocalServiceRate> localRates)
        {
            StringBuilder logMessage = new StringBuilder();
            localRates.ForEach(rate => rate.Log(logMessage));
            apiLog.LogResponse(logMessage.ToString(), "txt");
        }

        /// <summary>
        /// Logs rate failure.
        /// </summary>
        private void LogFailure(string errorMessage)
        {
            string completeErrorMessage = $"Error when calculating rates:\n\n{errorMessage}\n\nDelegating to UPS API.";
            apiLog.LogResponse(completeErrorMessage, "txt");
        }

        private GenericResult<List<UpsServiceRate>> GetRatesFromApi(ShipmentEntity shipment)
        {
            return upsRateClientFactory[UpsRatingMethod.ApiOnly].GetRates(shipment);
        }
    }
}