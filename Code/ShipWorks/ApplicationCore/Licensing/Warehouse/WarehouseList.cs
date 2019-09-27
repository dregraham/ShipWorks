using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Common.Net;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Login to the warehouse
    /// </summary>
    [Component]
    public class WarehouseList : IWarehouseList
    {
        private readonly IWarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseList(IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Get list of warehouses
        /// </summary>
        public async Task<GenericResult<WarehouseListDto>> GetList()
        {
            try
            {
                RestRequest restRequest = new RestRequest(WarehouseEndpoints.Warehouses, Method.GET)
                {
                    JsonSerializer = new RestSharpJsonNetSerializer(),
                    RequestFormat = DataFormat.Json
                };

                GenericResult<IRestResponse> restResponse = await warehouseRequestClient.MakeRequest(restRequest, "ListWarehouses")
                    .ConfigureAwait(false);

                if (restResponse.Success)
                {
                    // De-serialize the result
                    WarehouseListDto requestResult = JsonConvert.DeserializeObject<WarehouseListDto>(restResponse.Value.Content,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return requestResult;
                }

                return GenericResult.FromError<WarehouseListDto>(restResponse.Message);
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<WarehouseListDto>(ex);
            }
        }
    }
}
