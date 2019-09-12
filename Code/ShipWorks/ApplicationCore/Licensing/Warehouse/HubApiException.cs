using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Error codes that can be generated from the domain
    /// </summary>
    public enum HubErrorCode
    {
        /// <summary>
        /// Unknown error
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// There are no warehouses available to start the routing process
        /// </summary>
        NoWarehousesAvailableForRouting = 1,

        /// <summary>
        /// There are no warehouses left after the routing process has finished
        /// </summary>
        NoWarehousesLeftForRouting = 2,

        /// <summary>
        /// Items that are trying to be rerouted are not currently routed to the origin warehouse
        /// </summary>
        ItemsAreNotRoutedToWarehouse = 3,

        /// <summary>
        /// The customer only has a single warehouse
        /// </summary> 
        CustomerHasSingleWarehouse = 4,
    }

    /// <summary>
    /// DTO for an error from the Hub API
    /// </summary>
    public class HubApiError
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }

    /// <summary>
    /// Exception generated while making an API request to The Hub
    /// </summary>
    public class HubApiException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HubApiException(string message, HubErrorCode errorCode, HttpStatusCode statusCode) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Error from the api
        /// </summary>
        public HubErrorCode ErrorCode { get; }

        /// <summary>
        /// HTTP status code
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Create an exception based on a failure response
        /// </summary>
        public static HubApiException FromResponse(IRestResponse restResponse)
        {
            try
            {
                var error = JsonConvert.DeserializeObject<HubApiError>(
                    restResponse.Content,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy
                            {
                                OverrideSpecifiedNames = false
                            }
                        },
                    });

                return new HubApiException(error.Reason, (HubErrorCode) error.Code, restResponse.StatusCode);
            }
            catch (Exception)
            {
                // Gobble up exceptions because we don't want to fail because we couldn't parse an error
                return new HubApiException($"Unable to make warehouse request. StatusCode: {restResponse.StatusCode}", HubErrorCode.Unknown, restResponse.StatusCode);
            }
        }
    }
}