using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Partial class extension of the LLBLGen ReadOnlyShipmentEntity
    /// </summary>
    public partial class ReadOnlyShipmentEntity
    {
        private PersonAdapter originPerson;
        private PersonAdapter shipPerson;

        /// <summary>
        /// THIS IS ONLY TEMPORARY
        /// It is being used to determine number of newly created shipments for the
        /// LoadShipments telemetry metrics.
        ///
        /// Delete this after the performance code stories are done.
        /// </summary>
        public bool JustCreated { get; private set; }

        /// <summary>
        /// Utility flag to help track if we've pulled customs items form the database
        /// </summary>
        public bool CustomsItemsLoaded { get; private set; }

        /// <summary>
        /// Type of shipment
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode => (ShipmentTypeCode) ShipmentType;

        /// <summary>
        /// Gets the origin as a person adapter
        /// </summary>
        /// <remarks>Because we don't have a readonly person adapter, we'll copy it each time it's requested.
        /// This is a relatively light operation so it shouldn't be a big deal.</remarks>
        public PersonAdapter OriginPerson => originPerson.CopyToNew();

        /// <summary>
        /// Gets the shipping address as a person adapter
        /// </summary>
        /// <remarks>Because we don't have a readonly person adapter, we'll copy it each time it's requested.
        /// This is a relatively light operation so it shouldn't be a big deal.</remarks>
        public PersonAdapter ShipPerson => shipPerson.CopyToNew();

        /// <summary>
        /// Status of the shipment
        /// </summary>
        public ShipmentStatus Status { get; private set; }

        /// <summary>
        /// Indicates if the shipment is known to have been deleted from the database.  This flag is used instead of using Entity.Fields.State = EntityState.Deleted
        /// because when that is set LLBLgen throws an exception if you try to do anything with the entity - which due to threading we may still be showing and dealing
        /// with data from it shortly after its deleted.
        /// </summary>
        public bool DeletedFromDatabase { get; private set; }

        /// <summary>
        /// Copy extra data defined in the custom Shipment entity
        /// </summary>
        partial void CopyCustomShipmentData(IShipmentEntity source)
        {
            JustCreated = source.JustCreated;
            CustomsItemsLoaded = source.CustomsItemsLoaded;
            originPerson = source.OriginPerson.CopyToNew();
            shipPerson = source.ShipPerson.CopyToNew();
            Status = source.Status;
        }
    }
}
