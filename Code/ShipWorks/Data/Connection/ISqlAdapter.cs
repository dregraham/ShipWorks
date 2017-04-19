using System;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// A custom ShipWorks DataAccessAdapter
    /// </summary>
    public interface ISqlAdapter : IDisposable
    {
        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        bool SaveAndRefetch(IEntity2 entity);

        /// <summary>
        /// Transaction is committing
        /// </summary>
        void Commit();

        /// <summary>
        /// Fetch an entity from the database
        /// </summary>
        bool FetchEntity(IEntity2 entity);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket into the EntityCollection passed.
        /// The entity collection object has to contain an entity factory object which will be the factory for the entity instances
        /// to be fetched.
        /// This overload returns all found entities and doesn't apply sorting
        /// </summary>
        /// <param name="collectionToFill">EntityCollection object containing an entity factory which has to be filled</param>
        /// <param name="filterBucket">filter information for retrieving the entities. If null, all entities are returned of the type created by
        /// the factory in the passed in EntityCollection instance.</param>
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket);

        /// <summary>
        /// Deletes all dirty objects inside the collection passed from the persistent
        /// storage. It will do this inside a transaction if a transaction is not yet
        /// available. Entities which are physically deleted from the persistent storage
        /// are marked with the state 'Deleted' but are not removed from the collection.
        /// </summary>
        int DeleteEntityCollection(IEntityCollection2 collectionToDelete);

        /// <summary>
        /// Saves the entity.
        /// </summary>
        bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave, bool recurse);
    }
}