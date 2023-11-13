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
        private readonly IProxiedShipEngineWebClient proxiedShipEngineWebClient;
        private readonly UpsGroundSaverState upsGroundSaverState;
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipEngineRatingService(IShipEngineWebClient shipEngineWebClient,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactory,
            IShipmentTypeManager shipmentTypeManager,
            IShipEngineRateGroupFactory rateGroupFactory,
            IUpsShipmentValidatorFactory upsShipmentValidatorFactory,
            IProxiedShipEngineWebClient proxiedShipEngineWebClient, UpsGroundSaverState upsGroundSaverState)
        {
            this.shipEngineWebClient = shipEngineWebClient;
            this.rateGroupFactory = rateGroupFactory;
            this.upsShipmentValidatorFactory = upsShipmentValidatorFactory;
            this.rateRequestFactory = rateRequestFactory[ShipmentTypeCode.UpsOnLineTools];
            this.proxiedShipEngineWebClient = proxiedShipEngineWebClient;
            this.upsGroundSaverState = upsGroundSaverState;
            accountRepository = new UpsAccountRepository();
            shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.UpsOnLineTools);
        }

        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        public async Task<RateGroup> GetRates(ShipmentEntity shipment)
        {
            var account = accountRepository.Accounts.FirstOrDefault(a => !string.IsNullOrEmpty(a.ShipEngineCarrierId));
            // We don't have any ShipEngine UPS accounts, so let the user know they need an account. In the system, it could be only one account connected to ShipEngine
            if (account == null)
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
                upsGroundSaverState.IsGroundSaverEnabled |= await EnsureGroundSaverIsEnabled(account.ShipEngineCarrierId).ConfigureAwait(false);

                RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);
                RateShipmentResponse rateShipmentResponse = await shipEngineWebClient.RateShipment(request, ApiLogSource.UPS).ConfigureAwait(false);


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

        private async Task<bool> EnsureGroundSaverIsEnabled(string shipEngineCarrierId)
        {
            var groundSaverEnabledResult = await proxiedShipEngineWebClient.UpsGroundSaverEnabledState(shipEngineCarrierId).ConfigureAwait(false);
            if (groundSaverEnabledResult.Success)
            {
                if (groundSaverEnabledResult.Value)
                {
                    return true;
                }
                return (await proxiedShipEngineWebClient.UpsGroundSaverEnable(shipEngineCarrierId).ConfigureAwait(false)).Success;
            }
            return false;
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
