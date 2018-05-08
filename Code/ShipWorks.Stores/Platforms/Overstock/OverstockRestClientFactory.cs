using RestSharp;
using Interapptive.Shared.ComponentRegistration;
using RestSharp.Authenticators;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Class for creating Overstock IRestClients
    /// </summary>
    [Component]
    public class OverstockRestClientFactory : IOverstockRestClientFactory
    {
        /// <summary>
        /// Create an IRestClient for the given store
        /// </summary>
        public IRestClient Create(IOverstockStoreEntity store)
        {
            return new RestClient(StoreUrl)
            {
                Authenticator = new HttpBasicAuthenticator(store.Username, store.Password)
            };
        }

        /// <summary>
        /// Get the store URL
        /// </summary>
        private string StoreUrl => InterapptiveOnly.Registry.GetValue("OverstockLiveServer", true) ? 
            "https://api.supplieroasis.com" : 
            "https://api.test.supplieroasis.com";
    }
}
