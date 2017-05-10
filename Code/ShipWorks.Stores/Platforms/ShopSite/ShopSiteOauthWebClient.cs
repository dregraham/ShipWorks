using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite.Dto;
using RestSharp.Extensions.MonoHttp;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Interface to connecting to ShopSite using OAuth
    /// </summary>
    [KeyedComponent(typeof(IShopSiteWebClient), ShopSiteAuthenticationType.Oauth)]
    public class ShopSiteOauthWebClient : IShopSiteWebClient
    {
        // The store we are connecting to
        private readonly IShopSiteStoreEntity shopSiteStore;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly Func<IHttpVariableRequestSubmitter> variableRequestSubmitterFactory;
        private readonly IShopSiteOauthAccessTokenWebClient accessTokenWebClient;
        private const string storeSettingMissingErrorMessage = "The ShopSite {0} is missing or invalid.  Please enter your {0} by going to Manage > Stores > Your Store > Edit > Store Connection.  You will find instructions on how to find the {0} there.";

        /// <summary>
        /// Create this instance of the web client for connecting to the specified store
        /// </summary>
        public ShopSiteOauthWebClient(IShopSiteStoreEntity store, 
            Func<IShopSiteStoreEntity, IShopSiteOauthAccessTokenWebClient> accessTokenWebClientFactory,
            Func<IHttpVariableRequestSubmitter> variableRequestSubmitterFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            shopSiteStore = MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            accessTokenWebClient = accessTokenWebClientFactory(shopSiteStore);
            MethodConditions.EnsureArgumentIsNotNull(accessTokenWebClient, nameof(accessTokenWebClient));
            this.apiLogEntryFactory = MethodConditions.EnsureArgumentIsNotNull(apiLogEntryFactory, nameof(apiLogEntryFactory));
            this.variableRequestSubmitterFactory = variableRequestSubmitterFactory;

            ValidateApiAccessData(store);
        }

        /// <summary>
        /// Validate API access data
        /// </summary>
        private static void ValidateApiAccessData(IShopSiteStoreEntity store)
        {
            if (store.Authentication == ShopSiteAuthenticationType.Basic)
            {
                throw new ShopSiteException($"Store '{store.StoreName}', is configured to use Basic authentication but the OAuth web client is being used.");
            }

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

        /// <summary>
        /// Determines if we can successfully connect to and login to ShopSite
        /// </summary>
        public void TestConnection()
        {
            List<KeyValuePair<string, string>> options = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("maxorder", "1")
            };

            ProcessRequest(options, "TestConnection");
        }

        /// <summary>
        /// Get the next page of orders, starting with the order with the specified order number
        /// </summary>
        public XmlDocument GetOrders(long startOrder)
        {
            List<KeyValuePair<string, string>> options = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("maxorder", shopSiteStore.DownloadPageSize.ToString()),
                new KeyValuePair<string, string>("startorder", startOrder.ToString()),
                new KeyValuePair<string, string>("pay", "no_cvv")
            };

            return ProcessRequest(options, "GetOrders");
        }

        /// <summary>
        /// Send the given request to ShopShite
        /// </summary>
        private XmlDocument ProcessRequest(List<KeyValuePair<string, string>> options, string action)
        {
            AccessResponse accessResponse = accessTokenWebClient.FetchAuthAccessResponse();
            string timestamp = DateTimeUtility.ToUnixTimestamp(DateTime.Now).ToString();
            string nonceString = GenerateNonce();

            IHttpVariableRequestSubmitter postRequest = variableRequestSubmitterFactory();
            postRequest.Uri = new Uri(accessResponse.download_url);
            postRequest.Timeout = TimeSpan.FromSeconds(shopSiteStore.RequestTimeout);
            postRequest.KeepAlive = false;

            // clientApp and dbname must go first.  Everything has to be in the corrrect order or auth 
            // will get denied.
            postRequest.Variables.Add("clientApp", "1");
            postRequest.Variables.Add("dbname", "orders");

            string optionsText = string.Empty;

            // We have to sort so that we build the digest the same way that ShopSite will 
            options.Sort((a, b) => String.Compare(a.Key, b.Key, StringComparison.InvariantCultureIgnoreCase));
            options.ForEach(kvp =>
                {
                    optionsText += $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}\n";
                    postRequest.Variables.Add(kvp.Key, kvp.Value);
                });

            // This needs to be URL encoded
            string macDigest = $"{accessResponse.access_token}\n{ timestamp }\n{nonceString}\n\nPOST\n{postRequest.Uri.Host}\n{postRequest.Uri.Port}\n{postRequest.Uri.AbsolutePath}\nclientApp=1\ndbname=orders\n{optionsText}";

            postRequest.Variables.Add("signature", CreateSignature(SecureText.Decrypt(shopSiteStore.OauthSecretKey, shopSiteStore.OauthClientID), macDigest));
            postRequest.Variables.Add("token", accessResponse.access_token);
            postRequest.Variables.Add("timestamp", timestamp);
            postRequest.Variables.Add("nonce", nonceString);

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

        /// <summary>
        /// Create a Base64 encoded string based on the given key
        /// </summary>
        // We have to use SHA1 for ShopSite, so disable this warning.
        #pragma warning disable CA5350
        private static string CreateSignature(string key, string valueToEncode)
        {
            byte[] keyBytes = key.Select(x => Convert.ToByte(x)).ToArray();
            
            using (HMACSHA1 hmac = new HMACSHA1(keyBytes, true))
            {
                byte[] signed = hmac.ComputeHash(Encoding.ASCII.GetBytes(valueToEncode));
                return Convert.ToBase64String(signed);
            }
        }

        /// <summary>
        /// Generates a nonce
        /// </summary>
        private static string GenerateNonce()
        {
            byte[] nonce = new Byte[4];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // The array is now filled with cryptographically strong random bytes.
                rng.GetBytes(nonce);
            }

            return BitConverter.ToString(nonce).Replace("-", "");
        }
    }
}
