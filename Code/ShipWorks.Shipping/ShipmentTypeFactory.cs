using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Retrieve a shipment type based on its code
    /// </summary>
    /// <remarks>
    /// It's easier to test when the factory is a real type rather than
    /// relying on using Autofac's IIndex implementation.</remarks>
    public class ShipmentTypeFactory : IShipmentTypeFactory
    {
        private readonly IIndex<ShipmentTypeCode, ShipmentType> lookup;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lookup"></param>
        public ShipmentTypeFactory(IIndex<ShipmentTypeCode, ShipmentType> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Get the provider for the specified shipment
        /// </summary>
        public ShipmentType Get(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            return Get(shipment.ShipmentTypeCode);
        }
        
        /// <summary>
        /// Get the shipment type based on its code
        /// </summary>
        public ShipmentType Get(ShipmentTypeCode shipmentTypeCode) => lookup[shipmentTypeCode];
    }
}
