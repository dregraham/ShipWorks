using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using Interapptive.Shared.Business;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extention of the LLBLGen ShipmentEntity
    /// </summary>
    public partial class ShipmentEntity
    {
        bool customsItemsLoaded = false;
        bool deletedFromDatabase = false;

        /// <summary>
        /// Utility flag to help track if we've pulled customs items form the database
        /// </summary>
        public bool CustomsItemsLoaded
        {
            get { return customsItemsLoaded; }
            set { customsItemsLoaded = value; }
        }

        /// <summary>
        /// Gets the origin as a person adapter
        /// </summary>
        public PersonAdapter OriginPerson
        {
            get { return new PersonAdapter(this, "Origin"); }
        }

        /// <summary>
        /// Gets the shipping address as a person adapter
        /// </summary>
        public PersonAdapter ShipPerson
        {
            get {return new PersonAdapter(this, "Ship"); }
        }

        /// <summary>
        /// Indicates if the shipment is known to have been deleted from the database.  This flag is used instead of using Entity.Fields.State = EntityState.Deleted
        /// because when that is set LLBLgen throws an exception if you try to do anyting with the entity - which due to threading we may still be showing and dealing
        /// with data from it shortly after its deleted.
        /// </summary>
        public bool DeletedFromDatabase
        {
            get { return deletedFromDatabase; }
            set { deletedFromDatabase = value; }
        }

        /// <summary>
        /// Has to be overridden to serialize our extra data
        /// </summary>
        protected override void OnGetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.OnGetObjectData(info, context);

            info.AddValue("customsItemsLoaded", customsItemsLoaded);
            info.AddValue("deletedFromDatabase", deletedFromDatabase);
        }

        /// <summary>
        /// Has to be overridden to deserialize our extra data
        /// </summary>
        protected override void OnDeserialized(SerializationInfo info, StreamingContext context)
        {
            base.OnDeserialized(info, context);

            customsItemsLoaded = info.GetBoolean("customsItemsLoaded");
            deletedFromDatabase = info.GetBoolean("deletedFromDatabase");
        }
    }
}
