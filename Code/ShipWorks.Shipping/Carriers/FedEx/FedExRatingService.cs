using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public class FedExRatingService : IRatingService
    {
        private readonly ICachedRatesService cachedRatesService;
        private ICarrierSettingsRepository settings;
        private ICertificateInspector certificateInspector;
        private readonly FedExShipmentType fedExShipmentType;

        public FedExRatingService(ICachedRatesService cachedRatesService, ICarrierSettingsRepository settings,
            ICertificateInspector certificateInspector, FedExShipmentType fedExShipmentType)
        {
            this.cachedRatesService = cachedRatesService;
            this.settings = settings;
            this.certificateInspector = certificateInspector;
            this.fedExShipmentType = fedExShipmentType;
        }

        /// <summary>
        /// Get a list of rates for the FedEx shipment
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            ICarrierSettingsRepository originalSettings = settings;
            ICertificateInspector originalInspector = certificateInspector;
            string originalHubID = shipment.FedEx.SmartPostHubID;

            try
            {
                // Check with the SettingsRepository here rather than FedExAccountManager, so getting 
                // counter rates from the broker is not impacted
                if (!settings.GetAccounts().Any() && !fedExShipmentType.IsShipmentTypeRestricted)
                {
                    CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);

                    // We need to swap out the SettingsRepository and certificate inspector 
                    // to get FedEx counter rates
                    settings = new FedExCounterRateAccountRepository(TangoCredentialStore.Instance);
                    certificateInspector = new CertificateInspector(TangoCredentialStore.Instance.FedExCertificateVerificationData);
                }

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
                settings = originalSettings;
                certificateInspector = originalInspector;
                shipment.FedEx.SmartPostHubID = originalHubID;
            }
        }

        /// <summary>
        /// Get a list of rates for the FedEx shipment from the FedEx API
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            return FedExShippingClerkFactory.CreateShippingClerk(shipment, settings).GetRates(shipment);
        }
    }
}
