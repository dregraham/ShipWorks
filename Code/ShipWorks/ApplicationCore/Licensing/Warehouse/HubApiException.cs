using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Extensions;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Exception generated while making an API request to The Hub
    /// </summary>
    public class HubApiException : Exception
    {
        /// <summary>
        /// Constructor to recreate exception with another HubApiExecption
        /// </summary>
        public HubApiException(string message, HubApiException innerException) : base(message, innerException)
        {
            ErrorCode = innerException.ErrorCode;
            StatusCode = innerException.StatusCode;
        }

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

                var reason = "An unknown error occurred";
                var code = HubErrorCode.Unknown;
                if (error != null)
                {
                    code = (HubErrorCode) error.Code;

                    if (error.Reason.HasValue())
                    {
                        error.Errors.Add(error.Reason);
                    }

                    if (error.Error.HasValue())
                    {
                        error.Errors.Add(error.Error);
                    }

                    if (error.Message.HasValue())
                    {
                        error.Errors.Add(error.Message);
                    }

                    if (error.Errors.Any())
                    {
                        reason = string.Join(Environment.NewLine, error.Errors);
                    }
                }

                return new HubApiException(reason, code, restResponse.StatusCode);
            }
            catch (Exception)
            {
                // Gobble up exceptions because we don't want to fail because we couldn't parse an error
                return new HubApiException($"Unable to make warehouse request. StatusCode: {restResponse.StatusCode}", HubErrorCode.Unknown, restResponse.StatusCode);
            }
        }
    }
}