using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Factory responsible for creating the correct IFedExShippingClerk for a FedEx shipment.
    /// </summary>
    public class FedExShippingClerkFactory
    {
        /// <summary>
        /// Creates an IFedExShippingClerk with the specified shipment and ICertificateInspector.
        /// </summary>
        /// <param name="shipment">The shipment.  If this clerk should not need a shipment, like when doing a Close, just pass null.</param>
        /// <param name="settingsRepository">The ICarrierSettingsRepository.</param>
        /// <param name="certificateInspector">The certificate inspector to use</param>
        private IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment, ICarrierSettingsRepository settingsRepository, ICertificateInspector certificateInspector)
        {
            IFedExShippingClerk fedExShippingClerk = null;

            if (!IsFimsShipment(shipment))
            {
                FedExShippingClerkParameters parameters = new FedExShippingClerkParameters()
                {
                    Inspector = certificateInspector,
                    ForceVersionCapture = false,
                    LabelRepository = new FedExLabelRepository(),
                    RequestFactory = new FedExRequestFactory(settingsRepository),
                    SettingsRepository = settingsRepository,
                    Log = LogManager.GetLogger(typeof(FedExShippingClerk)),
                    ExcludedServiceTypeRepository = new ExcludedServiceTypeRepository()
                };

                fedExShippingClerk = new FedExShippingClerk(parameters);
            }
            else
            {
                // TODO: Switch to use FIMS test server check when it's implemented.
                if (settingsRepository.UseTestServer)
                {
                    fedExShippingClerk = new FimsShippingClerk(new FimsFakeWebClient(), new FimsLabelRepository());
                }
                else
                {
                    fedExShippingClerk = new FimsShippingClerk(new FimsWebClient(), new FimsLabelRepository());
                }
            }

            return fedExShippingClerk;
        }

        /// <summary>
        /// Creates a shipping clerk for a shipment
        /// </summary>
        public IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment, ICarrierSettingsRepository settingsRepository)
        {
            ICertificateInspector certificateInspector = null;
            certificateInspector = new TrustingCertificateInspector();
            return CreateShippingClerk(shipment, settingsRepository, certificateInspector);
        }

        /// <summary>
        /// Creates a shipping clerk for a shipment
        /// </summary>
        public IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment, bool useCounterRates)
        {
            ICarrierSettingsRepository settingsRepository = null;
            ICertificateInspector certificateInspector = null;
            
            // Create the appropriate settings, certificate inspector
            if (useCounterRates)
            {
                settingsRepository = new FedExCounterRateAccountRepository(TangoCredentialStore.Instance);
                certificateInspector = new TrustingCertificateInspector();
            }
            else
            {
                settingsRepository = new FedExSettingsRepository();
                certificateInspector = new CertificateInspector(TangoCredentialStore.Instance.FedExCertificateVerificationData);
            }

            return CreateShippingClerk(shipment, settingsRepository, certificateInspector);
        }

        /// <summary>
        /// Determines if the shipment is a FIMS shipment.
        /// </summary>
        private static bool IsFimsShipment(ShipmentEntity shipment)
        {
            return shipment != null && 
                shipment.FedEx != null &&
                FedExUtility.IsFimsService((FedExServiceType)shipment.FedEx.Service);
        }
    }
}
