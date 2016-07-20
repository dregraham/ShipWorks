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
        /// THIS IS ONLY TEMPORARY
        /// It is being used to determine number of newly created shipments for the
        /// LoadShipments telemetry metrics.
        ///
        /// Delete this after the performance code stories are done.
        /// </summary>
        bool JustCreated { get; }

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
