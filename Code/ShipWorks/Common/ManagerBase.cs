using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Utility;

namespace ShipWorks.Common
{
    public abstract class ManagerBase<TEntity, TEntityInterface> where TEntity : EntityBase2
    {
        private TableSynchronizer<TEntity> tableSynchronizer;
        private bool needCheckForChanges;
        private ReadOnlyCollection<TEntityInterface> readOnlyEntities;

        /// <summary>
        /// All the entities
        /// </summary>
        protected IEnumerable<TEntity> Entities
        {
            get
            {
                lock (tableSynchronizer)
                {
                    if (needCheckForChanges)
                    {
                        CheckForChanges();
                    }

                    return tableSynchronizer.EntityCollection;
                }
            }
        }

        /// <summary>
        /// All the entities - ReadOnly
        /// </summary>
        protected IEnumerable<TEntityInterface> EntitiesReadOnly
        {
            get
            {
                lock (tableSynchronizer)
                {
                    if (needCheckForChanges)
                    {
                        CheckForChanges();
                    }

                    return readOnlyEntities;
                }
            }
        }

        /// <summary>
        /// Remove an existing entity from the DB
        /// </summary>
        public void Delete(TEntity toDelete, ISqlAdapter adapter)
        {
            adapter.DeleteEntity(toDelete);
            CheckForChangesNeeded();
        }

        /// <summary>
        /// Save a new entity to the DB
        /// </summary>
        public void Save(TEntity toSave, ISqlAdapter adapter)
        {
            adapter.SaveAndRefetch(toSave);
            CheckForChangesNeeded();
        }

        /// <summary>
        /// Direct manager to update from the database before entities are returned
        /// </summary>
        public void CheckForChangesNeeded()
        {
            lock (tableSynchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Initializes the object
        /// </summary>
        public void InitializeForCurrentSession()
        {
            tableSynchronizer = new TableSynchronizer<TEntity>();
            readOnlyEntities = tableSynchronizer.EntityCollection.Select(AsReadOnly).ToReadOnly();
            CheckForChanges();
        }

        /// <summary>
        /// Update local version from the database
        /// </summary>
        private void CheckForChanges()
        {
            lock (tableSynchronizer)
            {
                if (tableSynchronizer.Synchronize())
                {
                    readOnlyEntities = tableSynchronizer.EntityCollection.Select(AsReadOnly).ToReadOnly();
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Method that gets the readonly version of the entity
        /// </summary>
        protected abstract TEntityInterface AsReadOnly(TEntity entity);
    }
}