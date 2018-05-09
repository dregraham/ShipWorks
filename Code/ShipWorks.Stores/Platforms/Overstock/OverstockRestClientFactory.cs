using RestSharp;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
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
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockRestClientFactory(IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Create an IRestClient for the given store
        /// </summary>
        public IRestClient Create(IOverstockStoreEntity store)
        {
            string decryptedPassword = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.Username).Decrypt(store.Password);
            return new RestClient(StoreUrl)
            {
                Authenticator = new HttpBasicAuthenticator(store.Username, decryptedPassword)
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
