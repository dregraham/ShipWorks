using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.Custom.EntityClasses
{
    /// <summary>
    /// Entity with no behavior
    /// </summary>
    /// <remarks>This class is intended to be used in situations where an entity is expected
    /// but specific behavior may not be.  It is an implementation of the null object pattern</remarks>
    public class NullEntity : EntityBase2, ICarrierAccount
    {
        private readonly int hashCode;

        /// <summary>
        /// Constructor
        /// </summary>
        public NullEntity()
        {
            // Make up a hash code so this will work in dictionaries
            hashCode = new Random().Next(Int32.MaxValue);
            base.Fields = new EntityFields2(0);
        }

        /// <summary>
        /// Custom Properties of type
        /// </summary>
        public override Dictionary<string, string> CustomPropertiesOfType
        {
            get { return new Dictionary<string, string>(); }
        }

        /// <summary>
        /// Field custom properties of type
        /// </summary>
        public override Dictionary<string, Dictionary<string, string>> FieldsCustomPropertiesOfType
        {
            get { return new Dictionary<string, Dictionary<string, string>>(); }
        }

        /// <summary>
        /// Set related entity property
        /// </summary>
        public override void SetRelatedEntityProperty(string propertyName, IEntity2 entity)
        {

        }

        /// <summary>
        /// Get all relations
        /// </summary>
        public override List<IEntityRelation> GetAllRelations()
        {
            return new List<IEntityRelation>();
        }

        /// <summary>
        /// Get relations for field of type
        /// </summary>
        public override RelationCollection GetRelationsForFieldOfType(string fieldName)
        {
            return new RelationCollection();
        }

        /// <summary>
        /// Get related data
        /// </summary>
        public override Dictionary<string, object> GetRelatedData()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Set related entity
        /// </summary>
        public override void SetRelatedEntity(IEntity2 relatedEntity, string fieldName)
        {

        }

        /// <summary>
        /// Unset related entity
        /// </summary>
        public override void UnsetRelatedEntity(IEntity2 relatedEntity, string fieldName, bool signalRelatedEntityManyToOne)
        {

        }

        /// <summary>
        /// Get depending related entities
        /// </summary>
        public override List<IEntity2> GetDependingRelatedEntities()
        {
            return new List<IEntity2>();
        }

        /// <summary>
        /// Get dependent related entities
        /// </summary>
        public override List<IEntity2> GetDependentRelatedEntities()
        {
            return new List<IEntity2>();
        }

        /// <summary>
        /// Get member entity collections
        /// </summary>
        public override List<IEntityCollection2> GetMemberEntityCollections()
        {
            return new List<IEntityCollection2>();
        }

        /// <summary>
        /// Get the made up hash code
        /// </summary>
        public override int GetHashCode()
        {
            return hashCode;
        }

        /// <summary>
        /// Get the made up account id
        /// </summary>
        public long AccountId => -1;

        /// <summary>
        /// Get the made up description
        /// </summary>
        public string Description => string.Empty;

        /// <summary>
        /// Get the made up shipment type (none)
        /// </summary>
        public ShipmentTypeCode ShipmentType => ShipmentTypeCode.None;
    }
}
