using System.Collections.Generic;
using Autofac;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Manages all ShipmentTypes available in ShipWorks
    /// </summary>
    public class ShipmentTypeManagerWrapper : IShipmentTypeManager
    {
        /// <summary>
        /// Returns all shipment types in ShipWorks
        /// </summary>
        public List<ShipmentType> ShipmentTypes => ShipmentTypeManager.ShipmentTypes;

        /// <summary>
        /// Returns the ShipmentType for the given type code
        /// </summary>
        public ShipmentType GetType(ShipmentTypeCode typeCode, ILifetimeScope lifetimeScope) => ShipmentTypeManager.GetType(typeCode, lifetimeScope);
    }
}