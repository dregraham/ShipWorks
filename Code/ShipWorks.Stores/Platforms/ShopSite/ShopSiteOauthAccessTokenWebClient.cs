using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite.Dto;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Interface to connecting to ShopSite using OAuth and getting an access token
    /// </summary>
    [Component]
    public class ShopSiteOauthAccessTokenWebClient : IShopSiteOauthAccessTokenWebClient
    {
        // The store we are connecting to
        private readonly IShopSiteStoreEntity shopSiteStore;
        private readonly ILifetimeScope lifetimeScope;
        private readonly Func<IHttpVariableRequestSubmitter> variableRequestSubmitterFactory;
        private const string storeSettingMissingErrorMessage = "The ShopSite {0} is missing or invalid.  Please enter your {0} by going to Manage > Stores > Your Store > Edit > Store Connection.  You will find instructions on how to find the {0} there.";

        /// <summary>
        /// Create this instance of the web client for connecting to the specified store
        /// </summary>
        public ShopSiteOauthAccessTokenWebClient(IShopSiteStoreEntity store, 
            ILifetimeScope lifetimeScope, 
            Func<IHttpVariableRequestSubmitter> variableRequestSubmitterFactory)
        {
            shopSiteStore = MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            this.lifetimeScope = MethodConditions.EnsureArgumentIsNotNull(lifetimeScope, nameof(lifetimeScope));
            this.variableRequestSubmitterFactory = variableRequestSubmitterFactory;

            ValidateApiAccessData(store);
        }

        /// <summary>
        /// Validate API access data
        /// </summary>
        private static void ValidateApiAccessData(IShopSiteStoreEntity store)
        {
            if (store.ShopSiteAuthentication == ShopSiteAuthenticationType.Basic)
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

            if (string.IsNullOrWhiteSpace(store?.OauthAuthorizationCode))
            {
                throw new ShopSiteException(string.Format(storeSettingMissingErrorMessage, "Authorization Code"));
            }
        }

        /// <summary>
        /// Get an AccessResponse for use in actual web calls
        /// </summary>
        public AccessResponse FetchAuthAccessResponse()
        {
            string nonceString = GenerateNonce();
            string rawCredentials = shopSiteStore.OauthClientID + ":" + nonceString;
            string clientCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(rawCredentials));
            string signature = CreateSignature(SecureText.Decrypt(shopSiteStore.OauthSecretKey, shopSiteStore.OauthClientID), clientCredentials);

            IHttpVariableRequestSubmitter postRequest = variableRequestSubmitterFactory();
            postRequest.Variables.Add("grant_type", "authorization_code");
            postRequest.Variables.Add("code", shopSiteStore.OauthAuthorizationCode);
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
            IApiLogEntry apiLogEntry = lifetimeScope.Resolve<IApiLogEntry>(TypedParameter.From(ApiLogSource.ShopSite), TypedParameter.From(action));
            apiLogEntry.LogRequest(postRequest);

            // Execute the request
            try
            {
                using (IHttpResponseReader postResponse = postRequest.GetResponse())
                {
                    string resultJson = postResponse.ReadResult();

                    // Log the response
                    apiLogEntry.LogResponse(resultJson);

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
