using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.FactoryClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Filters;
using ShipWorks.Data.Adapter.Custom;
using System.Diagnostics;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Adapter;
using System.Drawing;
using ShipWorks.Properties;
using ShipWorks.Shipping;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using ShipWorks.Templates;

namespace ShipWorks.Data
{
    /// <summary>
    /// Utility functions for working with LLBLGen entities
    /// </summary>
    public static class EntityUtility
    {
        #region InternalDataAdapter

        class InternalDataAdapter : DataAccessAdapter
        {
            /// <summary>
            /// Get the collection of all relations for the given entity
            /// </summary>
            public static void ApplyPersistanceInfoObjects(RelationCollection relations)
            {
                using (InternalDataAdapter adapter = new InternalDataAdapter())
                {
                    adapter.InsertPersistenceInfoObjects(relations);
                }
            }
        }

        #endregion

        static Dictionary<EntityType, int> entitySeedValues = new Dictionary<EntityType, int>();

        /// <summary>
        /// Static constructor
        /// </summary>
        static EntityUtility()
        {
            entitySeedValues[EntityType.ComputerEntity] = 1;
            entitySeedValues[EntityType.UserEntity] = 2;
            entitySeedValues[EntityType.AuditChangeEntity] = 3;
            entitySeedValues[EntityType.StoreEntity] = 5;
            entitySeedValues[EntityType.OrderEntity] = 6;
            entitySeedValues[EntityType.FilterNodeEntity] = 7;
            entitySeedValues[EntityType.CustomerEntity] = 12;
            entitySeedValues[EntityType.OrderItemEntity] = 13;
            entitySeedValues[EntityType.DownloadEntity] = 18;
            entitySeedValues[EntityType.OrderItemAttributeEntity] = 20;
            entitySeedValues[EntityType.OrderChargeEntity] = 21;
            entitySeedValues[EntityType.OrderPaymentDetailEntity] = 23;
            entitySeedValues[EntityType.TemplateEntity] = 25;
            entitySeedValues[EntityType.ResourceEntity] = 26;
            entitySeedValues[EntityType.ShipmentEntity] = 31;
            entitySeedValues[EntityType.EmailOutboundEntity] = 35;
            entitySeedValues[EntityType.ActionQueueEntity] = 41;
            entitySeedValues[EntityType.ActionTaskEntity] = 42;
            entitySeedValues[EntityType.ActionQueueStepEntity] = 43;
            entitySeedValues[EntityType.NoteEntity] = 44;
            entitySeedValues[EntityType.PrintResultEntity] = 45;
            entitySeedValues[EntityType.AuditChangeDetailEntity] = 47;
            entitySeedValues[EntityType.AuditEntity] = 48;
            entitySeedValues[EntityType.ShippingOriginEntity] = 50;
            entitySeedValues[EntityType.ShipmentCustomsItemEntity] = 51;
            entitySeedValues[EntityType.UspsAccountEntity] = 52;
            entitySeedValues[EntityType.FedExAccountEntity] = 55;
            entitySeedValues[EntityType.UpsAccountEntity] = 56;
            entitySeedValues[EntityType.FedExPackageEntity] = 61;
            entitySeedValues[EntityType.EndiciaAccountEntity] = 66;
            entitySeedValues[EntityType.ScanFormBatchEntity] = 95;
            entitySeedValues[EntityType.ServiceStatusEntity] = 96;
        }

        /// <summary>
        /// Get the primary key field of the given entity type
        /// </summary>
        public static EntityField2 GetPrimaryKeyField(EntityType entityType)
        {
            IEntityFields2 fields = EntityFieldsFactory.CreateEntityFieldsObject(entityType);
            return (EntityField2) fields.PrimaryKeyFields[0];
        }

        /// <summary>
        /// Get the EntityType based on its ID.  This function can work because in the database we partition
        /// our pk values by using unique initial seed values, and seed increments of 1000.
        /// </summary>
        public static EntityType GetEntityType(long entityID)
        {
            int seedBase = GetEntitySeed(entityID);

            List<EntityType> entityType = entitySeedValues.Where(p => p.Value == seedBase).Select(p => p.Key).ToList();
            if (entityType.Count != 1)
            {
                throw new InvalidOperationException(string.Format("Unhandled entityID in GetEntityType. {0}, {1}", entityID, seedBase));
            }

            return entityType[0];
        }

