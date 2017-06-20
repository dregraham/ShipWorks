using System;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;

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
        /// Fetches a new entity using the filter/relation combination filter passed via filterBucket and the new entity is created using the specified generic type
        /// </summary>
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket bucket) where TEntity : EntityBase2, IEntity2, new();

        /// <summary>
        /// Gets a scalar value, calculated with the aggregate and expression specified.
        /// the field specified is the field the expression and aggregate are applied on.
        /// </summary>
        object GetScalar(IEntityField2 field, IExpression expressionToExecute, AggregateFunction aggregateToApply, IPredicate filter);

        /// <summary>
        /// Save the collection of entities to the database
        /// </summary>
        int SaveEntityCollection(IEntityCollection2 stores);

        /// <summary>
        /// Save the collection of entities to the database
        /// </summary>
        int SaveEntityCollection(IEntityCollection2 stores, bool refetchSavedEntitiesAfterSave, bool recurse);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        /// Fetches the query specified on the adapter specified. Uses the TEntity type to
        /// produce an EntityCollection(Of TEntity) for the results to return
        /// </summary>
        Task<IEntityCollection2> FetchQueryAsync<T>(EntityQuery<T> query) where T : IEntity2;

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched. This overload returns all found entities and doesn't apply sorting
        /// </summary>
        /// <param name="collectionToFill">EntityCollection object containing an entity factory which has to be filled</param>
        /// <param name="filterBucket">filter information for retrieving the entities. If null, all entities are returned 
        ///     of the type created by the factory in the passed in EntityCollection instance.</param>
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

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched. This overload returns all found entities and doesn't apply sorting
        /// </summary>
        /// <param name="collectionToFill">EntityCollection object containing an entity factory which has to be filled</param>
        /// <param name="filterBucket">filter information for retrieving the entities. If null, all entities are returned 
        ///     of the type created by the factory in the passed in EntityCollection instance.</param>
        /// <param name="maxRowsToReturn">Maximum number of rows to return</param>
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxRowsToReturn);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket into the EntityCollection passed.
        /// The entity collection object has to contain an entity factory object which will be the factory for the entity instances
        /// to be fetched.
        /// </summary>
        /// <param name="collectionToFill">EntityCollection object containing an entity factory which has to be filled</param>
        /// <param name="filterBucket">filter information for retrieving the entities. If null, all entities are returned of the type created by
        /// the factory in the passed in EntityCollection instance.</param>
        /// <param name="maxNumberOfItemsToReturn">The maximum amount of entities to return. If 0, all entities matching the filter are returned</param>
        /// <param name="sortClauses">SortClause expression which is applied to the query executed, sorting the fetch result.</param>
        /// <exception cref="T:System.ArgumentException">If the passed in collectionToFill doesn't contain an entity factory.</exception>
        void FetchEntityCollection(IEntityCollection2 collectionToFill,
            IRelationPredicateBucket filterBucket,
            int maxNumberOfItemsToReturn,
            ISortExpression sortClauses);
    }
}