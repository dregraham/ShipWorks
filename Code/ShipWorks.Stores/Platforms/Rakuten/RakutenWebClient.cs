using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Rakuten.DTO;
using ShipWorks.Stores.Platforms.Rakuten.DTO.Requests;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// The client for communicating with the Rakuten API
    /// </summary>
    [Component]
    public class RakutenWebClient : IRakutenWebClient
    {
        private readonly RakutenStoreEntity store;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;

        private const string defaultEndpointBase = "https://openapi-rms.global.rakuten.com/2.0";
        private readonly string ordersEndpoint;
        private readonly string shippingPath;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenWebClient(RakutenStoreEntity store,
            IEncryptionProviderFactory encryptionProviderFactory,
            IHttpRequestSubmitterFactory submitterFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.store = store;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.submitterFactory = submitterFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;

            var endpointBase = GetEndpointBase();
            ordersEndpoint = $"{endpointBase}/ordersearch";
            shippingPath = $"{endpointBase}/orders/{store.MarketplaceID}/{store.ShopURL}/" + "{0}/shipping/{1}/shippingstatus";
        }

        /// <summary>
        /// Should the client use the fake api
        /// </summary>
        private bool UseFakeApi =>
            InterapptiveOnly.IsInterapptiveUser && !InterapptiveOnly.Registry.GetValue("RakutenLive", true);

        /// <summary>
        /// Get the base endpoint for Rakuten requests
        /// </summary>
        private string GetEndpointBase()
        {
            if (UseFakeApi)
            {
                var endpointOverride = InterapptiveOnly.Registry.GetValue("RakutenEndpoint", string.Empty);
                if (!string.IsNullOrWhiteSpace(endpointOverride))
                {
                    return endpointOverride;
                }
            }

            return defaultEndpointBase;
        }

        /// <summary>
        /// Get a list of orders from Rakuten
        /// </summary>
        public RakutenOrdersResponse GetOrders(DateTime startDate)
        {
            var requestObject = new RakutenGetOrdersRequest(store, DateTime.Now, DateTime.MinValue, startDate);

            var request = CreateRequest(ordersEndpoint, HttpVerb.Get, requestObject);

            return ProcessRequest<RakutenOrdersResponse>(request, "GetOrders");
        }

        /// <summary>
        /// Mark order as shipped and upload tracking number
        /// </summary>
        public RakutenShipmentResponse ConfirmShipping(ShipmentEntity shipment)
        {
            var order = shipment.Order as RakutenOrderEntity;

            var path = String.Format(shippingPath, shipment.Order.OrderNumberComplete, order.PackageID);

            var shippingInfo = new RakutenShippingInfo
            {
                CarrierName = GetCarrier(shipment),
                ShippingStatus = "Shipped",
                TrackingNumber = shipment.TrackingNumber

            };

            var requestObject = new RakutenConfirmShippingRequest();

            requestObject.Operations.Add(new RakutenPatchOperation
            {
                OP = "replace",
                Path = path,
                Value = shippingInfo

            });

            var request = CreateRequest(ordersEndpoint, HttpVerb.Patch, requestObject);

            return ProcessRequest<RakutenShipmentResponse>(request, "ConfirmShipping");
        }

        /// <summary>
        /// Create a request to Rakuten
        /// </summary>
        private IHttpRequestSubmitter CreateRequest<T>(string endpoint, HttpVerb method, T body)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };

            IHttpRequestSubmitter submitter = submitterFactory.GetHttpTextPostRequestSubmitter(JsonConvert.SerializeObject(body, jsonSerializerSettings),
                "application/json");

            submitter.Uri = new Uri(endpoint);
            submitter.Verb = method;

            var authKey = encryptionProviderFactory.CreateSecureTextEncryptionProvider("Rakuten")
                .Decrypt(store.AuthKey);

            submitter.Headers.Add("Authorization", $"ESA {authKey}");

            return submitter;
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        private T ProcessRequest<T>(IHttpRequestSubmitter request, string action)
        {
            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.Rakuten, action);
            apiLogEntry.LogRequest(request);

            try
            {
                IHttpResponseReader httpResponseReader = request.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                return JsonConvert.DeserializeObject<T>(result, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get the carrier name
        /// </summary>
        private static string GetCarrier(ShipmentEntity shipment)
        {
            switch (shipment.ShipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return "UPS";
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    return "USPS";
                case ShipmentTypeCode.FedEx:
                    return "FedEx";
                case ShipmentTypeCode.OnTrac:
                    return "OnTrac";
                default:
                    return "Other";
            }
        }

        public bool TestConnection(RakutenStoreEntity testStore)
        {
            return true;
        }
    }
}
