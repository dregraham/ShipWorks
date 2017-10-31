using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Factory responsible for creating the correct IFedExShippingClerk for a FedEx shipment.
    /// </summary>
    [Component]
    public class FedExShippingClerkFactory : IFedExShippingClerkFactory
    {
        private readonly Func<IFedExSettingsRepository, IFedExRequestFactory, IFedExShippingClerk> createFedExShippingClerk;
        private readonly Func<IFimsShippingClerk> createFimsShippingClerk;
        readonly Func<IFedExSettingsRepository, IFedExRequestFactory> createRequestFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShippingClerkFactory(
            Func<IFimsShippingClerk> createFimsShippingClerk,
            Func<IFedExSettingsRepository, IFedExRequestFactory, IFedExShippingClerk> createFedExShippingClerk,
            Func<IFedExSettingsRepository, IFedExRequestFactory> createRequestFactory)
        {
            this.createRequestFactory = createRequestFactory;
            this.createFimsShippingClerk = createFimsShippingClerk;
            this.createFedExShippingClerk = createFedExShippingClerk;
        }

        /// <summary>
        /// Creates a shipping clerk
        /// </summary>
        public IFedExShippingClerk Create() =>
            CreateShippingClerk(null, new FedExSettingsRepository());

        /// <summary>
        /// Creates a shipping clerk for a shipment
        /// </summary>
        public IFedExShippingClerk Create(IShipmentEntity shipment) =>
            CreateShippingClerk(shipment, new FedExSettingsRepository());

        /// <summary>
        /// Creates a shipping clerk for a counter rates shipment
        /// </summary>
        public IFedExShippingClerk CreateForCounterRates(IShipmentEntity shipment) =>
            CreateShippingClerk(shipment, new FedExCounterRateAccountRepository(TangoCredentialStore.Instance));

        /// <summary>
        /// Creates an IFedExShippingClerk with the specified shipment and ICertificateInspector.
        /// </summary>
        /// <param name="shipment">The shipment.  If this clerk should not need a shipment, like when doing a Close, just pass null.</param>
        /// <param name="settingsRepository">The IFedExSettingsRepository.</param>
        private IFedExShippingClerk CreateShippingClerk(IShipmentEntity shipment, IFedExSettingsRepository settingsRepository)
        {
            return IsFimsShipment(shipment) ?
                createFimsShippingClerk() :
                createFedExShippingClerk(settingsRepository, createRequestFactory(settingsRepository));
        }

        /// <summary>
        /// Determines if the shipment is a FIMS shipment.
        /// </summary>
        private static bool IsFimsShipment(IShipmentEntity shipment)
        {
            return shipment?.FedEx != null &&
                FedExUtility.IsFimsService((FedExServiceType) shipment.FedEx.Service);
        }
    }
}
