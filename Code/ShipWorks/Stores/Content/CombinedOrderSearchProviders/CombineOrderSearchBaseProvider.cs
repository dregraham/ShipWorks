using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;

namespace ShipWorks.Stores.Content.CombinedOrderSearchProviders
{
    /// <summary>
    /// Base combined order search provider.
    /// </summary>
    /// <typeparam name="TResult">Type of the resulting order identifier</typeparam>
    public abstract class CombineOrderSearchBaseProvider<TResult> : ICombineOrderSearchProvider<TResult>
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrderSearchBaseProvider(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Gets the online store's order identifiers, returning only those that are not manual orders.
        /// </summary>
        protected virtual async Task<IEnumerable<TResult>> GetCombinedOnlineOrderIdentifiers<TEntity>(
            IPredicate predicate, Expression<Func<TResult>> selectExpression) where TEntity : IEntityCore
        {
            QueryFactory factory = new QueryFactory();
            var query = factory.Create<TEntity>()
                .Select(selectExpression)
                .Where(predicate);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the online store's order identifiers, returning only those that are not manual orders.
        /// </summary>
        protected virtual Task<IEnumerable<TResult>> GetCombinedOnlineOrderIdentifiers<TEntity>(
            IPredicate predicate, IEntityFieldCore field) where TEntity : IEntityCore =>
            GetCombinedOnlineOrderIdentifiers<TEntity>(predicate, () => field.ToValue<TResult>());

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected abstract Task<IEnumerable<TResult>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order);

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected abstract TResult GetOnlineOrderIdentifier(IOrderEntity order);

        /// <summary>
        /// Get the order identifier(s) for the given order.  Multiple will be returned in the case of
        /// combined orders.
        /// </summary>
        public virtual async Task<IEnumerable<TResult>> GetOrderIdentifiers(IOrderEntity order)
        {
            return order.CombineSplitStatus == CombineSplitStatusType.Combined ?
                await GetCombinedOnlineOrderIdentifiers(order).ConfigureAwait(false) :
                order.IsManual ? Enumerable.Empty<TResult>() : new[] { GetOnlineOrderIdentifier(order) };
        }
    }
}