        /// <summary>
        /// Get the seed value used for the given EnityType
        /// </summary>
        public static int GetEntitySeed(EntityType entityType)
        {
            int seed;
            if (!entitySeedValues.TryGetValue(entityType, out seed))
            {
                throw new InvalidOperationException(string.Format("Unhandled entityType in GetEntitySeed. {0}", entityType));
            }

            return seed;
        }

        /// <summary>
        /// Get the seed value that can be used to identifiy the specified entity.  Each table in the database has a unique starting seed value.
        /// </summary>
        public static int GetEntitySeed(long entityID)
        {
            decimal num = (decimal) Math.Abs(entityID) / 100m;
            int seedBase = (int) (100m * Math.Round(num - Math.Truncate(num), 2));

            return seedBase;
        }

        /// <summary>
        /// Indicates if ShipWorks is tracking the seed information for the given EntityType
        /// </summary>
        public static bool HasEntitySeedInfo(EntityType entityType)
        {
            return entitySeedValues.ContainsKey(entityType);
        }

        /// <summary>
        /// Indicates if ShipWorks is tracking seed information for and can determine the EntityType of the given key
        /// </summary>
        public static bool HasEntitySeedInfo(long entityID)
        {
            return entitySeedValues.ContainsValue(GetEntitySeed(entityID));
        }

        /// <summary>
        /// Get the EntityType based on the given SystemType
        /// </summary>
        public static EntityType GetEntityType(Type systemType)
        {
            return EntityTypeProvider.GetEntityType(((EntityBase2) Activator.CreateInstance(systemType)).Fields[0].ActualContainingObjectName);
        }

        /// <summary>
        /// Clone the given entity using a deep clone.
        /// </summary>
        public static T CloneEntity<T>(T entity) where T : EntityBase2
        {
            return CloneEntity(entity, true);
        }

