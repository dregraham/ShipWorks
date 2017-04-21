using Interapptive.Shared.Enums;
using RestSharp;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Factory for creating web client authenticators
    /// </summary>
    [Component]
    public class BigCommerceAuthenticatorFactory : IBigCommerceAuthenticatorFactory
    {
        /// <summary>
        /// Create the correct authenticator based on the store
        /// </summary>
        public IAuthenticator Create(IBigCommerceStoreEntity store)
        {
            if (store.BigCommerceAuthentication == BigCommerceAuthenticationType.Oauth)
            {
                return new BigCommerceOAuthAuthenticator(store);
            }

            return new HttpBasicAuthenticator(store.ApiUserName, store.ApiToken);
        }
    }
}
