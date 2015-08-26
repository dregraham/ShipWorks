using System;
using System.Xml.Linq;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Class for handeling Amazon MWS API Related Settings
    /// </summary>
    public class AmazonMwsWebClientSettings : IAmazonMwsWebClientSettings
    {
        // mwsEntity to get settings from
        public IAmazonMwsConnection Connection;

        public AmazonMwsWebClientSettings(IAmazonMwsConnection mwsConnection)
        {
            this.Connection = mwsConnection;
        }

        // Default base namespace for Amazon requests and responses
        private static string endpointNamespace = "https://mws.amazonservices.com";

        /// <summary>
        /// HTTP endpoint of the Amazon service
        /// </summary>
        public string Endpoint
        {
            get
            {
                switch (Connection.AmazonApiRegion)
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
        public string InterapptiveAccessKeyID
        {
            get
            {
                return IsNorthAmericanStore ?
                    "FMrhIncQWseTBwglDs00lVdXyPVgObvu" :
                    "6bFMt0mymaWE0aWiaWT3SGs9LjvI//db";
            }
        }

        /// <summary>
        /// Gets the secret key that should be used for the current store
        /// </summary>
        public string InterapptiveSecretKey
        {
            get
            {
                return IsNorthAmericanStore ?
                    "JIX6YaY03qfP5LO31sssIzlVV2kAskmIPw/mj7X+M3EQpsyocKz062su7+INVas5" :
                    "JjHvzq+MGZuxJu9EkjDv0QGSNQC/FYFg4lSe5PP5HMHRinkOWJhMLPeRH2057Ohd";
            }
        }

        /// <summary>
        /// Is the current store in North America?
        /// </summary>
        private bool IsNorthAmericanStore
        {
            get
            {
                return Connection.AmazonApiRegion == "US" || Connection.AmazonApiRegion == "CA" || Connection.AmazonApiRegion == "MX";
            }
        }

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
                default:
                    throw new InvalidOperationException(String.Format("Unhandled AmazonMwsApiCall value in GetApiVersion: {0}", amazonMwsApiCall));
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
                default:
                    throw new InvalidOperationException(String.Format("Unhandled AmazonMwsApiCall value in GetApiEndpointPath: {0}", amazonMwsApiCall));
            }

            string path = string.Format("/{0}/{1}", apiName, version);
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
                default:
                    throw new InvalidOperationException(string.Format("Unhandled AmazonMwsApiCall '{0}'", amazonMwsApiCall));
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
                apiNamespace = String.Format("http://mws.amazonaws.com/doc/{0}/", GetApiVersion(api));
            }
            else if (api == AmazonMwsApiCall.GetMatchingProductForId)
            {
                apiNamespace = string.Format("http://mws.amazonservices.com/schema/Products/{0}", GetApiVersion(api));
            }
            else
            {
                apiNamespace = endpointNamespace + GetApiEndpointPath(api);
            }
            
            return apiNamespace;
        }
    }
}
