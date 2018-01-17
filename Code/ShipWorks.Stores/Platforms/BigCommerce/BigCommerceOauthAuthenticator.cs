﻿using System;
using System.Linq;
using RestSharp;
using RestSharp.Authenticators;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Authenticator implementing OAuth
    /// </summary>
    public class BigCommerceOAuthAuthenticator : IAuthenticator
    {
        private readonly IBigCommerceStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceOAuthAuthenticator(IBigCommerceStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (request.Parameters.Any(p => p.Name == "X-Auth-Token"))
            {
                return;
            }

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("X-Auth-Client", store.OauthClientId);
            request.AddHeader("X-Auth-Token", store.OauthToken);
        }
    }
}