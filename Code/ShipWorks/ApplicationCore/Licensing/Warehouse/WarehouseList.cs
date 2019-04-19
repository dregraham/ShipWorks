using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Login to the warehouse
    /// </summary>
    [Component]
    public class WarehouseList : IWarehouseList
    {
        private readonly WarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseList(WarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Get list of warehouses
        /// </summary>
        public GenericResult<WarehouseListDto> GetList()
        {
            try
            {
                RestRequest restRequest = new RestRequest(WarehouseEndpoints.Warehouses, Method.GET);
                restRequest.RequestFormat = DataFormat.Json;

                GenericResult<IRestResponse> restResponse = warehouseRequestClient.MakeRequest(restRequest);

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
