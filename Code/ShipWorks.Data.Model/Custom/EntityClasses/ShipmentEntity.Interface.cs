using Interapptive.Shared.Business;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'Shipment'
    /// </summary>
    public partial interface IShipmentEntity
    {
        /// <summary>
        /// Utility flag to help track if we've pulled customs items form the database
        /// </summary>
        bool CustomsItemsLoaded { get; }

        /// <summary>
        /// Type of shipment
        /// </summary>
        ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Gets the origin as a person adapter
        /// </summary>
        PersonAdapter OriginPerson { get; }

        /// <summary>
        /// Gets the shipping address as a person adapter
        /// </summary>
        PersonAdapter ShipPerson { get; }

        /// <summary>
        /// Status of the shipment
        /// </summary>
        ShipmentStatus Status { get; }
    }
}
