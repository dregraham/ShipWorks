using RestSharp;
using Interapptive.Shared.ComponentRegistration;
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
            return new RestClient(store.ApiUrl)
            {
                Authenticator = authenticatorFactory.Create(store)
            };
        }
    }
}
