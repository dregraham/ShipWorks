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
        /// Deletes all dirty objects inside the collection passed from the persistent
        /// storage. It will do this inside a transaction if a transaction is not yet
        /// available. Entities which are physically deleted from the persistent storage
        /// are marked with the state 'Deleted' but are not removed from the collection.
        /// </summary>
        int DeleteEntityCollection(IEntityCollection2 collectionToDelete);

        /// <summary>
        /// Saves the passed in entity to the persistent storage. Will <i>not</i> refetch the entity after this save.
        /// The entity will stay out-of-sync. If the entity is new, it will be inserted, if the entity is existent, the changed
        /// entity fields will be changed in the database. Will do a recursive save.
        /// Will pass the concurrency predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save) as update restriction.
        /// </summary>
        /// <param name="entityToSave">The entity to save</param>
        /// <returns>true if the save was succesful, false otherwise.</returns>
        /// <remarks>Will use a current transaction if a transaction is in progress</remarks>
        bool SaveEntity(IEntity2 entityToSave);

        bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave, bool recurse);
    }
}