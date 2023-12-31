﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Orders.Combine.SearchProviders
{
    /// <summary>
    /// Base combined order search provider.
    /// </summary>
    /// <typeparam name="TResult">Type of the resulting order identifier</typeparam>
    [SuppressMessage("SonarLint", "S3358: Ternary operators should not be nested",
            Justification = "This is legacy code. If there's time, we can address the issue.")]
    public abstract class CombineOrderSearchBaseProvider<TResult> : ICombineOrderSearchProvider<TResult>
    {
        protected readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        protected CombineOrderSearchBaseProvider(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Gets the online store's order identifier entities.
        /// </summary>
        protected virtual async Task<IEnumerable<TEntity>> GetCombinedOnlineOrderIdentifiers<TEntity>(
            IPredicate predicate) where TEntity : IEntity2
        {
            QueryFactory factory = new QueryFactory();
            var query = factory.Create<TEntity>()
                .Where(predicate)
                .Distinct();

            if (typeof(TEntity) == typeof(OrderSearchEntity))
            {
                query = query.AndWhere(OrderSearchFields.IsManual == false);
            }

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false) as IEnumerable<TEntity>;
            }
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
                .Distinct()
                .Where(predicate);

            if (typeof(TEntity) == typeof(OrderSearchEntity))
            {
                query = query.AndWhere(OrderSearchFields.IsManual == false);
            }

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
            return order.CombineSplitStatus != CombineSplitStatusType.None ?
                await GetCombinedOnlineOrderIdentifiers(order).ConfigureAwait(false) :
                order.IsManual ? Enumerable.Empty<TResult>() : new[] { GetOnlineOrderIdentifier(order) };
        }
    }
}
