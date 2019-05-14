using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Stores.Communication;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    [Component]
    public class WarehouseOrderClient : IWarehouseOrderClient
    {
        private readonly WarehouseRequestClient warehouseRequestClient;

        public WarehouseOrderClient(WarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestClient = warehouseRequestClient;
        }
        
        public async Task<IEnumerable<WarehouseOrder>> GetOrders(string warehouseID)
        {
            try
            {
                IRestRequest request = new RestRequest(WarehouseEndpoints.Orders(warehouseID), Method.POST);

                GenericResult<IRestResponse> response = await warehouseRequestClient
                    .MakeRequest(request, "Get Orders")
                    .ConfigureAwait(true);

                if (response.Failure)
                {
                    throw new DownloadException(response.Message, response.Exception);
                }
                
                IEnumerable<WarehouseOrder> orders = JsonConvert.DeserializeObject<IEnumerable<WarehouseOrder>>(response.Value.Content);

                return orders;
            }
            catch (Exception ex) when (ex.GetType() != typeof(DownloadException))
            {
                throw new DownloadException($"An error occured downloading orders for warehouse ID {warehouseID}", ex);
            }
        }
    }
}
