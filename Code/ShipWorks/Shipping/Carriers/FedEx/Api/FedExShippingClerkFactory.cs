using System;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
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
        /// Creates a shipping clerk for a shipment
        /// </summary>
        public IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment, ICarrierSettingsRepository settingsRepository) =>
            CreateShippingClerk(shipment, settingsRepository, new TrustingCertificateInspector());

        /// <summary>
        /// Creates an IFedExShippingClerk with the specified shipment and ICertificateInspector.
        /// </summary>
        /// <param name="shipment">The shipment.  If this clerk should not need a shipment, like when doing a Close, just pass null.</param>
        /// <param name="settingsRepository">The ICarrierSettingsRepository.</param>
        /// <param name="certificateInspector">The certificate inspector to use</param>
        public static IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment,
                ICarrierSettingsRepository settingsRepository, ICertificateInspector certificateInspector)
        {
            return CreateShippingClerk(shipment, settingsRepository, certificateInspector,
                new FedExLabelRepositoryFactory(), () => new FedExRequestFactory(settingsRepository));
        }

        /// <summary>
        /// Creates an IFedExShippingClerk with the specified shipment and ICertificateInspector.
        /// </summary>
        /// <param name="shipment">The shipment.  If this clerk should not need a shipment, like when doing a Close, just pass null.</param>
        /// <param name="settingsRepository">The ICarrierSettingsRepository.</param>
        public static IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment,
            ICarrierSettingsRepository settingsRepository,
            ICertificateInspector certificateInspector,
            IFedExLabelRepositoryFactory labelRepositoryFactory,
            Func<IFedExRequestFactory> createRequestFactory)
        {
            return IsFimsShipment(shipment) ?
                CreateFimsShippingClerk(settingsRepository, labelRepositoryFactory.CreateFims) :
                CreateFedExShippingClerk(settingsRepository, certificateInspector,
                    labelRepositoryFactory.CreateFedEx, createRequestFactory);
        }

        /// <summary>
        /// Create a FedEx shipping clerk
        /// </summary>
        private static IFedExShippingClerk CreateFedExShippingClerk(ICarrierSettingsRepository settingsRepository,
            ICertificateInspector certificateInspector,
            Func<ILabelRepository> createLabelRepository, Func<IFedExRequestFactory> createRequestFactory)
        {
            FedExShippingClerkParameters parameters = new FedExShippingClerkParameters()
            {
                Inspector = certificateInspector,
                ForceVersionCapture = false,
                LabelRepository = createLabelRepository(),
                RequestFactory = createRequestFactory(),
                SettingsRepository = settingsRepository,
                Log = LogManager.GetLogger(typeof(FedExShippingClerk)),
                ExcludedServiceTypeRepository = new ExcludedServiceTypeRepository()
            };

            return new FedExShippingClerk(parameters);
        }

        /// <summary>
        /// Create a FIMS shipping clerk
        /// </summary>
        private static IFedExShippingClerk CreateFimsShippingClerk(ICarrierSettingsRepository settingsRepository,
            Func<IFimsLabelRepository> createFimsLabelRepository)
        {
            IFimsWebClient client = settingsRepository.UseTestServer ?
                (IFimsWebClient) new FimsFakeWebClient() : new FimsWebClient();

            return new FimsShippingClerk(client, createFimsLabelRepository());
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
                certificateInspector = new CertificateInspector(TangoCredentialStore.Instance.FedExCertificateVerificationData);
            }
            else
            {
                settingsRepository = new FedExSettingsRepository();
                certificateInspector = new TrustingCertificateInspector();
            }

            return CreateShippingClerk(shipment, settingsRepository, certificateInspector);
        }

        /// <summary>
        /// Determines if the shipment is a FIMS shipment.
        /// </summary>
        private static bool IsFimsShipment(ShipmentEntity shipment)
        {
            return shipment?.FedEx != null &&
                FedExUtility.IsFimsService((FedExServiceType) shipment.FedEx.Service);
        }
    }
}
