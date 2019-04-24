using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Upload skus to a warehouse
    /// </summary>
    [Component]
    public class UploadSkusToWarehouse : IUploadSkusToWarehouse
    {
        private readonly WarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public UploadSkusToWarehouse(WarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Upload skus to a warehouse
        /// </summary>
        public async Task<Result> Upload(IEnumerable<SkusToUploadDto> skusToUploadDto)
        {
            try
            {
                RestRequest restRequest = new RestRequest(WarehouseEndpoints.UploadSkus, Method.POST);
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddJsonBody(skusToUploadDto);

                GenericResult<IRestResponse> restResponse = await warehouseRequestClient.MakeRequest(restRequest)
                    .ConfigureAwait(false);

                return restResponse.Success ? Result.FromSuccess() : Result.FromError("Failed to uploaded skus.");
            }
            catch (Exception ex)
            {
                return Result.FromError(ex);
            }
        }
    }
}
