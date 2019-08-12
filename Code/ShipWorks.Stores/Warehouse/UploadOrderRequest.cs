using System;
using System.Reflection;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Request to upload order to the hub
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [Component]
    public class UploadOrderRequest : IUploadOrdersRequest
    {
        private readonly WarehouseRequestClient warehouseRequestClient;
        private readonly IWarehouseOrderDtoFactory warehouseOrderDtoFactory;

        public UploadOrderRequest(
            WarehouseRequestClient warehouseRequestClient, 
            IWarehouseOrderDtoFactory warehouseOrderDtoFactory)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.warehouseOrderDtoFactory = warehouseOrderDtoFactory;
        }

        /// <summary>
        /// The batch
        /// </summary>
        [JsonProperty("batch")]
        public Guid Batch { get; private set; }

        /// <summary>
        /// The order
        /// </summary>
        [JsonProperty("order")]
        public WarehouseOrder Order { get; private set; }

        /// <summary>
        /// Submit this UploadOrderRequest to the hub and return the response
        /// </summary>
        public async Task<GenericResult<WarehouseUploadOrderResponse>> Submit(OrderEntity order, IStoreEntity store)
        {
            IRestRequest request =
                new RestRequest(WarehouseEndpoints.UploadOrder, Method.POST);

            request.JsonSerializer = new RestSharpJsonNetSerializer(GetJsonSerializerSettings());

            request.RequestFormat = DataFormat.Json;

            Batch = Guid.NewGuid();
            Order = warehouseOrderDtoFactory.Create(order, store);

            request.AddJsonBody(this);

            GenericResult<IRestResponse> response = await warehouseRequestClient
                .MakeRequest(request, "Upload Order")
                .ConfigureAwait(true);

            if (response.Failure)
            {
                return GenericResult.FromError<WarehouseUploadOrderResponse>(response.Message);
            }

            var orderResponse = JsonConvert.DeserializeObject<WarehouseUploadOrderResponse>(
                response.Value.Content, GetJsonSerializerSettings());

            return orderResponse;
        }

        /// <summary>
        /// Gets json serializer settings
        /// </summary>
        private JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        OverrideSpecifiedNames = false
                    }
                },
            };
        }
    }
}
