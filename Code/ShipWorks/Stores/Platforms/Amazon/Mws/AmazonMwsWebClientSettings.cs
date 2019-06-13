﻿using System;
using System.Xml.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Class for handling Amazon MWS API Related Settings
    /// </summary>
    [Component]
    public class AmazonMwsWebClientSettings : IAmazonMwsWebClientSettings
    {
        private readonly IInterapptiveOnly interapptiveOnly;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsWebClientSettings(IAmazonCredentials mwsCredentials, IInterapptiveOnly interapptiveOnly)
        {
            this.interapptiveOnly = interapptiveOnly;
            Credentials = mwsCredentials;
        }

        /// <summary>
        /// Amazon MWS Credentials
        /// </summary>
        public IAmazonCredentials Credentials { get; set; }

        // Default base namespace for Amazon requests and responses
        private static readonly string endpointNamespace = "https://mws.amazonservices.com";

        /// <summary>
        /// HTTP endpoint of the Amazon service
        /// </summary>
        public string Endpoint
        {
            get
            {
                if (interapptiveOnly.IsInterapptiveUser)
                {
                    var useLiveEndpoint = interapptiveOnly.Registry.GetValue("AmazonMwsLive", true);
                    var endpointOverride = interapptiveOnly.Registry.GetValue("AmazonMwsEndpoint", string.Empty);

                    if (!useLiveEndpoint && !string.IsNullOrWhiteSpace(endpointOverride))
                    {
                        return endpointOverride;
                    }
                }

                switch (Credentials.Region)
                {
                    case "US":
                        return "https://mws.amazonservices.com";
                    case "CA":
                        return "https://mws.amazonservices.ca";
                    case "MX":
                        return "https://mws.amazonservices.com.mx";
                    default:
                        return "https://mws.amazonservices.co.uk";
                }
            }
        }

        /// <summary>
        /// Gets the access key id that should be used for the current store
        /// </summary>
        public string InterapptiveAccessKeyID => IsNorthAmericanStore ?
            "FMrhIncQWseTBwglDs00lVdXyPVgObvu" :
            "6bFMt0mymaWE0aWiaWT3SGs9LjvI//db";

        /// <summary>
        /// Gets the secret key that should be used for the current store
        /// </summary>
        public string InterapptiveSecretKey => IsNorthAmericanStore ?
            "JIX6YaY03qfP5LO31sssIzlVV2kAskmIPw/mj7X+M3EQpsyocKz062su7+INVas5" :
            "JjHvzq+MGZuxJu9EkjDv0QGSNQC/FYFg4lSe5PP5HMHRinkOWJhMLPeRH2057Ohd";

        /// <summary>
        /// Is the current store in North America?
        /// </summary>
        private bool IsNorthAmericanStore => Credentials.Region == "US" || Credentials.Region == "CA" || Credentials.Region == "MX";

        /// <summary>
        /// Gets the api version for each call
        /// </summary>
        public string GetApiVersion(AmazonMwsApiCall amazonMwsApiCall)
        {
            switch (amazonMwsApiCall)
            {
                case AmazonMwsApiCall.GetServiceStatus:
                case AmazonMwsApiCall.ListOrderItems:
                case AmazonMwsApiCall.ListOrderItemsByNextToken:
                case AmazonMwsApiCall.ListOrders:
                case AmazonMwsApiCall.ListOrdersByNextToken:
                    return "2013-09-01";
                case AmazonMwsApiCall.SubmitFeed:
                    return "2009-01-01";
                case AmazonMwsApiCall.ListMarketplaceParticipations:
                case AmazonMwsApiCall.GetAuthToken:
                    return "2011-07-01";
                case AmazonMwsApiCall.GetMatchingProductForId:
                    return "2011-10-01";
                case AmazonMwsApiCall.GetEligibleShippingServices:
                case AmazonMwsApiCall.CreateShipment:
                case AmazonMwsApiCall.CancelShipment:
                    return "2015-06-01";
                default:
                    throw new InvalidOperationException(
                        $"Unhandled AmazonMwsApiCall value in GetApiVersion: {amazonMwsApiCall}");
            }
        }

        /// <summary>
        /// Gets the API endpoint path
        /// </summary>
        public string GetApiEndpointPath(AmazonMwsApiCall amazonMwsApiCall)
        {
            string apiName = "";
            string version = "";

            switch (amazonMwsApiCall)
            {
                case AmazonMwsApiCall.GetServiceStatus:
                case AmazonMwsApiCall.ListOrderItems:
                case AmazonMwsApiCall.ListOrderItemsByNextToken:
                case AmazonMwsApiCall.ListOrders:
                case AmazonMwsApiCall.ListOrdersByNextToken:
                    apiName = "Orders";
                    version = GetApiVersion(amazonMwsApiCall);
                    break;
                case AmazonMwsApiCall.SubmitFeed:
                    break;
                case AmazonMwsApiCall.ListMarketplaceParticipations:
                case AmazonMwsApiCall.GetAuthToken:
                    apiName = "Sellers";
                    version = GetApiVersion(amazonMwsApiCall);
                    break;
                case AmazonMwsApiCall.GetMatchingProductForId:
                    apiName = "Products";
                    version = GetApiVersion(amazonMwsApiCall);
                    break;
                case AmazonMwsApiCall.GetEligibleShippingServices:
                case AmazonMwsApiCall.CreateShipment:
                case AmazonMwsApiCall.CancelShipment:
                    apiName = "MerchantFulfillment";
                    version = GetApiVersion(amazonMwsApiCall);
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unhandled AmazonMwsApiCall value in GetApiEndpointPath: {amazonMwsApiCall}");
            }

            string path = $"/{apiName}/{version}";
            path = path.Replace(@"//", @"/");

            return path;
        }

        /// <summary>
        /// Gets the Action parameter value for an API call
        /// </summary>
        public string GetActionName(AmazonMwsApiCall amazonMwsApiCall)
        {
            switch (amazonMwsApiCall)
            {
                case AmazonMwsApiCall.GetServiceStatus:
                    return "GetServiceStatus";
                case AmazonMwsApiCall.ListOrderItems:
                    return "ListOrderItems";
                case AmazonMwsApiCall.ListOrderItemsByNextToken:
                    return "ListOrderItemsByNextToken";
                case AmazonMwsApiCall.ListOrders:
                    return "ListOrders";
                case AmazonMwsApiCall.ListOrdersByNextToken:
                    return "ListOrdersByNextToken";
                case AmazonMwsApiCall.SubmitFeed:
                    return "SubmitFeed";
                case AmazonMwsApiCall.ListMarketplaceParticipations:
                    return "ListMarketplaceParticipations";
                case AmazonMwsApiCall.GetMatchingProductForId:
                    return "GetMatchingProductForId";
                case AmazonMwsApiCall.GetAuthToken:
                    return "GetAuthToken";
                case AmazonMwsApiCall.GetEligibleShippingServices:
                    return "GetEligibleShippingServices";
                case AmazonMwsApiCall.CreateShipment:
                    return "CreateShipment";
                case AmazonMwsApiCall.CancelShipment:
                    return "CancelShipment";
                default:
                    throw new InvalidOperationException($"Unhandled AmazonMwsApiCall '{amazonMwsApiCall}'");
            }
        }

        /// <summary>
        /// Gets the XNamespace value for a particular API
        /// </summary>
        public XNamespace GetApiNamespace(AmazonMwsApiCall api)
        {
            string apiNamespace = string.Empty;
            if (api == AmazonMwsApiCall.SubmitFeed)
            {
                // thanks Amazon for not sticking to a scheme
                apiNamespace = $"http://mws.amazonaws.com/doc/{GetApiVersion(api)}/";
            }
            else if (api == AmazonMwsApiCall.GetMatchingProductForId)
            {
                apiNamespace = $"http://mws.amazonservices.com/schema/Products/{GetApiVersion(api)}";
            }
            else
            {
                apiNamespace = endpointNamespace + GetApiEndpointPath(api);
            }

            return apiNamespace;
        }
    }
}
