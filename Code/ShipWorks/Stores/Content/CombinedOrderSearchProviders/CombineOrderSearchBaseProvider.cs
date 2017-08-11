using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using Interapptive.Shared.Enums;

namespace ShipWorks.Stores.Content.CombinedOrderSearchProviders
{
    /// <summary>
    /// Base combined order search provider.
    /// </summary>
    /// <typeparam name="TResult">Type of the resulting order identifier</typeparam>
    public abstract class CombineOrderSearchBaseProvider<TResult> : ICombineOrderSearchProvider<TResult>
    {
        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <typeparam name="TResult">Return type of the selectExpression</typeparam>
        /// <param name="order">The order for which to find combined order identifiers</param>
        /// <param name="searchTableName">Name of the table, i.e. "OrderSearch"</param>
        /// <param name="predicate">Where clause predicate, i.e. OrderSearchFields.OrderID == order.OrderID</param>
        /// <param name="selectExpression">What to return, i.e. , () => OrderSearchFields.OrderNumber.ToValue<long>()</param>
        /// <returns></returns>
        protected virtual async Task<IEnumerable<TResult>> GetCombinedOnlineOrderIdentifiers(
            OrderEntity order,
            string searchTableName,
            IPredicate predicate,
            Expression<Func<TResult>> selectExpression)
        {
            QueryFactory factory = new QueryFactory();
            var query = factory.Create(searchTableName)
                .Select(selectExpression)
                .Where(predicate);

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                using (ISqlAdapter sqlAdapter = lifetimeScope.Resolve<ISqlAdapterFactory>().Create())
                {
                    return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                }
            }
        }

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
                new[] { GetOnlineOrderIdentifier(order) };
        }
    }
}
