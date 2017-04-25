using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Class for creating BigCommerce IRestClients
    /// </summary>
    [Component]
    public class BigCommerceRestClientFactory : IBigCommerceRestClientFactory
    {
        private readonly IBigCommerceAuthenticatorFactory authenticatorFactory;
        readonly Func<string, IRestClient> createRestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceRestClientFactory(IBigCommerceAuthenticatorFactory authenticatorFactory, Func<string, IRestClient> createRestClient)
        {
            this.createRestClient = createRestClient;
            this.authenticatorFactory = authenticatorFactory;
        }

        /// <summary>
        /// Create an IRestClient for the given store
        /// </summary>
        public IRestClient Create(IBigCommerceStoreEntity store)
        {
            IRestClient apiClient = createRestClient(store.ApiUrl);

            apiClient.Authenticator = authenticatorFactory.Create(store);

            return apiClient;
        }
    }
}
