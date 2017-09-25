using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Newegg.OnlineUpdating
{
    /// <summary>
    /// Data access necessary for Newegg data uploads
    /// </summary>
    [Component]
    public class DataAccess : IDataAccess
    {
        private readonly INeweggCombineOrderSearchProvider orderNumberSearchProvider;
        private readonly IDataProvider dataProvider;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataAccess(IDataProvider dataProvider,
            INeweggCombineOrderSearchProvider orderNumberSearchProvider,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.dataProvider = dataProvider;
            this.orderNumberSearchProvider = orderNumberSearchProvider;
        }

        /// <summary>
        /// Load shipment details
        /// </summary>
        public async Task<ShipmentUploadDetails> LoadShipmentDetailsAsync(IShipmentEntity shipmentEntity)
        {
            var order = await dataProvider.GetEntityAsync<OrderEntity>(shipmentEntity.OrderID).ConfigureAwait(false);
            var identifiers = await orderNumberSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);
            var items = await GetItems(order).ConfigureAwait(false);

            return new ShipmentUploadDetails(order, identifiers, items);
        }

        /// <summary>
        /// Get items for an order
        /// </summary>
        private async Task<IDictionary<long, IEnumerable<ItemDetails>>> GetItems(IOrderEntity order)
        {
            var factory = new QueryFactory();
            var query = factory.NeweggOrderItem
                .Select(() => Tuple.Create(
                    OrderItemFields.OriginalOrderID.ToValue<long>(),
                    new ItemDetails(NeweggOrderItemFields.SellerPartNumber.ToValue<string>(), OrderItemFields.Quantity.ToValue<double>())))
                .Where(OrderItemFields.OrderID == order.OrderID);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                var items = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                return items.GroupBy(x => x.Item1).ToDictionary(x => x.Key, x => x.Select(y => y.Item2));
            }
        }
    }
}
