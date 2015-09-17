using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;

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
        void LoadCustomsItems(ShipmentEntity shipment, bool v);

        /// <summary>
        /// Ensire customs items are loaded if the address or shipment type has changed
        /// </summary>
        IDictionary<ShipmentEntity, Exception> EnsureCustomsLoaded(IEnumerable<ShipmentEntity> shipments, ValidatedAddressScope validatedAddressScope);
    }
}
