using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using RestSharp;
using RestSharp.Authenticators;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class BigCommerceOauthAuthenticatorTest
    {
        [Fact]
        public void Authenticate_AddsOauthHeaders()
        {
            IRestClient client = new RestClient("https://www.com");
            IRestRequest request = new RestRequest("POST");
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            store.OauthClientId = "client ID";
            store.OauthToken = "oauth token";

            IAuthenticator authenticator = new BigCommerceOAuthAuthenticator(store);
            authenticator.Authenticate(client, request);

            Assert.Equal(4, request.Parameters.Count);
            Assert.Equal(store.OauthClientId, request.Parameters.FirstOrDefault(p => p.Name == "X-Auth-Client").Value);
            Assert.Equal(store.OauthToken, request.Parameters.FirstOrDefault(p => p.Name == "X-Auth-Token").Value);
            Assert.Equal("application/json", request.Parameters.FirstOrDefault(p => p.Name == "Content-Type").Value);
            Assert.Equal("application/json", request.Parameters.FirstOrDefault(p => p.Name == "Accept").Value);
        }
    }
}