        /// <summary>
        /// Clone the given entity.  If deep is true, a clone of each reference Entity is made, recursively.
        /// </summary>
        public static T CloneEntity<T>(T entity, bool deep) where T : EntityBase2
        {
            if (entity == null)
            {
                return null;
            }

            if (deep)
            {
                SerializationHelper.Optimization = SerializationOptimization.Fast;
                SerializationHelper.PreserveObjectIDs = false;

                using (MemoryStream memoryStream = new MemoryStream(1284))
                {
                    BinaryFormatter formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));

                    formatter.Serialize(memoryStream, entity);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    T clone = (T) formatter.Deserialize(memoryStream);

                    return clone;
                }
            }
            else
            {
                T clone = (T) GeneralEntityFactory.Create((EntityType) entity.LLBLGenProEntityTypeValue);
                clone.Fields = entity.Fields.Clone();
                clone.IsNew = entity.IsNew;
                clone.IsDirty = entity.IsDirty;

                return clone;
            }
        }

        /// <summary>
        /// Clone the given entity collection using a deep clone.  
        /// </summary>
        public static List<T> CloneEntityCollection<T>(IEnumerable<T> collection) where T : EntityBase2
        {
            return CloneEntityCollection(collection, true);
        }

        /// <summary>
        /// Clone the given entity collection.  If deep is true, a clone of each reference Entity is made, recursively.
        /// </summary>
        public static List<T> CloneEntityCollection<T>(IEnumerable<T> collection, bool deep) where T : EntityBase2
        {
            if (collection == null)
            {
                return null;
            }

            List<T> cloned = new List<T>();

            foreach (T entity in collection)
            {
                cloned.Add(CloneEntity(entity, deep));
            }

            return cloned;
        }

        /// <summary>
        /// Determine if the two field objects represent the same field.
        /// </summary>
        public static bool IsSameField(IEntityFieldCore left, IEntityFieldCore right)
        {
            if ((object) left == null || (object) right == null)
            {
                return false;
            }

            if (left.ActualContainingObjectName == right.ActualContainingObjectName &&
                left.FieldIndex == right.FieldIndex)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the value of the given field for the specified entity.  If the field does not belong to the entity (like a Yahoo field and an eBay entity),
        /// then return null.
        /// </summary>
        public static object GetFieldValue(EntityBase2 entity, EntityField2 field)
        {
            EntityType fieldType = EntityTypeProvider.GetEntityType(field.ContainingObjectName);
            EntityType entityType = EntityTypeProvider.GetEntityType(entity.GetEntityFactory().ForEntityName);

            // If the field entity type does not match the given entity type, return a null value.  This is a legitamate situation,
            // for example when we have both eBay and Yahoo stores, a Yahoo entity could be passed in, but wants the data for an
            // ebay specific column.
            if (fieldType != entityType)
            {
                // If the entity type derives from the field type, that's OK.  Like if its an OsCommerceOrderEntity, but the field is from plain OrderEntity.
                IEntity2 fieldEntity = GeneralEntityFactory.Create(fieldType);
                if (!entity.GetType().IsSubclassOf(fieldEntity.GetType()))
                {
                    return null;
                }
            }

            EntityField2 entityField = (EntityField2) entity.Fields[field.Name];

            if ((object) entityField == null)
            {
                return null;
            }

            return entityField.CurrentValue;
        }


        /// <summary>
        /// Find a chain of relations that goes from the given entity to the given entity. OneToMany relationships are considered.
        /// Returns null if no such chain is found. Many to Many relationships are never considered.
        /// </summary>
        public static RelationCollection FindRelationChain(EntityType fromEntityType, EntityType toEntityType)
        {
            return FindRelationChain(fromEntityType, toEntityType, true);
        }

        /// <summary>
        /// Find a chain of relations that goes from the given entity to the given entity. Returns null if no such chain is found.
        /// Many to Many relationships are never considered.
        /// </summary>
        public static RelationCollection FindRelationChain(EntityType fromEntityType, EntityType toEntityType, bool allowOneToMany)
        {
            // Try it bottom-to-top with the given entities
            RelationCollection relations = FindRelationChain(fromEntityType, toEntityType, new List<EntityType>());

            // Try the other direction
            if (relations == null)
            {
                relations = FindRelationChain(toEntityType, fromEntityType, new List<EntityType>());

                // Put them in order from "from" to "to"
                if (relations != null)
                {
                    List<IEntityRelation> reversedList = relations.Cast<IEntityRelation>().Reverse().ToList();
                    relations = new RelationCollection();

                    foreach (EntityRelation relation in reversedList)
                    {
                        // Find the 'reversed' version of this relation.  What was the end, we now want to be the start
                        EntityType reverseEntityType = EntityTypeProvider.GetEntityType(relation.StartEntityIsPkSide ? relation.GetFKEntityFieldCore(0).ContainingObjectName : relation.GetPKEntityFieldCore(0).ContainingObjectName);
                        EntityBase2 reverseEntity = (EntityBase2) GeneralEntityFactory.Create(reverseEntityType);

                        EntityRelation reverseRelation = null;
                        IInheritanceInfo inheritanceInfo = reverseEntity.GetInheritanceInfo();

                        // Check all the standard relations
                        List<IEntityRelation> relationsToCheck = reverseEntity.GetAllRelations();

                        if (inheritanceInfo != null)
                        {
                            // Check the relations to types it derives from (like EBayOrder -> Order)
                            if (inheritanceInfo.RelationsToHierarchyRoot != null)
                            {
                                relationsToCheck.AddRange(inheritanceInfo.RelationsToHierarchyRoot.Cast<IEntityRelation>().ToList());
                            }

                            // Check the relations of subtypes (like Order -> EBayOrder)
                            IRelationFactory relationFactory = EntityTypeProvider.GetInheritanceRelationFactory(reverseEntityType);
                            foreach (string subTypeEntityName in inheritanceInfo.EntityNamesOfPathsToLeafs)
                            {
                                EntityRelation subTypeRelation = (EntityRelation) relationFactory.GetSubTypeRelation(subTypeEntityName);
                                if (subTypeRelation != null)
                                {
                                    relationsToCheck.Add(subTypeRelation);
                                }
                            }
                        }

                        foreach (EntityRelation testRelation in relationsToCheck)
                        {
                            if (testRelation.StartEntityIsPkSide != relation.StartEntityIsPkSide &&
                                 EntityUtility.IsSameField(testRelation.GetPKEntityFieldCore(0), relation.GetPKEntityFieldCore(0)) &&
                                 EntityUtility.IsSameField(testRelation.GetFKEntityFieldCore(0), relation.GetFKEntityFieldCore(0)))
                            {
                                if (reverseRelation != null)
                                {
                                    throw new InvalidOperationException("Found multiple reverse relations.");
                                }

                                reverseRelation = testRelation;
                            }
                        }

                        // Try moving along the inheritance axis

                        if (reverseRelation == null)
                        {
                            throw new InvalidOperationException("Could not find reverse relation.");
                        }

                        relations.Add(reverseRelation);
                    }
                }
            }

            if (relations != null)
            {
                // Now we have to verify there aren't any one to many relationships involved
                if (!allowOneToMany)
                {
                    foreach (EntityRelation relation in relations)
                    {
                        if (relation.TypeOfRelation == RelationType.ManyToMany || relation.TypeOfRelation == RelationType.OneToMany)
                        {
                            return null;
                        }
                    }
                }

                InternalDataAdapter.ApplyPersistanceInfoObjects(relations);
            }

            return relations;
        }

        /// <summary>
        /// Check our set of known relations for a relation with the given properties
        /// </summary>
        private static RelationCollection CheckKnownRelations(EntityType fromEntity, EntityType toEntity)
        {
            if (fromEntity == EntityType.CustomerEntity && toEntity == EntityType.StoreEntity)
            {
                RelationCollection customerToStore = new RelationCollection(CustomerEntity.Relations.OrderEntityUsingCustomerID);
                customerToStore.Add(OrderEntity.Relations.StoreEntityUsingStoreID);

                return customerToStore;
            }

            if (fromEntity == EntityType.ShipmentEntity && toEntity == EntityType.OrderItemEntity)
            {
                RelationCollection shipmentToItems = new RelationCollection(ShipmentEntity.Relations.OrderEntityUsingOrderID);
                shipmentToItems.Add(OrderEntity.Relations.OrderItemEntityUsingOrderID);

                return shipmentToItems;
            }

            if (fromEntity == EntityType.OrderEntity && toEntity == EntityType.NoteEntity)
            {
                EntityRelation relation = new EntityRelation(OrderFields.OrderID, NoteFields.ObjectID, RelationType.OneToMany, true, string.Empty);
                return new RelationCollection(relation);
            }

            if (fromEntity == EntityType.CustomerEntity && toEntity == EntityType.NoteEntity)
            {
                EntityRelation relation = new EntityRelation(CustomerFields.CustomerID, NoteFields.ObjectID, RelationType.OneToMany, true, string.Empty);
                return new RelationCollection(relation);
            }

            return null;
        }

        /// <summary>
        /// Find a chain of relations that goes from the given entity to the given entity. Returns null if no such chain is found.
        /// </summary>
        private static RelationCollection FindRelationChain(EntityType fromEntityType, EntityType toEntityType, List<EntityType> visitedEntityTypes)
        {
            // If this is the type we are looking for, we are done.  Just return an empty relation collection (since no relation is needed to get from an
            // entity to itself.
            if (fromEntityType == toEntityType)
            {
                return new RelationCollection();
            }

            // We have either already visited, or are in the process of visiting this entity type via recursion.  Just return a not-found.  If
            // we are in the middle of visiting it via recursion, the first one can still return the valid result.
            if (visitedEntityTypes.Contains(fromEntityType))
            {
                return null;
            }
            else
            {
                visitedEntityTypes.Add(fromEntityType);
            }

            // Check for known relations
            {
                RelationCollection relations = CheckKnownRelations(fromEntityType, toEntityType);
                if (relations != null)
                {
                    // If any of the relations revisit one of our visited, that violates how we use visited, and probably generated a longer, and invalid relation path.  The first 
                    // one will obviously contain the "fromType" since that's what we are coming from - so we skip that.
                    foreach (EntityRelation relation in relations.Cast<EntityRelation>().Skip(1))
                    {
                        EntityType pkType = EntityTypeProvider.GetEntityType(relation.GetPKEntityFieldCore(0).ContainingObjectName);
                        EntityType fkType = EntityTypeProvider.GetEntityType(relation.GetFKEntityFieldCore(0).ContainingObjectName);

                        EntityType startType = relation.StartEntityIsPkSide ? pkType : fkType;

                        if (!visitedEntityTypes.Contains(startType))
                        {
                            visitedEntityTypes.Add(startType);
                        }
                        else
                        {
                            relations = null;
                            break;
                        }
                    }

                    if (relations != null)
                    {
                        return relations;
                    }
                }
            }

            EntityBase2 fromEntity = (EntityBase2) GeneralEntityFactory.Create(fromEntityType);

            // We will need to check inheritance info
            IInheritanceInfo inheritanceInfo = fromEntity.GetInheritanceInfo();

            // First check to see if we can get directly to a super-type (a type we are derived from)
            if (inheritanceInfo != null)
            {
                RelationCollection relationsToSuper = inheritanceInfo.RelationsToHierarchyRoot;
                if (relationsToSuper != null && relationsToSuper.Count > 0)
                {
                    // There may be more than one, but we can just start with the top - and it would then check it's top
                    RelationCollection relations = CheckRelationForChainTo((EntityRelation) relationsToSuper[0], toEntityType, visitedEntityTypes);
                    if (relations != null)
                    {
                        return relations;
                    }
                }
            }

            // Then check  all standard relations
            RelationCollection foundRelations = null;
            foreach (EntityRelation relation in fromEntity.GetAllRelations())
            {
                RelationCollection relations = CheckRelationForChainTo(relation, toEntityType, visitedEntityTypes);
                if (relations != null)
                {
                    if (foundRelations != null)
                    {
                        throw new InvalidOperationException(string.Format("Multiple relations found from {0} to {1}", fromEntityType, toEntityType));
                    }

                    foundRelations = relations;
                }
            }

            if (foundRelations != null)
            {
                return foundRelations;
            }

            // Now check the inheritance chain that goes from this entity to its derived types
            if (inheritanceInfo != null)
            {
                IRelationFactory relationFactory = EntityTypeProvider.GetInheritanceRelationFactory(fromEntityType);

                foreach (string subTypeEntityName in inheritanceInfo.EntityNamesOfPathsToLeafs)
                {
                    EntityRelation relation = (EntityRelation) relationFactory.GetSubTypeRelation(subTypeEntityName);

                    if (relation != null)
                    {
                        RelationCollection relations = CheckRelationForChainTo(relation, toEntityType, visitedEntityTypes);
                        if (relations != null)
                        {
                            return relations;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Check the given relation to see if it has an eventual chain of relations to the specified entity type.  If it does, it and its set of required
        /// relations to get there is returned.
        /// </summary>
        private static RelationCollection CheckRelationForChainTo(EntityRelation relation, EntityType toEntityType, List<EntityType> visitedEntityTypes)
        {
            string pkObject = relation.GetPKEntityFieldCore(0).ContainingObjectName;
            string fkObject = relation.GetFKEntityFieldCore(0).ContainingObjectName;

            string relationStartObject = relation.StartEntityIsPkSide ? pkObject : fkObject;
            string relationEndObject = relation.StartEntityIsPkSide ? fkObject : pkObject;

            // We can only move up or sideways - not down to multi records.
            if (relation.StartEntityIsPkSide && relation.TypeOfRelation != RelationType.OneToOne)
            {
                return null;
            }

            // Determine what entity the target of this relation is
            EntityType relatesToType = EntityTypeProvider.GetEntityType(relationEndObject);

            RelationCollection relations = FindRelationChain(relatesToType, toEntityType, visitedEntityTypes);

            if (relations != null)
            {
                // We are adding these in as we recurse backwards up the stack, so to keep them in order we have to insert them
                // in the beginning
                relations.Insert(relation, 0);
                return relations;
            }

            return null;
        }

        /// <summary>
        /// Get a 16x16 image representing the given entity
        /// </summary>
        public static Image GetEntityImage(long entityID)
        {
            return GetEntityImage(entityID, 16);
        }

        /// <summary>
        /// Get an image of the given entity of the given size
        /// </summary>
        public static Image GetEntityImage(long entityID, int size)
        {
            // If its a template, we can get more specific than the EntityType version
            if (GetEntityType(entityID) == EntityType.TemplateEntity)
            {
                // If we need 32, just add them to resources, and update TemplateHelper.GetTemplateImage
                if (size != 16)
                {
                    throw new ArgumentException("size for templates must be 16", "size");
                }

                TemplateEntity template = TemplateManager.Tree.GetTemplate(entityID);

                if (template != null)
                {
                    return TemplateHelper.GetTemplateImage(template);
                }
                else
                {
                    // Special case for preview, which uses 25 - which we would never give out as a real ID since we start at 1025.
                    if (entityID == 25)
                    {
                        return TemplateHelper.GetTemplateImage(TemplateType.Standard);
                    }

                    return Resources.template_deleted16;
                }
            }

            return GetEntityImage(GetEntityType(entityID), size);
        }
        
        /// <summary>
        /// Get a 16x16 image representing the given EntityType
        /// </summary>
        public static Image GetEntityImage(EntityType entityType)
        {
            return GetEntityImage(entityType, 16);
        }

        /// <summary>
        /// Get an image of the given entity of the given size
        /// </summary>
        public static Image GetEntityImage(EntityType entityType, int size)
        {
            if (size != 16 && size != 32)
            {
                throw new ArgumentException("size must be 16 or 32", "size");
            }

            switch (entityType)
            {
                case EntityType.StoreEntity:
                    return size == 16 ? Resources.school16 : Resources.school32;

                case EntityType.OrderEntity:
                    return size == 16 ? Resources.order16 : Resources.order32;

                case EntityType.CustomerEntity:
                    return size == 16 ? Resources.customer16 : Resources.customer32;

                case EntityType.ShipmentEntity:
                case EntityType.FedExPackageEntity:
                    return size == 16 ? Resources.box_closed16 : Resources.box_closed32;

                case EntityType.OrderItemEntity:
                    return size == 16 ? Resources.shoppingcart16 : Resources.shoppingcart32;
                
                case EntityType.OrderChargeEntity:
                    return size == 16 ? Resources.currency_dollar16 : Resources.currency_dollar32;

                case EntityType.OrderItemAttributeEntity:
                    return size == 16 ? Resources.paperclip16 : Resources.paperclip32;

                case EntityType.TemplateEntity:
                    return size == 16 ? Resources.template_general_16 : Resources.template_general_32;

                case EntityType.NoteEntity:
                    return size == 16 ? Resources.note16 : Resources.note32;

                default:
                    //Debug.Fail("EntityType " + entityType + " has no image.");
                    break;
            }

            return null;
        }

        /// <summary>
        /// Clone the given bucket.  The default implementation keeps causing nested PredicateExpressions for every clone.
        /// </summary>
        public static RelationPredicateBucket ClonePredicateBucket(IRelationPredicateBucket bucket)
        {
            if (bucket == null)
            {
                return null;
            }

            // First do the base clone
            RelationPredicateBucket clone = new RelationPredicateBucket();

            PredicateExpressionOperator nextOperator = PredicateExpressionOperator.And;

            // Add the expression elements
            foreach (IPredicateExpressionElement element in bucket.PredicateExpression)
            {
                if (element.Type == PredicateExpressionElementType.Predicate)
                {
                    if (nextOperator == PredicateExpressionOperator.And)
                    {
                        clone.PredicateExpression.AddWithAnd((IPredicate) element.Contents);
                    }
                    else
                    {
                        clone.PredicateExpression.AddWithOr((IPredicate) element.Contents);
                    }
                }

                if (element.Type == PredicateExpressionElementType.Operator)
                {
                    nextOperator = (PredicateExpressionOperator) element.Contents;
                }
            }

            // Do the relations
            clone.Relations.AddRange((RelationCollection) bucket.Relations);

            // Set properties (copied from the base code)
            clone.Relations.ObeyWeakRelations = bucket.Relations.ObeyWeakRelations;
            clone.SelectListAlias = bucket.SelectListAlias;

            return clone;
        }

        /// <summary>
        /// Mark the given entity, which may be in any state, as completely new
        /// </summary>
        public static void MarkAsNew(IEntity2 entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entity.IsNew = true;

            foreach (IEntityField2 field in entity.Fields)
            {
                field.IsChanged = true;
            }

            entity.GetDependingRelatedEntities().ForEach(e => MarkAsNew(e));

            entity.GetMemberEntityCollections().ForEach(c =>
                {
                    foreach (IEntity2 e2 in c)
                    {
                        MarkAsNew(e2);
                    }
                });
        }

        /// <summary>
        /// Gets the id of the entity as a long
        /// </summary>
        public static long GetEntityId(IEntity2 entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            List<IEntityField2> primaryKeyNotParent = entity.PrimaryKeyFields.Where(f => f.ActualContainingObjectName == f.ContainingObjectName).ToList();
            Debug.Assert(primaryKeyNotParent.Count() == 1, "GetEntityId cannot be used with entities that have compound primary keys");

            return (long)primaryKeyNotParent.Single().CurrentValue;
        }
    }
}
