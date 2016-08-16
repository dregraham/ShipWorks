using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Keeps an in memory entity list in sync with a database table
    /// </summary>
    public class TableSynchronizer<TEntity> where TEntity : EntityBase2, IEntity2
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TableSynchronizer<TEntity>));

        // The collection we maintain
        EntityCollection<TEntity> collection = new EntityCollection<TEntity>();

        // The synchronizer
        EntityChangeTrackingMonitor changeMonitor = null;

        // Properties of our table
        EntityField2 primaryKeyField;

        // Allow overwriting of edited entities
        bool allowOverwriteOfEdited = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TableSynchronizer()
        {
            primaryKeyField = (EntityField2) collection.EntityFactoryToUse.CreateFields()[0];
        }

        /// <summary>
        /// The entity collection that is kept synchronized with the database
        /// </summary>
        public EntityCollection<TEntity> EntityCollection
        {
            get { return collection; }
        }

        /// <summary>
        /// If false, entities that have IsDirty set to true will not be able to be overwritten with
        /// changes from the database.  Instead an exception will be thrown.
        /// </summary>
        public bool AllowOverwriteOfEdited
        {
            get { return allowOverwriteOfEdited; }
            set { allowOverwriteOfEdited = value; }
        }

        /// <summary>
        /// Synchronize the contents of the in memory collection with what is in the database
        /// </summary>
        public bool Synchronize()
        {
            return Synchronize(null, null);
        }

        /// <summary>
        /// Synchronize the contents of the in memory collection with what is in the database
        /// </summary>
        [NDependIgnoreLongMethod]
        public bool Synchronize(List<TEntity> modified, List<TEntity> added)
        {
            if (changeMonitor == null)
            {
                changeMonitor = new EntityChangeTrackingMonitor();
                changeMonitor.Initialize(new List<EntityType> { EntityUtility.GetEntityType(typeof(TEntity)) });

                using (SqlAdapter adapter = SqlAdapter.Create(false))
                {
                    adapter.FetchEntityCollection(collection, null);

                    if (added != null)
                    {
                        added.AddRange(collection);
                    }
                }

                return collection.Count > 0;
            }
            else
            {
                EntityChangeTrackingChangeset changeset = changeMonitor.CheckForChanges()[0];

                if (!changeset.IsValid)
                {
                    // If the changeset is invalid - we have to start over from scratch
                    using (SqlAdapter adapter = SqlAdapter.Create(false))
                    {
                        collection.Clear();

                        adapter.FetchEntityCollection(collection, null);

                        if (added != null)
                        {
                            added.AddRange(collection);
                        }
                    }

                    return true;
                }
                else if (!changeset.HasChanges)
                {
                    return false;
                }
                else
                {
                    bool hasChanges = RemoveDeletions(changeset.Deletes);

                    RelationPredicateBucket bucket = new RelationPredicateBucket();
                    bucket.PredicateExpression.Add(primaryKeyField == changeset.Inserts | primaryKeyField == changeset.Updates);

                    // Get all the changed entities and new entities
                    EntityCollection<TEntity> changeCollection = new EntityCollection<TEntity>();
                    using (SqlAdapter adapter = SqlAdapter.Create(false))
                    {
                        adapter.FetchEntityCollection(changeCollection, bucket);
                    }

                    // Go through each entity that changed
                    foreach (TEntity entity in changeCollection)
                    {
                        hasChanges = true;

                        TEntity modifiedEntity = MergeEntity(entity);
                        if (modifiedEntity != null)
                        {
                            if (modified != null)
                            {
                                modified.Add(modifiedEntity);
                            }
                        }
                        else
                        {
                            if (added != null)
                            {
                                added.Add(entity);
                            }
                        }
                    }
                    return hasChanges;
                }
            }
        }

        /// <summary>
        /// Merges the entity.
        /// </summary>
        /// <returns> Returns the updated entity if updating else, returns null.</returns>
        /// <exception cref="System.InvalidOperationException">Cannot overwrite changes to a dirty entity when synchronizing.</exception>
        public TEntity MergeEntity(TEntity entity)
        {
            // Try to find the entity in our existing collection
            IEntityField2 pkField = entity.Fields.PrimaryKeyFields[0];
            List<int> matches = collection.FindMatches(new FieldCompareValuePredicate(pkField, null, ComparisonOperator.Equal, pkField.CurrentValue));

            // Its already in the collection
            if (matches.Count == 1)
            {
                TEntity existing = collection[matches[0]];

                if (!AllowOverwriteOfEdited && existing.IsDirty)
                {
                    throw new InvalidOperationException("Cannot overwrite changes to a dirty entity when synchronizing.");
                }

                foreach (IEntityField2 field in entity.Fields)
                {
                    if (!field.IsPrimaryKey)
                    {
                        object newValue = entity.Fields[field.FieldIndex].CurrentValue;

                        if (!field.IsReadOnly)
                        {
                            // First, we use this to ensure proper eventing and propagation
                            existing.SetNewFieldValue(field.FieldIndex, newValue);
                        }

                        // Then we use this to set the original value
                        existing.Fields[field.FieldIndex].ForcedCurrentValueWrite(newValue, newValue);
                        existing.Fields[field.FieldIndex].IsNull = (newValue == null);

                        // Then, we need to mark it as not changed
                        existing.Fields[field.FieldIndex].IsChanged = false;
                    }
                }

                existing.IsDirty = false;
                return existing;
            }

            // It's not already in the collection, we have to add it
            collection.Add(entity);
            return null;
        }

        /// <summary>
        /// Check for any deletions, and update the given entity list to reflect them.
        /// </summary>
        private bool RemoveDeletions(IEnumerable<long> deleted)
        {
            List<IEntity2> toRemove = new List<IEntity2>();

            foreach (IEntity2 entity in collection)
            {
                if (deleted.Contains((long) entity.PrimaryKeyFields[0].CurrentValue))
                {
                    toRemove.Add(entity);
                }
            }

            // Remove all the ones that need removed
            foreach (IEntity2 entity in toRemove)
            {
                collection.Remove((TEntity) entity);

                // If the entity has FK fields, where LLBLgen generates a reference to an actual object, we want
                // that reference removed.  This accomplishes that.
                foreach (IEntityField2 field in entity.Fields)
                {
                    if (field.IsForeignKey)
                    {
                        entity.SetNewFieldValue(field.FieldIndex, null);
                    }
                }
            }

            return toRemove.Count > 0;
        }
    }
}
