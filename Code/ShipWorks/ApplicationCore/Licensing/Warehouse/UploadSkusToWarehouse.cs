using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Common.Net;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Upload skus to a warehouse
    /// </summary>
    [Component]
    public class UploadSkusToWarehouse : IUploadSkusToWarehouse
    {
        private readonly IWarehouseRequestClient warehouseRequestClient;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public UploadSkusToWarehouse(IWarehouseRequestClient warehouseRequestClient, Func<Type, ILog> createLogger)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Upload skus to a warehouse
        /// </summary>
        public async Task<Result> Upload(SkusToUploadDto skusToUploadDto)
        {
            try
            {
                RestRequest restRequest = new RestRequest(WarehouseEndpoints.UploadSkus, Method.POST);
                restRequest.JsonSerializer = new RestSharpJsonNetSerializer();
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddJsonBody(skusToUploadDto);

                GenericResult<IRestResponse> restResponse = await warehouseRequestClient.MakeRequest(restRequest, "UploadSkusToWarehouse")
                    .ConfigureAwait(false);

                return restResponse.Success ? Result.FromSuccess() : Result.FromError("Failed to upload SKUs.");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Result.FromError(ex);
            }
        }
    }
}
