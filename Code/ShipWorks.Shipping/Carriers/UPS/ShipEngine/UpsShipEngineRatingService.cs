using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.ShipEngine;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Ups.ShipEngine
{
    /// <summary>
    /// Service for retrieving UPS rates via ShipEngine
    /// </summary>
    [Component]
    class UpsShipEngineRatingService : IUpsShipEngineRatingService
    {
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineRateGroupFactory rateGroupFactory;
        private readonly IUpsShipmentValidatorFactory upsShipmentValidatorFactory;
        private readonly UpsAccountRepository accountRepository;
        private readonly ICarrierShipmentRequestFactory rateRequestFactory;
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipEngineRatingService(IShipEngineWebClient shipEngineWebClient,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactory,
            IShipmentTypeManager shipmentTypeManager,
            IShipEngineRateGroupFactory rateGroupFactory,
            IUpsShipmentValidatorFactory upsShipmentValidatorFactory)
        {
            this.shipEngineWebClient = shipEngineWebClient;
            this.rateGroupFactory = rateGroupFactory;
            this.upsShipmentValidatorFactory = upsShipmentValidatorFactory;
            this.rateRequestFactory = rateRequestFactory[ShipmentTypeCode.UpsOnLineTools];

            accountRepository = new UpsAccountRepository();
            shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.UpsOnLineTools);
        }

        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            // We don't have any ShipEngine UPS accounts, so let the user know they need an account.
            if (accountRepository.Accounts.All(x => string.IsNullOrEmpty(x.ShipEngineCarrierId)))
            {
                throw new ShippingException("A UPS from ShipWorks account is required to view UPS rates.");
            }

            Result validationResult = upsShipmentValidatorFactory.Create(shipment).ValidateShipment(shipment);
            if (validationResult.Failure)
            {
                throw new ShippingException(validationResult.Message);
            }

            try
            {
                RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);
                RateShipmentResponse rateShipmentResponse = Task.Run(async () =>
                        await shipEngineWebClient.RateShipment(request, ApiLogSource.UPS).ConfigureAwait(false)).Result;

                string countryCode = shipment.AdjustedShipCountryCode();
                IEnumerable<string> availableServiceTypeApiCodes = shipmentType.GetAvailableServiceTypes()
                    .Cast<UpsServiceType>()
                    .Where(serviceType => UpsShipEngineServiceTypeUtility.IsServiceSupported(serviceType, countryCode))
                    .Select(serviceType => UpsShipEngineServiceTypeUtility.GetServiceCode(serviceType, countryCode));

                RateGroup rateGroup = rateGroupFactory.Create(rateShipmentResponse.RateResponse, ShipmentTypeCode.UpsOnLineTools, availableServiceTypeApiCodes);

                AddFootnotes(rateGroup, rateShipmentResponse);

                return rateGroup;
            }
            catch (Exception ex) when (ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Add footnotes to the rate group
        /// </summary>
        private void AddFootnotes(RateGroup rateGroup, RateShipmentResponse rateShipmentResponse)
        {
            var warnings = rateShipmentResponse.RateResponse.Rates.SelectMany(r => r.WarningMessages).Distinct();

            foreach (string warning in warnings)
            {
                if (warning.Contains("Large Package Surcharge", StringComparison.InvariantCultureIgnoreCase))
                {
                    var largePackageSurchargeFootnoteFactory = new InformationFootnoteFactory(warning);
                    rateGroup.AddFootnoteFactory(largePackageSurchargeFootnoteFactory);
                }
            }
        }
    }
}
