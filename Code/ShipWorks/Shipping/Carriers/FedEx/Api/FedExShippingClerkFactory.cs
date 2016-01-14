using System;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Factory responsible for creating the correct IFedExShippingClerk for a FedEx shipment.
    /// </summary>
    public static class FedExShippingClerkFactory
    {
        /// <summary>
        /// Creates an IFedExShippingClerk with the specified shipment and ICertificateInspector.
        /// </summary>
        /// <param name="shipment">The shipment.  If this clerk should not need a shipment, like when doing a Close, just pass null.</param>
        /// <param name="settingsRepository">The ICarrierSettingsRepository.</param>
        public static IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment, ICarrierSettingsRepository settingsRepository) =>
            CreateShippingClerk(shipment, settingsRepository,
            () => new FedExLabelRepository(), () => new FimsLabelRepository(), () => new FedExRequestFactory(settingsRepository));

        /// <summary>
        /// Creates an IFedExShippingClerk with the specified shipment and ICertificateInspector.
        /// </summary>
        /// <param name="shipment">The shipment.  If this clerk should not need a shipment, like when doing a Close, just pass null.</param>
        /// <param name="settingsRepository">The ICarrierSettingsRepository.</param>
        public static IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment,
            ICarrierSettingsRepository settingsRepository,
            Func<ILabelRepository> createFedExLabelRepository,
            Func<IFimsLabelRepository> createFimsLabelRepository,
            Func<IFedExRequestFactory> createRequestFactory)
        {
            return IsFimsShipment(shipment) ?
                CreateFimsShippingClerk(settingsRepository, createFimsLabelRepository) :
                CreateFedExShippingClerk(settingsRepository, createFedExLabelRepository, createRequestFactory);
        }

        /// <summary>
        /// Create a FedEx shipping clerk
        /// </summary>
        private static IFedExShippingClerk CreateFedExShippingClerk(ICarrierSettingsRepository settingsRepository,
            Func<ILabelRepository> createLabelRepository, Func<IFedExRequestFactory> createRequestFactory)
        {
            FedExShippingClerkParameters parameters = new FedExShippingClerkParameters()
            {
                Inspector = new FedExShipmentType().CertificateInspector,
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
            // TODO: Switch to use FIMS test server check when it's implemented.
            IFimsWebClient client = settingsRepository.UseTestServer ?
                (IFimsWebClient) new FimsFakeWebClient() : new FimsWebClient();

            return new FimsShippingClerk(client, createFimsLabelRepository());
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
