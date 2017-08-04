using System;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    [Component]
    public class JetTokenManager : IJetTokenManager
    {
        private readonly IJsonRequest jsonRequest;
        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly LruCache<string, string> tokenCache;
        private readonly string tokenEndpoint = "https://merchant-api.jet.com/api/token";

        public JetTokenManager(IJsonRequest jsonRequest, IHttpRequestSubmitterFactory submitterFactory, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.jsonRequest = jsonRequest;
            this.submitterFactory = submitterFactory;
            this.encryptionProviderFactory = encryptionProviderFactory;

            tokenCache = new LruCache<string, string>(50, TimeSpan.FromHours(9));
        }
        
        /// <summary>
        /// Get Token
        /// </summary>
        public void AddTokenToRequest(IHttpRequestSubmitter request, JetStoreEntity store)
        {
            if (tokenCache.Contains(store.ApiUser))
            {
                request.Headers.Add("Authorization", $"bearer {tokenCache[store.ApiUser]}");
            }
            
            string password = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.ApiUser)
                .Decrypt(store.Secret);

            string token = GetToken(store.ApiUser, password);

            request.Headers.Add("Authorization", $"bearer {token}");
        }

        /// <summary>
        /// 
        /// </summary>
        public string GetToken(string username, string password)
        {
            if (tokenCache.Contains(username))
            {
                return tokenCache[username];
            }

            IHttpRequestSubmitter submitter = submitterFactory.GetHttpTextPostRequestSubmitter(
                $"{{\"user\": \"{username}\",\"pass\":\"{password}\"}}",
                "application/json");

            submitter.Uri = new Uri(tokenEndpoint);

            JetTokenResponse tokenResponse = jsonRequest.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter);
            
            tokenCache[username] = tokenResponse.Token;

            return tokenResponse.Token;
        }

    }
}