using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
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
        private readonly IUpsRateClient localRateClient;
        private readonly IUpsRateClient apiRateClient;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository;
        private readonly ILocalRateValidationResultFactory validationResultFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly IShippingManager shippingManager;
        private DateTime wakeTime;
        private List<UpsLocalRateDiscrepancy> rateDiscrepancies;

        public static readonly string CreateLabelLogFileName = "Rate Discrepancies (Create Label)";
        public static readonly string UploadRatesLogFileName = "Rate Discrepancies (Upload Rate File)";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateValidator"/> class.
        /// </summary>
        public UpsLocalRateValidator(IIndex<UpsRatingMethod, IUpsRateClient> rateClientFactory,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository,
            ILocalRateValidationResultFactory validationResultFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
            IShippingManager shippingManager)
        {
            localRateClient = rateClientFactory[UpsRatingMethod.LocalOnly];
            apiRateClient = rateClientFactory[UpsRatingMethod.ApiOnly];
            this.upsAccountRepository = upsAccountRepository;
            this.validationResultFactory = validationResultFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Given a list of processed UPS shipments, if applicable, validate the local rates match the rate charged by UPS
        /// </summary>
        public ILocalRateValidationResult ValidateShipments(IEnumerable<ShipmentEntity> shipments)
        {
            // Reset discrepancy list every validation run
            List<ShipmentEntity> processedShipments = null;

            rateDiscrepancies = new List<UpsLocalRateDiscrepancy>();

            if (DateTime.Now > wakeTime)
            {
                processedShipments = shipments.Where(s => s.Processed).ToList();

                foreach (ShipmentEntity shipment in processedShipments)
                {
                    EnsureLocalRatesMatchShipmentCost(shipment);
                }
            }

            LogRateDiscrepancies(CreateLabelLogFileName);

            return validationResultFactory.Create(rateDiscrepancies, processedShipments?.Count ?? 0, Snooze);
        }

        /// <summary>
        /// Validates the local rate against the api rate for the most recent shipments for the given account
        /// </summary>
        public ILocalRateValidationResult ValidateRecentShipments(UpsAccountEntity account)
        {
            // Reset discrepancy list every validation run
            rateDiscrepancies = new List<UpsLocalRateDiscrepancy>();

            IEnumerable<ShipmentEntity> shipments = GetRecentShipments(account);

            foreach (ShipmentEntity shipment in shipments)
            {
                EnsureLocalRatesMatchApiRates(shipment);
            }
            
            LogRateDiscrepancies(UploadRatesLogFileName);

            return validationResultFactory.Create(rateDiscrepancies);
        }

        /// <summary>
        /// Gets the 10 most recent shipments that were processed using the given UPS account
        /// </summary>
        /// <param name="account">The account.</param>
        private IEnumerable<ShipmentEntity> GetRecentShipments(UpsAccountEntity account)
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.Relations.Add(UpsShipmentEntity.Relations.ShipmentEntityUsingShipmentID);
            bucket.Relations.Add(ShipmentEntity.Relations.UpsShipmentEntityUsingShipmentID);
            bucket.PredicateExpression.Add(UpsShipmentFields.UpsAccountID == account.UpsAccountID);
            bucket.PredicateExpression.AddWithAnd(ShipmentFields.Processed == true);
            
            ISortExpression sortExpression = new SortExpression(ShipmentFields.ProcessedDate | SortOperator.Descending);

            try
            {
                 return shippingManager.GetShipments(bucket, sortExpression, 10);
            }
            catch (ShippingException ex)
            {
                throw new UpsLocalRatingException($"Failed to validate local rates:{Environment.NewLine}{Environment.NewLine}{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Ensures the shipments local rate matches is actual shipment cost. If not add to list of discrepancies. 
        /// </summary>
        private void EnsureLocalRatesMatchShipmentCost(ShipmentEntity shipment)
        {
            if (RequiresValidation(shipment, true))
            {
                GenericResult<List<UpsServiceRate>> rateResult = localRateClient.GetRates(shipment);

                if (rateResult.Success)
                {
                    UpsLocalServiceRate rate =
                        rateResult.Value.Cast<UpsLocalServiceRate>().SingleOrDefault(r => r.Service == (UpsServiceType)shipment.Ups.Service);

                    // UPS shipping API returns 0 when using third party billing. This is
                    // not a discrepancy
                    if (shipment.ShipmentCost > 0 && rate?.Amount != shipment.ShipmentCost)
                    {
                        // Local rate did not match actual shipment cost
                        rateDiscrepancies.Add(new UpsLocalRateDiscrepancy(shipment, rate));
                    }
                }
            }
        }

        /// <summary>
        /// Ensures the shipments local rate matches is actual shipment cost. If not add to list of discrepancies. 
        /// </summary>
        private void EnsureLocalRatesMatchApiRates(ShipmentEntity shipment)
        {
            if (RequiresValidation(shipment, false))
            {
                GenericResult<List<UpsServiceRate>> localRateResult = localRateClient.GetRates(shipment);

                if (localRateResult.Success)
                {
                    UpsServiceType serviceType = (UpsServiceType) shipment.Ups.Service;

                    UpsLocalServiceRate localRate =
                        localRateResult.Value.Cast<UpsLocalServiceRate>().SingleOrDefault(r => r.Service == serviceType);
                    UpsServiceRate apiRate = apiRateClient.GetRates(shipment).Value.SingleOrDefault(r => r.Service == serviceType);

                    // Do not validate if no local OR api rates were found for the shipment
                    if (localRate != null || apiRate != null)
                    {
                        if (localRate?.Amount != apiRate?.Amount)
                        {
                            // Local rate did not match api rate
                            rateDiscrepancies.Add(new UpsLocalRateDiscrepancy(shipment, localRate, apiRate));
                        }
                    }
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
        /// Only validate UPS shipments, where third party billing is not used. And when required,
        /// only when local rating is enabled for the account.
        /// 
        /// Third party billing does not affect the returned API rates, even though it should.
        /// Don't bother validating since we know the API rate is wrong anyway.
        /// </remarks>
        private bool RequiresValidation(ShipmentEntity shipment, bool localRatingEnabledRequiredForValidation)
        {
            bool requiresValidation = (shipment.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools || shipment.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip) &&
                    shipment.Ups != null &&
                    shipment.Ups.PayorType != (int) UpsPayorType.ThirdParty;

            if (localRatingEnabledRequiredForValidation)
            {
                requiresValidation = requiresValidation && upsAccountRepository.GetAccountReadOnly(shipment).LocalRatingEnabled;
            }

            return requiresValidation;
        }

        /// <summary>
        /// Logs the rate discrepancies.
        /// </summary>
        private void LogRateDiscrepancies(string fileName)
        {
            if (rateDiscrepancies.Any())
            {
                string log = string.Join(Environment.NewLine,
                    rateDiscrepancies.Select(rateDiscrepancy => rateDiscrepancy.GetLogMessage()).ToList());
                apiLogEntryFactory(ApiLogSource.UpsLocalRating, fileName).LogResponse(log, "txt");
            }
        }
    }
}