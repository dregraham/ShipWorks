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
        private IBigCommerceAuthenticatorFactory authenticatorFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceRestClientFactory(IBigCommerceAuthenticatorFactory authenticatorFactory)
        {
            this.authenticatorFactory = authenticatorFactory;
        }

        /// <summary>
        /// Create an IRestClient for the given store
        /// </summary>
        public IRestClient Create(IBigCommerceStoreEntity store)
        {
            IRestClient apiClient = new RestClient(store.ApiUrl);

            if (apiClient == null)
            {
                throw new BigCommerceException("Unable to create API client for BigCommerce.");
            }

            apiClient.Authenticator = authenticatorFactory.Create(store);

            return apiClient;
        }
    }
}
