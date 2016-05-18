using System.Collections.Generic;
using Autofac;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Manages all ShipmentTypes available in ShipWorks
    /// </summary>
    public class ShipmentTypeManagerWrapper : IShipmentTypeManager
    {
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeManagerWrapper(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Returns all shipment types in ShipWorks
        /// </summary>
        public List<ShipmentType> ShipmentTypes => ShipmentTypeManager.ShipmentTypes;

        /// <summary>
        /// Returns the ShipmentType for the given type code
        /// </summary>
        public ShipmentType GetType(ShipmentTypeCode typeCode) => ShipmentTypeManager.GetType(typeCode, lifetimeScope);
    }
}