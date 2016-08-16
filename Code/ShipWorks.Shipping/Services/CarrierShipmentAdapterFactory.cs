using ShipWorks.Data.Model.EntityClasses;
using Autofac;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Factory for creating carrier shipment adapters
    /// </summary>
    public class CarrierShipmentAdapterFactory : ICarrierShipmentAdapterFactory
    {
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierShipmentAdapterFactory(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Get a carrier shipment adapter for the specified shipment, using the shipment type of the shipment
        /// </summary>
        public ICarrierShipmentAdapter Get(ShipmentEntity shipment) =>
            lifetimeScope.ResolveKeyed<ICarrierShipmentAdapter>(shipment.ShipmentTypeCode, new TypedParameter(typeof(ShipmentEntity), shipment));
    }
}
