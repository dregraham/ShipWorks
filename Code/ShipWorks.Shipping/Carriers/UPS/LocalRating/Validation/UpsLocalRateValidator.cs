using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
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
    [Obfuscation(Exclude = true)]
    [NamedComponent(nameof(UpsLocalRateValidator), typeof(IUpsLocalRateValidator), SingleInstance = true)]
    public class UpsLocalRateValidator : IUpsLocalRateValidator
    {
        private readonly IUpsRateClient rateClient;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository;
        private readonly ILocalRateValidationResultFactory validationResultFactory;
        private readonly IApiLogEntry logger;
        private DateTime wakeTime;
        private List<UpsLocalRateDiscrepancy> rateDiscrepancies;

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
        public ILocalRateValidationResult ValidateShipments(IEnumerable<ShipmentEntity> shipments)
        {
            // Reset discrepancy list every validation run
            rateDiscrepancies = new List<UpsLocalRateDiscrepancy>();

            if (DateTime.Now > wakeTime)
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    EnsureLocalRatesMatchShipmentCost(shipment);
                }
            }

            if (rateDiscrepancies.Any())
            {
                string log = string.Join(Environment.NewLine, rateDiscrepancies.Select(rateDiscrepancy => rateDiscrepancy.GetLogMessage()).ToList());
                logger.LogResponse(log, ".txt");
            }

            return validationResultFactory.Create(rateDiscrepancies, shipments.Count(), Snooze);
        }

        public ILocalRateValidationResult ValidateRecentShipments(UpsAccountEntity account)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ensures the shipments local rate matches is actual shipment cost. If not add to list of discrepancies. 
        /// </summary>
        private void EnsureLocalRatesMatchShipmentCost(ShipmentEntity shipment)
        {
            if (RequiresValidation(shipment))
            {
                GenericResult<List<UpsServiceRate>> rateResult = rateClient.GetRates(shipment);

                if (rateResult.Success)
                {
                    UpsLocalServiceRate rate =
                        rateResult.Value.Cast<UpsLocalServiceRate>().SingleOrDefault(r => r.Service == (UpsServiceType)shipment.Ups.Service);

                    if (rate?.Amount != shipment.ShipmentCost)
                    {
                        // Local rate did not match actual shipment cost
                        rateDiscrepancies.Add(new UpsLocalRateDiscrepancy(shipment, rate));
                    }
                }
                else
                {
                    // Could not find local rate for shipment
                    rateDiscrepancies.Add(new UpsLocalRateDiscrepancy(shipment, null));
                }
            }
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
            return shipment.Ups != null &&
                shipment.Ups.PayorType != (int) UpsPayorType.ThirdParty &&
                upsAccountRepository.GetAccountReadOnly(shipment).LocalRatingEnabled;
        }
    }
}