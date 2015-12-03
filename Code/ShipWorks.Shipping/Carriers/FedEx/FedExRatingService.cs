using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public class FedExRatingService : IRatingService
    {
        private readonly ICachedRatesService cachedRatesService;
        private FedExSettingsRepository settingsRepository;
        private readonly FedExShipmentType fedExShipmentType;
        private readonly Func<ShipmentEntity, IShippingClerk> shippingClerkFactory;
        private readonly Func<string, ICertificateInspector> certificateInspectorFactory;

        public FedExRatingService(ICachedRatesService cachedRatesService, 
            FedExSettingsRepository settingsRepository, 
            FedExShipmentType fedExShipmentType, Func<ShipmentEntity, IShippingClerk> shippingClerkFactory, Func<string, ICertificateInspector> certificateInspectorFactory)
        { 
            this.cachedRatesService = cachedRatesService;
            this.settingsRepository = settingsRepository;
            this.fedExShipmentType = fedExShipmentType;
            this.shippingClerkFactory = shippingClerkFactory;
            this.certificateInspectorFactory = certificateInspectorFactory;
        }

        /// <summary>
        /// Get a list of rates for the FedEx shipment
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            FedExSettingsRepository originalSettings = settingsRepository;
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
                settingsRepository = originalSettings;
                shipment.FedEx.SmartPostHubID = originalHubID;
            }
        }

        /// <summary>
        /// Get a list of rates for the FedEx shipment from the FedEx API
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            ICertificateInspector certificateInspector = null;

            // Check with the SettingsRepository here rather than FedExAccountManager, so getting 
            // counter rates from the broker is not impacted
            if (!settingsRepository.GetAccounts().Any() && !fedExShipmentType.IsShipmentTypeRestricted)
            {
                CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);

                // We need to swap out the SettingsRepository and certificate inspector 
                // to get FedEx counter rates
                settingsRepository = new FedExCounterRateAccountRepository(TangoCredentialStore.Instance);

                certificateInspector = certificateInspectorFactory("");
            }
            
            return shippingClerkFactory(shipment).GetRates(shipment, certificateInspector);
        }
    }
}
