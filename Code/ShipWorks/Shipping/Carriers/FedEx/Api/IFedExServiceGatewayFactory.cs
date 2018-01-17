using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Factory for FedEx service gateway
    /// </summary>
    public interface IFedExServiceGatewayFactory
    {
        /// <summary>
        /// Create a FedEx service gateway
        /// </summary>
        IFedExServiceGateway Create(ICarrierSettingsRepository settingsRepository);

        /// <summary>
        /// Create a FedEx service gateway for the given shipment
        /// </summary>
        /// <returns>
        /// If Email Return Shipment, return OpenShipFedExServiceGateway, else return default gateway.
        /// </returns>
        IFedExServiceGateway Create(IShipmentEntity shipmentEntity, ICarrierSettingsRepository settingsRepository);
    }
}