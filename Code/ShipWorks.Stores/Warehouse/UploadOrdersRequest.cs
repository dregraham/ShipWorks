﻿using System;
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
using ShipWorks.Warehouse.Orders.DTO;

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
        private readonly Func<string, Method, IRestRequest> createRateRequest;

        public UploadOrdersRequest(
            IWarehouseRequestClient warehouseRequestClient,
            IWarehouseOrderDtoFactory warehouseOrderDtoFactory,
            Func<string, Method, IRestRequest> createRateRequest)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.warehouseOrderDtoFactory = warehouseOrderDtoFactory;
            this.createRateRequest = createRateRequest;
        }

        /// <summary>
        /// The version to use
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; private set; }

        /// <summary>
        /// The batch
        /// </summary>
        [JsonProperty("batch")]
        public Guid? Batch { get; private set; }

        /// <summary>
        /// The order
        /// </summary>
        [JsonProperty("orders")]
        public IEnumerable<WarehouseOrder> Orders { get; private set; }

        /// <summary>
        /// Submit this UploadOrderRequest to the hub and return the response
        /// </summary>
        public async Task<GenericResult<IEnumerable<WarehouseUploadOrderResponse>>> Submit(IEnumerable<OrderEntity> orders, IStoreEntity store, bool assignBatch)
        {
            if (store.TypeCode == (int) StoreTypeCode.Odbc)
            {
                Version = 1;
            }

            IRestRequest request = createRateRequest(WarehouseEndpoints.UploadOrders, Method.POST);

            request.JsonSerializer = new RestSharpJsonNetSerializer(GetJsonSerializerSettings());

            request.RequestFormat = DataFormat.Json;

            Orders = ConvertWarehouseOrders(orders, store);

            if (assignBatch)
            {
                Batch = Guid.NewGuid();
            }
            else
            {
                foreach (var order in Orders)
                {
                    order.Warehouse = string.Empty;
                }
            }

            request.AddJsonBody(this);

            GenericResult<IRestResponse> response = await warehouseRequestClient
                .MakeRequest(request, "Upload Orders")
                .ConfigureAwait(true);

            if (response.Failure)
            {
                return GenericResult.FromError<IEnumerable<WarehouseUploadOrderResponse>>(response.Message);
            }

            try
            {
                var orderResponse = JsonConvert.DeserializeObject<IEnumerable<WarehouseUploadOrderResponse>>(
                    response.Value.Content, GetJsonSerializerSettings());

                return GenericResult.FromSuccess(orderResponse);
            }
            catch (Exception)
            {
                return GenericResult.FromError<IEnumerable<WarehouseUploadOrderResponse>>("An error occurred while uploading orders to hub.");
            }
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
