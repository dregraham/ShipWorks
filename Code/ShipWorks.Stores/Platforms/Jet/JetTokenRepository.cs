﻿using System;
using System.Net;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    [Component(SingleInstance = true)]
    public class JetTokenRepository : IJetTokenRepository
    {
        private readonly IJsonRequest jsonRequest;
        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly LruCache<string, JetToken> tokenCache;
        private readonly string tokenEndpoint = "https://merchant-api.jet.com/api/token";

        /// <summary>
        /// Constructor
        /// </summary>
        public JetTokenRepository(IJsonRequest jsonRequest, IHttpRequestSubmitterFactory submitterFactory, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.jsonRequest = jsonRequest;
            this.submitterFactory = submitterFactory;
            this.encryptionProviderFactory = encryptionProviderFactory;

            tokenCache = new LruCache<string, JetToken>(50, TimeSpan.FromHours(9));
        }
        
        /// <summary>
        /// Get the token for the store
        /// </summary>
        public IJetToken GetToken(JetStoreEntity store)
        {
            string password = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.ApiUser)
                .Decrypt(store.Secret);

            return GetToken(store.ApiUser, password);
        }

        /// <summary>
        /// Removes the token from the Cache
        /// </summary>
        public void RemoveToken(JetStoreEntity store)
        {
            if (tokenCache.Contains(store.ApiUser))
            {
                tokenCache.Remove(store.ApiUser);
            }
        }

        /// <summary>
        /// Get the token for the given username/password
        /// </summary>
        public IJetToken GetToken(string username, string password)
        {
            if (tokenCache.Contains(username))
            {
                return tokenCache[username];
            }

            IHttpRequestSubmitter submitter = submitterFactory.GetHttpTextPostRequestSubmitter(
                $"{{\"user\": \"{username}\",\"pass\":\"{password}\"}}",
                "application/json");

            submitter.Uri = new Uri(tokenEndpoint);

            try
            {
                JetTokenResponse tokenResponse = jsonRequest.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter);

                tokenCache[username] = new JetToken(tokenResponse.Token);

                return tokenCache[username];
            }
            catch (WebException)
            {
                return JetToken.InvalidToken;
            }
        }
    }
}