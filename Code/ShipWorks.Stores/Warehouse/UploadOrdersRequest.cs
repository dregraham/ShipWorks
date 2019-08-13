using System;
using System.Collections.Generic;
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
    public class UploadOrdersRequest : IUploadOrdersRequest
    {
        private readonly IWarehouseRequestClient warehouseRequestClient;
        private readonly IWarehouseOrderDtoFactory warehouseOrderDtoFactory;

        public UploadOrdersRequest(
            IWarehouseRequestClient warehouseRequestClient,
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
        [JsonProperty("orders")]
        public IEnumerable<WarehouseOrder> Orders { get; private set; }

        /// <summary>
        /// Submit this UploadOrderRequest to the hub and return the response
        /// </summary>
        public async Task<GenericResult<WarehouseUploadOrderResponses>> Submit(IEnumerable<OrderEntity> orders, IStoreEntity store)
        {
            IRestRequest request =
                new RestRequest(WarehouseEndpoints.UploadOrders, Method.POST);

            request.JsonSerializer = new RestSharpJsonNetSerializer(GetJsonSerializerSettings());

            request.RequestFormat = DataFormat.Json;

            Batch = Guid.NewGuid();
            Orders = ConvertWarehouseOrders(orders, store);

            request.AddJsonBody(this);

            GenericResult<IRestResponse> response = await warehouseRequestClient
                .MakeRequest(request, "Upload Order")
                .ConfigureAwait(true);

            if (response.Failure)
            {
                return GenericResult.FromError<WarehouseUploadOrderResponses>(response.Message);
            }

            var orderResponse = JsonConvert.DeserializeObject<WarehouseUploadOrderResponses>(
                response.Value.Content, GetJsonSerializerSettings());

            return orderResponse;
        }

        /// <summary>
        /// Given a collection of orderEntities, return a colleciton of WarehouseOrders.
        /// </summary>
        private IEnumerable<WarehouseOrder> ConvertWarehouseOrders(IEnumerable<OrderEntity> orders, IStoreEntity store)
        {
            var warehouseOrders = new List<WarehouseOrder>();
            foreach (var order in orders)
            {
                warehouseOrders.Add(warehouseOrderDtoFactory.Create(order, store));
            }
            return warehouseOrders;
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
