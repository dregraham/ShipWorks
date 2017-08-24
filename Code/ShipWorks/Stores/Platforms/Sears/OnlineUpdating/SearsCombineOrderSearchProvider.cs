using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using ShipWorks.Stores.Platforms.Sears.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Sears.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for orders
    /// </summary>
    [Component]
    public class SearsCombineOrderSearchProvider : CombineOrderSearchBaseProvider<SearsOrderDetail>, ISearsCombineOrderSearchProvider
    {
        readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<SearsOrderDetail>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.SearsOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(SearsOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => 
                        new SearsOrderDetail(
                            SearsOrderSearchFields.OrderID.ToValue<long>(),
                            SearsOrderSearchFields.PoNumber.ToValue<string>(),
                            order.OrderDate))
                .Where(SearsOrderSearchFields.OrderID == order.OrderID);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override SearsOrderDetail GetOnlineOrderIdentifier(IOrderEntity order)
        {
            ISearsOrderEntity searsOrder = order as ISearsOrderEntity;

            return new SearsOrderDetail(searsOrder.OrderID, searsOrder.PoNumber, order.OrderDate);
        }
    }
}
