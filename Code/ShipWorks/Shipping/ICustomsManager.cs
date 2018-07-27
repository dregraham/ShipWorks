using System;
using System.Collections.Generic;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Manage customs information
    /// </summary>
    public interface ICustomsManager
    {
        /// <summary>
        /// Ensure custom's contents for the given shipment have been created
        /// </summary>
        void LoadCustomsItems(ShipmentEntity shipment, bool reloadIfPresent, ISqlAdapter adapter);

        /// <summary>
        /// Ensure customs contents for the given shipment have been created
        /// </summary>
        void LoadCustomsItems(ShipmentEntity shipment, bool reloadIfPresent, Func<ShipmentEntity, bool> saveShipment);

        /// <summary>
        /// Generate customs for a shipment.  If the shipment is processed, or doesn't require customs,
        /// or customs have already been generated, nothing will be done.
        /// 
        /// Customs items are not persisted to the database, as that is the caller's responsibility.
        /// </summary>
        void GenerateCustomsItems(ShipmentEntity shipment);

        /// <summary>
        /// Ensure customs items are loaded if the address or shipment type has changed
        /// </summary>
        IDictionary<ShipmentEntity, Exception> EnsureCustomsLoaded(IEnumerable<ShipmentEntity> shipments);

        /// <summary>
        /// Create a new ShipmentCustomsItemEntity for the given shipment, filled in with defaults
        /// </summary>
        ShipmentCustomsItemEntity CreateCustomsItem(ShipmentEntity shipment);
    }
}
