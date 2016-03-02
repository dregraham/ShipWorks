using System.Runtime.Serialization;
using Interapptive.Shared.Business;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extension of the LLBLGen ShipmentEntity
    /// </summary>
    public partial class ShipmentEntity
    {
        /// <summary>
        /// Utility flag to help track if we've pulled customs items form the database
        /// </summary>
        public bool CustomsItemsLoaded { get; set; }

        /// <summary>
        /// Type of shipment
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get { return (ShipmentTypeCode) ShipmentType; }
            set { ShipmentType = (int) value; }
        }

        /// <summary>
        /// Gets the origin as a person adapter
        /// </summary>
        public PersonAdapter OriginPerson
        {
            get { return new PersonAdapter(this, "Origin"); }
            set { PersonAdapter.Copy(value, OriginPerson); }
        }

        /// <summary>
        /// Gets the shipping address as a person adapter
        /// </summary>
        public PersonAdapter ShipPerson
        {
            get { return new PersonAdapter(this, "Ship"); }
            set { PersonAdapter.Copy(value, ShipPerson); }
        }

        /// <summary>
        /// Indicates if the shipment is known to have been deleted from the database.  This flag is used instead of using Entity.Fields.State = EntityState.Deleted
        /// because when that is set LLBLgen throws an exception if you try to do anything with the entity - which due to threading we may still be showing and dealing
        /// with data from it shortly after its deleted.
        /// </summary>
        public bool DeletedFromDatabase { get; set; }

        /// <summary>
        /// Has to be overridden to serialize our extra data
        /// </summary>
        protected override void OnGetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.OnGetObjectData(info, context);

            info.AddValue("customsItemsLoaded", CustomsItemsLoaded);
            info.AddValue("deletedFromDatabase", DeletedFromDatabase);
        }

        /// <summary>
        /// Has to be overridden to deserialize our extra data
        /// </summary>
        protected override void OnDeserialized(SerializationInfo info, StreamingContext context)
        {
            base.OnDeserialized(info, context);

            CustomsItemsLoaded = info.GetBoolean("customsItemsLoaded");
            DeletedFromDatabase = info.GetBoolean("deletedFromDatabase");
        }
    }
}
