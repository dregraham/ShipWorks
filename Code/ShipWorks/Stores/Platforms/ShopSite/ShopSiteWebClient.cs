using System;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using System.Net;
using Interapptive.Shared.Utility;
using System.Xml;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Interface to connecting to ShopSite
    /// </summary>
    public class ShopSiteWebClient
    {
        // The store we are connecting to
        ShopSiteStoreEntity store;

        /// <summary>
        /// Create an instance of the web client for connecting to the specified store
        /// </summary>
        public ShopSiteWebClient(ShopSiteStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;
        }

        /// <summary>
        /// Determines if we can succesffuly connect to and login to ShopSite
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
            postRequest.Variables.Add("maxorder", store.DownloadPageSize.ToString());
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
            postRequest.Variables.Add("version", "8.0");

            // Set the uri and parameters
            postRequest.Uri = GetShopSiteCgiUri();
            postRequest.Timeout = TimeSpan.FromSeconds(store.RequestTimeout);
            postRequest.Credentials = new NetworkCredential(store.Username, SecureText.Decrypt(store.Password, store.Username));

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

                    XmlDocument xmlResponse = new XmlDocument();
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

        /// <summary>
        /// Get the URI to use to connecto the ShopSite CGI script
        /// </summary>
        private Uri GetShopSiteCgiUri()
        {
            string requiredScheme = store.RequireSSL ? "https://" : "http://";

            // If Url scheme not set, default to https
            string url = store.CgiUrl;
            if (url.IndexOf(Uri.SchemeDelimiter) == -1)
            {
                url = requiredScheme + url;
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ShopSiteException("The ShopSite CGI URl is not valid.");
            }

            Uri uri = new Uri(url);

            // Validate the secure scheme
            if (store.RequireSSL && uri.Scheme != Uri.UriSchemeHttps)
            {
                throw new ShopSiteException(
                    "The ShopSite Module URL protocol you entered is (" + uri.Scheme + "://).  For your security,\n" +
                    "you are required to use the (https://) protocol.");
            }

            // Validate the unsecure scheme
            if (!store.RequireSSL && uri.Scheme != Uri.UriSchemeHttp)
            {
                throw new ShopSiteException(
                    "The ShopSite Module URL protocol you entered is (" + uri.Scheme + "://).  To connect unsecure,\n" +
                    "you are required to use the (http://) protocol.");
            }

            return uri;
        }
    }
}
