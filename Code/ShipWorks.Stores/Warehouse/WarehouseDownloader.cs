using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Downloader for orders coming from ShipWorks Warehouse
    /// </summary>
    [Component]
    public class WarehouseDownloader : IWarehouseDownloader
    {
        private readonly IWarehouseOrderClient webClient;
        private readonly Func<StoreTypeCode, IWarehouseOrderFactory> orderLoaderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseDownloader(IWarehouseOrderClient webClient, Func<StoreTypeCode, IWarehouseOrderFactory> orderLoaderFactory)
        {
            this.webClient = webClient;
            this.orderLoaderFactory = orderLoaderFactory;
        }
        
        /// <summary>
        /// Downloads orders from ShipWorks Warehouse app for the given warehouse id
        /// </summary>
        public async Task Download(string warehouseID)
        {
            // start progress bar
            
            // get orders for this warehouse
            IEnumerable<WarehouseOrder> orders = await webClient.GetOrders(warehouseID).ConfigureAwait(false);
            
            // group orders by store type, so we don't constantly have to make new order loaders
            IEnumerable<IGrouping<int,WarehouseOrder>> ordersGroupedByStoreType = orders.GroupBy(x => x.StoreType);

            // load orders
            foreach (var warehouseOrderGroup in ordersGroupedByStoreType)
            {
                IWarehouseOrderFactory orderFactory = orderLoaderFactory((StoreTypeCode) warehouseOrderGroup.Key);

                foreach (WarehouseOrder warehouseOrder in warehouseOrderGroup)
                {
                    // load order
                    OrderEntity orderEntity = await orderFactory.CreateOrder(warehouseOrder).ConfigureAwait(false);
                    
                    // save order

                }
            }
            
            // finish progress
            
        }
    }
}