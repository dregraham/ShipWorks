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
    /// <summary>
    /// Client for retrieving orders from the ShipWorks Warehouse app
    /// </summary>
    [Component]
    public class WarehouseOrderClient : IWarehouseOrderClient
    {
        private readonly WarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseOrderClient(WarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestClient = warehouseRequestClient;
        }
        
        /// <summary>
        /// Get orders for the given warehouse ID from the ShipWorks Warehouse app
        /// </summary>
        public async Task<IEnumerable<WarehouseOrder>> GetOrders(string warehouseID, string warehouseStoreID, DateTime lastModified, StoreTypeCode storeType)
        {
            try
            {
                IRestRequest request = new RestRequest(WarehouseEndpoints.Orders(warehouseID), Method.POST);

                request.AddQueryParameter("storeID", warehouseStoreID);
                request.AddQueryParameter("onlineLastModified", lastModified.ToString("o"));

                GenericResult<IRestResponse> response = await warehouseRequestClient
                    .MakeRequest(request, "Get Orders")
                    .ConfigureAwait(true);

                if (response.Failure)
                {
                    throw new DownloadException(response.Message, response.Exception);
                }

                IEnumerable<WarehouseOrder> orders = JsonConvert.DeserializeObject<IEnumerable<WarehouseOrder>>(
                    response.Value.Content,
                    new JsonSerializerSettings
                    {
                        Converters = {new WarehouseOrderJsonConverter(storeType)}
                    });

                return orders;
            }
            catch (Exception ex) when (ex.GetType() != typeof(DownloadException))
            {
                throw new DownloadException($"An error occured downloading orders for warehouse ID {warehouseID}", ex);
            }
        }
    }
}
