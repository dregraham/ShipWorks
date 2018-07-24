using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Overstock.OnlineUpdating
{
    /// <summary>
    /// Data access for uploading orders to Overstock
    /// </summary>
    [Component]
    public class OverstockDataAccess : IOverstockDataAccess
    {
        private readonly IDataProvider dataProvider;
        private readonly IOverstockOrderSearchProvider searchProvider;
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockDataAccess(IDataProvider dataProvider, IOverstockOrderSearchProvider searchProvider, IOrderManager orderManager)
        {
            this.orderManager = orderManager;
            this.searchProvider = searchProvider;
            this.dataProvider = dataProvider;
        }

        /// <summary>
        /// Get order details for uploading
        /// </summary>
        public async Task<IEnumerable<OverstockSupplierShipment>> GetOrderDetails(IShipmentEntity shipmentEntity)
        {
            var order = await dataProvider.GetEntityAsync<OverstockOrderEntity>(shipmentEntity.OrderID).ConfigureAwait(false);
            orderManager.PopulateOrderDetails(order);
            var details = await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            return details
                .GroupJoin(
                    order.OrderItems.OfType<IOverstockOrderItemEntity>(),
                    x => x.OriginalOrderID,
                    x => x.OriginalOrderID,
                    (orderDetail, items) => items.Select(item => new OverstockSupplierShipment(orderDetail, shipmentEntity, item)))
                .Flatten();
        }
    }
}