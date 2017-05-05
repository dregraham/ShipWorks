using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite.Dto;
using Newtonsoft.Json;
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
        
        /// <summary>
        /// Create this instance of the web client for connecting to the specified store
        /// </summary>
        public ShopSiteOauthWebClient(IShopSiteStoreEntity store)
        {
            shopSiteStore = MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
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
            AccessResponse accessResponse = FetchAuthAccessResponse();
            string timestamp = DateTimeUtility.ToUnixTimestamp(DateTime.Now).ToString();
            string nonceString = GenerateNonce();

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter()
            {
                Uri = new Uri(accessResponse.download_url),
                Timeout = TimeSpan.FromSeconds(shopSiteStore.RequestTimeout),
                KeepAlive = false
            };

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

            postRequest.Variables.Add("signature", CreateSignature(shopSiteStore.OauthSecretKey, macDigest));
            postRequest.Variables.Add("token", accessResponse.access_token);
            postRequest.Variables.Add("timestamp", timestamp);
            postRequest.Variables.Add("nonce", nonceString);

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
        /// Get an AccessResponse for use in actual web calls
        /// </summary>
        private AccessResponse FetchAuthAccessResponse()
        {
            string nonceString = GenerateNonce();
            string rawCredentials = shopSiteStore.OauthClientID + ":" + nonceString;
            string clientCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(rawCredentials));
            string signature = CreateSignature(shopSiteStore.OauthSecretKey, clientCredentials);

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("grant_type", "authorization_code");
            postRequest.Variables.Add("code", shopSiteStore.AuthorizationCode);
            postRequest.Variables.Add("client_credentials", clientCredentials);
            postRequest.Variables.Add("signature", signature);

            return ProcessFetchAccessTokenRequest(postRequest, "FetchAuthAccessToken");
        }

        /// <summary>
        /// Send the authorization token request to ShopSite
        /// </summary>
        private AccessResponse ProcessFetchAccessTokenRequest(IHttpVariableRequestSubmitter postRequest, string action)
        {
            // Communication with ShopSite is unreliable at best with KeepAlive on
            postRequest.KeepAlive = false;

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
                    string resultJson = postResponse.ReadResult();

                    // Log the response
                    logger.LogResponse(resultJson);

                    AccessResponse accessResponse = JsonConvert.DeserializeObject<AccessResponse>(resultJson);
                    if (accessResponse != null && !accessResponse.access_token.IsNullOrWhiteSpace())
                    {
                        return accessResponse;
                    }

                    ErrorResponse error = JsonConvert.DeserializeObject<ErrorResponse>(resultJson);
                    throw new ShopSiteException($"ShopSite failed to provide a valid access token.  {error.error_description}");
                }
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                throw new ShopSiteException("ShopSite returned an invalid JSON document.\n\nDetail: " + ex.Message, ex);
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
