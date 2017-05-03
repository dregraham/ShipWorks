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
        /// Gets a scalar value, calculated with the aggregate and expression specified.
        /// the field specified is the field the expression and aggregate are applied on.
        /// </summary>
        object GetScalar(IEntityField2 field, IExpression expressionToExecute, AggregateFunction aggregateToApply, IPredicate filter);

        /// <summary>
        /// Save the collection of entities to the database
        /// </summary>
        int SaveEntityCollection(IEntityCollection2 stores);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        /// Fetches the query specified on the adapter specified. Uses the TEntity type to
        /// produce an EntityCollection(Of TEntity) for the results to return
        /// </summary>
        Task<IEntityCollection2> FetchQueryAsync<T>(EntityQuery<T> query) where T : IEntity2;
    }
}