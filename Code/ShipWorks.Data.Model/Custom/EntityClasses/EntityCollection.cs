using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.HelperClasses
{
    /// <summary>
    /// Partial class extension for LLBLGen's builtin version.
    /// </summary>
    public partial class EntityCollection<TEntity> where TEntity : EntityBase2, IEntity2
    {
        /// <summary>
        /// Overriden to provide v2.5 functionality.  This was broken in 2.6 to call "OnRelatedEntitySet" on the ContainingEntity,
        /// which is wrong and it didn't used to do.  If they fix it, this override can be removed.  Bug filed on their form on 10/03/08.
        /// </summary>
        protected override void PerformSetRelatedEntity(TEntity entity)
        {
            if ((this.ContainingEntity != null))
            {
                entity.SetRelatedEntity(this.ContainingEntity, this.ContainingEntityMappedField);
            }
        }

        /// <summary>
        /// Overriden to provide v2.5 functionality.  This was broken in 2.6 to call "OnRelatedEntityUnset" on the ContainingEntity,
        /// which is wrong and it didn't used to do.  If they fix it, this override can be removed.  Bug filed on their form on 10/03/08.
        /// </summary>
        protected override void PerformUnsetRelatedEntity(TEntity entity)
        {
            if ((this.ContainingEntity != null))
            {
                entity.UnsetRelatedEntity(this.ContainingEntity, this.ContainingEntityMappedField);
            }
        }

        /// <summary>
        /// Method which restores owned data - i.e. considered private to this collection
        /// and not shared with any external object
        /// </summary>
        /// <param name="writer">SerializationWriter</param>
        /// <param name="context">The serialization flags (previously constructed)</param>
        protected override void SerializeOwnedData(SerializationWriter writer, object context)
        {
            // Taken from
            // http://www.llblgen.com/tinyforum/Messages.aspx?ThreadID=11076&HighLight=1

            base.SerializeOwnedData(writer, context);
            byte[] trackerData = new byte[0];

            // BN: commented out Count > 0 check - if we set a tracker, we want to restore it empty or not!
            if ((this.RemovedEntitiesTracker != null))// && (this.RemovedEntitiesTracker.Count > 0))
            {
                // serialize tracker
                FastSerializer serializer = new FastSerializer();
                trackerData = serializer.Serialize(this.RemovedEntitiesTracker).ToArray();
            }
            writer.Write(trackerData);
        }

        /// <summary>
        /// Method which restores owned data - i.e. considered private to this entity
        /// and not shared with any external object
        /// </summary>
        /// <param name="reader">The SerializationReader containing the serialized data</param>
        /// <param name="context">The serialization flags (previously read)</param>
        protected override void DeserializeOwnedData(SerializationReader reader, object context)
        {
            // Taken from
            // http://www.llblgen.com/tinyforum/Messages.aspx?ThreadID=11076&HighLight=1

            base.DeserializeOwnedData(reader, context);
            byte[] trackerData = reader.ReadByteArray();
            if (trackerData.Length > 0)
            {
                // tracker data read, deserialize it to a real tracker collection
                EntityCollection<TEntity> trackerCollection = new EntityCollection<TEntity>();
                FastDeserializer deserializer = new FastDeserializer();
                deserializer.Deserialize(trackerData, trackerCollection);
                this.RemovedEntitiesTracker = trackerCollection;
            }
        }
    }
}
