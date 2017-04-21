using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using RestSharp;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.Groupon;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class BigCommerceAuthenticatorFactoryTest
    {
        [Fact]
        public void Create_ReturnsBasicAuth_WhenStoreSetForBasicAuth()
        {
            IBigCommerceAuthenticatorFactory authenticatorFactory = new BigCommerceAuthenticatorFactory();
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            IAuthenticator authenticator = authenticatorFactory.Create(store);

            Assert.True(authenticator is HttpBasicAuthenticator);
        }

        [Fact]
        public void Create_ReturnsOauth_WhenStoreSetForOauth()
        {
            IBigCommerceAuthenticatorFactory authenticatorFactory = new BigCommerceAuthenticatorFactory();
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            IAuthenticator authenticator = authenticatorFactory.Create(store);

            Assert.True(authenticator is BigCommerceOAuthAuthenticator);
        }
    }
}
