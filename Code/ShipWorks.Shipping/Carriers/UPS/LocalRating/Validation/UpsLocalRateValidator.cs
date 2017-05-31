using System;
using System.Collections.Generic;
using System.Linq;
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
    [Component(SingleInstance = true)]
    public class UpsLocalRateValidator : IUpsLocalRateValidator
    {
        private readonly IUpsRateClient rateClient;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository;
        private readonly ILocalRateValidationResultFactory validationResultFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private DateTime wakeTime;

        public UpsLocalRateValidator(IIndex<UpsRatingMethod, IUpsRateClient> rateClient,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository,
            ILocalRateValidationResultFactory validationResultFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory
            )
        {
            this.rateClient = rateClient[UpsRatingMethod.LocalOnly];
            this.upsAccountRepository = upsAccountRepository;
            this.validationResultFactory = validationResultFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
        }

        public ILocalRateValidationResult Validate(IEnumerable<ShipmentEntity> shipments)
        {
            int discrepencies = 0;

            if (DateTime.Now > wakeTime)
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    if (RequiresValidation(shipment))
                    {
                        GenericResult<List<UpsServiceRate>> rateResult = rateClient.GetRates(shipment);

                        if (rateResult.Success)
                        {
                            UpsServiceRate rate =
                                rateResult.Value.SingleOrDefault(r => r.Service == (UpsServiceType) shipment.Ups.Service);

                            if (rate?.Amount != shipment.ShipmentCost)
                            {
                                discrepencies++;
                            }
                        }
                        else
                        {
                            discrepencies++;
                        }
                    }
                }
            }

            return validationResultFactory.Create(shipments.Count(), discrepencies);
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
    }
}