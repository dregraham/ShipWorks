using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Factory for FedEx service gateway
    /// </summary>
    [Component]
    public class FedExServiceGatewayFactory : IFedExServiceGatewayFactory
    {
        readonly Func<ICarrierSettingsRepository, IFedExServiceGateway> createDefaultFedExServiceGateway;
        readonly Func<ICarrierSettingsRepository, IFedExOpenShipServiceGateway> createOpenShipFedExServiceGateway;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExServiceGatewayFactory(
            Func<ICarrierSettingsRepository, IFedExServiceGateway> createDefaultFedExServiceGateway,
            Func<ICarrierSettingsRepository, IFedExOpenShipServiceGateway> createOpenShipFedExServiceGateway)
        {
            this.createOpenShipFedExServiceGateway = createOpenShipFedExServiceGateway;
            this.createDefaultFedExServiceGateway = createDefaultFedExServiceGateway;
        }

        /// <summary>
        /// Create a FedEx service gateway
        /// </summary>
        public IFedExServiceGateway Create(ICarrierSettingsRepository settingsRepository) =>
            createDefaultFedExServiceGateway(settingsRepository);

        /// <summary>
        /// Create a FedEx service gateway for the given shipment
        /// </summary>
        /// <returns>
        /// If Email Return Shipment, return OpenShipFedExServiceGateway, else return default gateway.
        /// </returns>
        public IFedExServiceGateway Create(IShipmentEntity shipment, ICarrierSettingsRepository settingsRepository) =>
            RequiresOpenShip(shipment) ?
                createOpenShipFedExServiceGateway(settingsRepository) :
                createDefaultFedExServiceGateway(settingsRepository);

        /// <summary>
        /// Does the shipment require OpenShip
        /// </summary>
        private bool RequiresOpenShip(IShipmentEntity shipment) =>
            shipment.ReturnShipment && shipment.FedEx.ReturnType == (int) FedExReturnType.EmailReturnLabel;
    }
}