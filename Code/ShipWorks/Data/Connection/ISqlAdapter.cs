using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// A custom ShipWorks DataAccessAdapter
    /// </summary>
    public interface ISqlAdapter : IDataAccessAdapter
    {
        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        bool SaveAndRefetch(IEntity2 entity);

        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        Task<bool> SaveAndRefetchAsync(IEntity2 entity);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        /// Fetches the query specified on the adapter specified. Uses the TEntity type to
        /// produce an EntityCollection(Of TEntity) for the results to return
        /// </summary>
        Task<IEntityCollection2> FetchQueryAsync<T>(EntityQuery<T> query) where T : IEntity2;
        /// SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        /// Fetches the query specified on the adapter specified. Uses the TEntity type to
        /// produce an EntityCollection(Of TEntity) for the results to return
        /// </summary>
        List<TElement> FetchQuery<TElement>(DynamicQuery<TElement> query);

        /// <summary>
    }
}