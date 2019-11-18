using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly IJsonRequest jsonRequest;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        private const string defaultEndpointBase = "https://openapi-rms.global.rakuten.com/2.0";
        private const string shippingPath = "/orders/{0}/{1}/{2}/shipping/{3}";
        private readonly string endpointBase;
        private readonly string ordersEndpoint;
        private readonly string shippingEndpoint;
        private readonly string testEndpoint;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenWebClient(IEncryptionProviderFactory encryptionProviderFactory,
            IHttpRequestSubmitterFactory submitterFactory,
            IJsonRequest jsonRequest)
        {
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.submitterFactory = submitterFactory;
            this.jsonRequest = jsonRequest;

            endpointBase = GetEndpointBase();
            ordersEndpoint = $"{endpointBase}/ordersearch";
            shippingEndpoint = $"{endpointBase}/orders/";
            testEndpoint = $"{endpointBase}/configurations" + "/{0}/labels/";

            jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
            };
        }

        /// <summary>
        /// Should the client use the fake api
        /// </summary>
        private bool UseFakeApi =>
            InterapptiveOnly.IsInterapptiveUser && !InterapptiveOnly.Registry.GetValue("RakutenLiveServer", true);

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
        public RakutenOrdersResponse GetOrders(IRakutenStoreEntity store, DateTime startDate)
        {
            var requestObject = new RakutenGetOrdersRequest(store, DateTime.UtcNow.AddDays(7), new DateTime(1970, 1, 1), startDate);

            var request = CreateRequest(store.AuthKey, ordersEndpoint, HttpVerb.Post, JsonConvert.SerializeObject(requestObject, jsonSerializerSettings));

            return SubmitRequest<RakutenOrdersResponse>("GetOrders", request);
        }

        /// <summary>
        /// Mark order as shipped and upload tracking number
        /// </summary>
        public RakutenBaseResponse ConfirmShipping(IRakutenStoreEntity store, ShipmentEntity shipment)
        {
            var rakutenOrder = shipment.Order as RakutenOrderEntity;
            var path = string.Format(shippingPath, store.MarketplaceID, store.ShopURL, rakutenOrder.OrderNumberComplete, rakutenOrder.RakutenPackageID);

            var shippingInfo = new RakutenShippingInfo
            {
                CarrierName = shipment.ShipmentTypeCode == ShipmentTypeCode.Other ?
                    ShippingManager.GetOtherCarrierDescription(shipment).Name :
                    ShippingManager.GetCarrierName(shipment.ShipmentTypeCode),
                ShippingStatus = "Shipped",
                TrackingNumber = shipment.TrackingNumber

            };

            var requestObject = new List<RakutenPatchOperation>();

            requestObject.Add(new RakutenPatchOperation
            {
                Operation = "replace",
                Path = path,
                Value = shippingInfo

            });

            var request = CreateRequest(store.AuthKey, shippingEndpoint, HttpVerb.Patch, JsonConvert.SerializeObject(requestObject, jsonSerializerSettings));

            return SubmitRequest<RakutenBaseResponse>("ConfirmShipping", request);
        }

        /// <summary>
        /// Create a request to Rakuten
        /// </summary>
        private IHttpRequestSubmitter CreateRequest(string encryptedAuthKey, string endpoint, HttpVerb method, string body)
        {
            IHttpRequestSubmitter submitter;

            submitter = !string.IsNullOrEmpty(body) ? submitterFactory.GetHttpTextPostRequestSubmitter(body, "application/json") :
                submitterFactory.GetHttpVariableRequestSubmitter();

            submitter.Uri = new Uri(endpoint);
            submitter.Verb = method;
            submitter.AllowHttpStatusCodes(new[] { HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.Accepted });

            var authKey = encryptionProviderFactory.CreateRakutenEncryptionProvider().Decrypt(encryptedAuthKey);

            submitter.Headers.Add("Authorization", $"ESA {authKey}");

            return submitter;
        }

        /// <summary>
        /// Verify we can connect with Rakuten
        /// </summary>
        public bool TestConnection(RakutenStoreEntity testStore)
        {
            if (UseFakeApi)
            {
                return true;
            }

            RakutenBaseResponse response;
            try
            {
                var endpoint = string.Format(testEndpoint, testStore.MarketplaceID);

                var request = CreateRequest(testStore.AuthKey, endpoint, HttpVerb.Get, null);

                response = jsonRequest.Submit<RakutenBaseResponse>("TestConnection", ApiLogSource.Rakuten, request);
            }
            catch (Exception)
            {
                return false;
            }

            return response?.Errors == null;
        }

        /// <summary>
        /// Parse the Rakuten errors
        /// </summary>
        private void ThrowError(RakutenErrors errors)
        {
            RakutenError error = null;

            // Use the common error first
            if (errors.Common != null)
            {
                error = errors.Common.First();
            }
            else if (errors.Specific != null)
            {
                error = errors.Specific.First().Value.First();
            }

            if (error != null)
            {
                throw new WebException($"An error occured when communicating with Rakuten: {error.ShortMessage} ({error.ErrorCode}) - {error.LongMessage}");
            }
            else
            {
                throw new WebException("An error occured when communicating with Rakuten");
            }
        }

        /// <summary>
        /// Submits the request to Rakuten
        /// </summary>
        private T SubmitRequest<T>(string action, IHttpRequestSubmitter request) where T : RakutenBaseResponse
        {
            var response = jsonRequest.Submit<T>(action, ApiLogSource.Rakuten, request);

            if (response?.Errors != null)
            {
                ThrowError(response.Errors);
            }

            return response;
        }
    }
}
