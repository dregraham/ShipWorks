using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Validates whether or not a shipments local rate matches it's actual label cost
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation.IUpsLocalRateValidator" />
    [Component(SingleInstance = true)]
    public class UpsLocalRateValidator : IUpsLocalRateValidator
    {
        private readonly IUpsRateClient rateClient;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository;
        private readonly ILocalRateValidationResultFactory validationResultFactory;
        private readonly IApiLogEntry logger;
        private DateTime wakeTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateValidator"/> class.
        /// </summary>
        public UpsLocalRateValidator(IIndex<UpsRatingMethod, IUpsRateClient> rateClientFactory,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository,
            ILocalRateValidationResultFactory validationResultFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            rateClient = rateClientFactory[UpsRatingMethod.LocalOnly];
            this.upsAccountRepository = upsAccountRepository;
            this.validationResultFactory = validationResultFactory;
            logger = apiLogEntryFactory(ApiLogSource.UpsLocalRating, "Rate Discrepancies");
        }

        /// <summary>
        /// Given a list of processed UPS shipments, if applicable, validate the local rates match the rate charged by UPS
        /// </summary>
        public ILocalRateValidationResult Validate(IEnumerable<ShipmentEntity> shipments)
        {
            int discrepancies = 0;
            List<ShipmentEntity> processedShipments = null;

            StringBuilder logBuilder = new StringBuilder();

            if (DateTime.Now > wakeTime)
            {
                processedShipments = shipments.Where(s => s.Processed).ToList();

                foreach (ShipmentEntity shipment in processedShipments)
                {
                    if (EnsureLocalRatesMatchShipmentCost(shipment, logBuilder) == false)
                    {
                        discrepancies++;
                    }
                }
            }

            if (logBuilder.Length > 0)
            {
                logger.LogResponse(logBuilder.ToString(), "txt");
            }

            return validationResultFactory.Create(processedShipments?.Count() ?? 0, discrepancies, Snooze);
        }

        /// <summary>
        /// Validates the shipment.
        /// </summary>
        /// <returns>true if the local rates match the cost of the shipment, else false</returns>
        private bool EnsureLocalRatesMatchShipmentCost(ShipmentEntity shipment, StringBuilder logBuilder)
        {
            bool isValid = true;
        
            if (RequiresValidation(shipment))
            {
                GenericResult<List<UpsServiceRate>> rateResult = rateClient.GetRates(shipment);

                if (rateResult.Success)
                {
                    UpsServiceRate rate =
                        rateResult.Value.SingleOrDefault(r => r.Service == (UpsServiceType)shipment.Ups.Service);

                    if (rate?.Amount != shipment.ShipmentCost)
                    {
                        isValid = false;
                        AddDiscrepancyToLogger(logBuilder, shipment, rate);
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Suppresses validation for a limited amount of time or until SW restarts
        /// </summary>
        public void Snooze()
        {
            wakeTime = DateTime.Now.AddHours(12);
        }
        
        /// <summary>
        /// Whether or not the shipment should have it's local rates validated
        /// </summary>
        /// <remarks>
        /// Only validate UPS shipments, where third party billing is not used, and the account has local rating enabled.
        /// 
        /// Third party billing does not affect the returned API rates, even though it should.
        /// Don't bother validating since we know the API rate is wrong anyway.
        /// </remarks>
        private bool RequiresValidation(ShipmentEntity shipment)
        {
            return shipment.Ups != null && shipment.Ups.PayorType != (int) UpsPayorType.ThirdParty &&
                    upsAccountRepository.GetAccountReadOnly(shipment).LocalRatingEnabled;
        }

        /// <summary>
        /// Logs the discrepancies. Includes the shipmentID, the local rate, and the actual shipment cost
        /// </summary>
        private void AddDiscrepancyToLogger(StringBuilder stringBuilder, ShipmentEntity shipment, UpsServiceRate rate)
        {
            stringBuilder.AppendLine($"Order Number: {shipment.Order.OrderNumber}");
            stringBuilder.AppendLine($"Shipment ID: {shipment.ShipmentID}");
            stringBuilder.AppendLine($"Local Rate: {rate?.Amount.ToString(CultureInfo.InvariantCulture) ?? "Not found"}");
            stringBuilder.AppendLine($"Label Cost: {shipment.ShipmentCost}");
            stringBuilder.AppendLine();
        }
    }
}