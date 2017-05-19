using System;
using System.Net;
using System.Xml;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
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
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly Func<IHttpVariableRequestSubmitter> variableRequestSubmitterFactory;
        private const string storeSettingMissingErrorMessage = "The ShopSite {0} is missing or invalid.  Please enter your {0} by going to Manage > Stores > Your Store > Edit > Store Connection.  You will find instructions on how to find the {0} there.";
        private readonly IEncryptionProviderFactory encryptionFactory;

        /// <summary>
        /// Create this instance of the web client for connecting to the specified store
        /// </summary>
        public ShopSiteWebClient(IShopSiteStoreEntity store,
                                 IEncryptionProviderFactory encryptionFactory,
                                 Func<IHttpVariableRequestSubmitter> variableRequestSubmitterFactory,
                                 Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.encryptionFactory = encryptionFactory;
            shopSiteStore = MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            this.apiLogEntryFactory = MethodConditions.EnsureArgumentIsNotNull(apiLogEntryFactory, nameof(apiLogEntryFactory));
            this.variableRequestSubmitterFactory = MethodConditions.EnsureArgumentIsNotNull(variableRequestSubmitterFactory, nameof(variableRequestSubmitterFactory));

            ValidateApiAccessData(store);
        }

        /// <summary>
        /// Progress reporter associated with the client
        /// </summary>
        /// <remarks>
        /// If this is null, the client cannot be canceled and progress will not be reported
        /// </remarks>
        public IProgressReporter ProgressReporter { get; set; }

        /// <summary>
        /// Validate API access data
        /// </summary>
        private static void ValidateApiAccessData(IShopSiteStoreEntity store)
        {
            if (store.ShopSiteAuthentication == ShopSiteAuthenticationType.Oauth)
            {
                throw new ShopSiteException($"Store '{store.StoreName}', is configured to use OAuth authentication but the Basic web client is being used.");
            }

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

        /// <summary>
        /// Determines if we can successfully connect to and login to ShopSite
        /// </summary>
        public void TestConnection()
        {
            IHttpVariableRequestSubmitter postRequest = variableRequestSubmitterFactory();

            postRequest.Variables.Add("maxorder", "1");

            ProcessRequest(postRequest, "TestConnection");
        }

        /// <summary>
        /// Get the next page of orders, starting with the order with the specified order number
        /// </summary>
        public XmlDocument GetOrders(long startOrder)
        {
            IHttpVariableRequestSubmitter postRequest = variableRequestSubmitterFactory();

            postRequest.Variables.Add("maxorder", shopSiteStore.DownloadPageSize.ToString());
            postRequest.Variables.Add("pay", "yes");
            postRequest.Variables.Add("startorder", startOrder.ToString());

            return ProcessRequest(postRequest, "GetOrders");
        }

        /// <summary>
        /// Send the given request to the ShopShite CGI url
        /// </summary>
        private XmlDocument ProcessRequest(IHttpVariableRequestSubmitter postRequest, string action)
        {
            // Communication with ShopSite is unreliable at best with KeepAlive on
            postRequest.KeepAlive = false;

            // Add required parameters
            postRequest.Variables.Add("version", "12.0");

            // Set the uri and parameters
            postRequest.Uri = new Uri(shopSiteStore.ApiUrl);
            postRequest.Timeout = TimeSpan.FromSeconds(shopSiteStore.RequestTimeout);
            postRequest.Credentials = new NetworkCredential(shopSiteStore.Username,
                encryptionFactory.CreateSecureTextEncryptionProvider(shopSiteStore.Username).Decrypt(shopSiteStore.Password));

            // Log the request
            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.ShopSite, action);
            apiLogEntry.LogRequest(postRequest);

            // Execute the request
            try
            {
                using (IHttpResponseReader postResponse = postRequest.GetResponse())
                {
                    string resultXml = postResponse.ReadResult();

                    // Log the response
                    apiLogEntry.LogResponse(resultXml);

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
