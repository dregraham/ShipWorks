using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateClient"/> class.
        /// </summary>
        public UpsLocalRateClient(IUpsLocalRateTable localRateTable,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.localRateTable = localRateTable;
            apiLog = apiLogEntryFactory(ApiLogSource.UpsLocalRating, "Rate");
        }

        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        public GenericResult<List<UpsServiceRate>> GetRates(ShipmentEntity shipment)
        {
            GenericResult<IEnumerable<UpsLocalServiceRate>> calculatedRateResult = localRateTable.CalculateRates(shipment);
            GenericResult<List<UpsServiceRate>> rateResult;
            if (calculatedRateResult.Success)
            {
                IEnumerable<UpsLocalServiceRate> localRates = calculatedRateResult.Value.ToList();
                LogSuccess(localRates, shipment);
                rateResult = GenerateLocalRateResults(localRates);
            }
            else
            {
                LogFailure(calculatedRateResult.Message);
                rateResult = GenericResult.FromError(calculatedRateResult.Message, new List<UpsServiceRate>());
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
        private void LogSuccess(IEnumerable<UpsLocalServiceRate> localRates, ShipmentEntity shipment)
        {
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine($"OrderID: {shipment.Order.OrderID}");
            logMessage.AppendLine($"ShipmentID: {shipment.ShipmentID}{Environment.NewLine}");
            localRates.ForEach(rate => rate.Log(logMessage));
            apiLog.LogResponse(logMessage.ToString(), "txt");
        }

        /// <summary>
        /// Logs rate failure.
        /// </summary>
        private void LogFailure(string errorMessage)
        {
            string completeErrorMessage = $"Error when calculating rates:\n\n{errorMessage}";
            apiLog.LogResponse(completeErrorMessage, "txt");
        }
    }
}