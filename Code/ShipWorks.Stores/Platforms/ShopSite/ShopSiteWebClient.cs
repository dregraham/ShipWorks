using System;
using System.Net;
using System.Xml;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Interface to connecting to ShopSite
    /// </summary>
    [KeyedComponent(typeof(IShopSiteWebClient), ShopSiteAuthenticationType.Basic)]
    public class ShopSiteWebClient : IShopSiteWebClient
    {
        // The store we are connecting to
        private readonly IShopSiteStoreEntity shopSiteStore;

        private const string storeSettingMissingErrorMessage = "The ShopSite {0} is missing or invalid.  Please enter your {0} by going to Manage > Stores > Your Store > Edit > Store Connection.  You will find instructions on how to find the {0} there.";

        /// <summary>
        /// Create this instance of the web client for connecting to the specified store
        /// </summary>
        public ShopSiteWebClient(IShopSiteStoreEntity store)
        {
            shopSiteStore = MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            ValidateApiAccessData(store);
        }

        /// <summary>
        /// Validate API access data
        /// </summary>
        private static void ValidateApiAccessData(IShopSiteStoreEntity store)
        {
            if (store.Authentication == ShopSiteAuthenticationType.Basic)
            {
                if (string.IsNullOrWhiteSpace(store?.ApiUrl))
                {
                    throw new ShopSiteException(string.Format(storeSettingMissingErrorMessage, "CGI Path"));
                }

                if (string.IsNullOrWhiteSpace(store?.Username))
                {
                    throw new ShopSiteException(string.Format(storeSettingMissingErrorMessage, "Merchant ID"));
                }

                if (string.IsNullOrWhiteSpace(store?.Password))
                {
                    throw new ShopSiteException(string.Format(storeSettingMissingErrorMessage, "Password"));
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(store?.ApiUrl))
                {
                    throw new ShopSiteException(string.Format(storeSettingMissingErrorMessage, "Authorization URL"));
                }

                if (string.IsNullOrWhiteSpace(store?.OauthClientID))
                {
                    throw new ShopSiteException(string.Format(storeSettingMissingErrorMessage, "Client ID"));
                }

                if (string.IsNullOrWhiteSpace(store?.OauthSecretKey))
                {
                    throw new ShopSiteException(string.Format(storeSettingMissingErrorMessage, "Secret Key"));
                }

                if (string.IsNullOrWhiteSpace(store?.AuthorizationCode))
                {
                    throw new ShopSiteException(string.Format(storeSettingMissingErrorMessage, "Authorization Code"));
                }
            }
        }

        /// <summary>
        /// Determines if we can successfully connect to and login to ShopSite
        /// </summary>
        public void TestConnection()
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("maxorder", "1");

            ProcessRequest(postRequest, "TestConnection");
        }

        /// <summary>
        /// Get the next page of orders, starting with the order with the specified order number
        /// </summary>
        public XmlDocument GetOrders(long startOrder)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("startorder", startOrder.ToString());
            postRequest.Variables.Add("maxorder", shopSiteStore.DownloadPageSize.ToString());
            postRequest.Variables.Add("pay", "yes");

            return ProcessRequest(postRequest, "GetOrders");
        }

        /// <summary>
        /// Send the given request to the ShopShite CGI url
        /// </summary>
        private XmlDocument ProcessRequest(HttpVariableRequestSubmitter postRequest, string action)
        {
            // Communication with ShopSite is unreliable at best with KeepAlive on
            postRequest.KeepAlive = false;

            // Add required parameters
            postRequest.Variables.Add("version", "12.0");

            // Set the uri and parameters
            postRequest.Uri = new Uri(shopSiteStore.ApiUrl);
            postRequest.Timeout = TimeSpan.FromSeconds(shopSiteStore.RequestTimeout);

            // Log the request
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.ShopSite, action);
            logger.LogRequest(postRequest);

            // Execute the request
            try
            {
                using (IHttpResponseReader postResponse = postRequest.GetResponse())
                {
                    string resultXml = postResponse.ReadResult();

                    // Log the response
                    logger.LogResponse(resultXml);

                    // As of 12/22/09 I saw HTML being returned as the XML response if the username as bad that contained this message...
                    if (resultXml.Contains("Your User Name could not be determined"))
                    {
                        throw new ShopSiteException("Your username could not be validated by ShopSite.");
                    }

                    resultXml = XmlUtility.StripInvalidXmlCharacters(resultXml);

                    XmlDocument xmlResponse = new XmlDocument { XmlResolver = new XmlUrlResolver() };
                    xmlResponse.LoadXml(resultXml);

                    return xmlResponse;
                }
            }
            catch (XmlException ex)
            {
                throw new ShopSiteException("ShopSite returned an invalid XML document.\n\nDetail: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ShopSiteException));
            }
        }
    }
}
