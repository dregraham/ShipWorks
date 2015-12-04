﻿using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public class FedExRatingService : IRatingService
    {
        private readonly ICachedRatesService cachedRatesService;
        private ICarrierSettingsRepository settingsRepository;
        private readonly FedExShipmentType fedExShipmentType;
        private readonly FedExShippingClerkFactory shippingClerkFactory;

        public FedExRatingService(ICachedRatesService cachedRatesService, 
            FedExSettingsRepository settingsRepository, 
            FedExShipmentType fedExShipmentType,
            FedExShippingClerkFactory shippingClerkFactory)
        { 
            this.cachedRatesService = cachedRatesService;
            this.settingsRepository = settingsRepository;
            this.fedExShipmentType = fedExShipmentType;
            this.shippingClerkFactory = shippingClerkFactory;
        }

        /// <summary>
        /// Get a list of rates for the FedEx shipment
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            string originalHubID = shipment.FedEx.SmartPostHubID;

            try
            {
                return cachedRatesService.GetCachedRates<FedExException>(shipment, GetRatesFromApi);
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(fedExShipmentType));
                return errorRates;
            }
            finally
            {
                // Switch the settings repository back to the original now that we have counter rates
                shipment.FedEx.SmartPostHubID = originalHubID;
            }
        }

        /// <summary>
        /// Get a list of rates for the FedEx shipment from the FedEx API
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            // if there are no FedEx accounts call get rates using counter rates
            if (!settingsRepository.GetAccounts().Any() && !fedExShipmentType.IsShipmentTypeRestricted)
            {
                CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);
                return shippingClerkFactory.CreateShippingClerk(shipment, true).GetRates(shipment);
            }

            // there must be at least one FedEx account so we get rates using it
            return shippingClerkFactory.CreateShippingClerk(shipment, false).GetRates(shipment);
        }
    }
}
