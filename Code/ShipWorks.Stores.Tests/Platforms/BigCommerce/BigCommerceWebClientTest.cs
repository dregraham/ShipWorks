using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class BigCommerceWebClientTest
    {
        readonly AutoMock mock;

        public BigCommerceWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiPath()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            store.ApiToken = "token";
            store.ApiUserName = "username";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceAuthenticatorFactory()));
            Assert.Contains("Store API Path is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiUsername()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            store.ApiToken = "token";
            store.ApiUrl = "url";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceAuthenticatorFactory()));
            Assert.Contains("Store API Username is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiToken()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            store.ApiUserName = "username";
            store.ApiUrl = "url";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceAuthenticatorFactory()));
            Assert.Contains("Store API Token is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenOauthAndMissingOauthClientId()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            store.OauthToken = "token";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceAuthenticatorFactory()));
            Assert.Contains("Store API OAuth Client ID is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenOauthAndMissingOauthToken()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            store.OauthClientId = "client id";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceAuthenticatorFactory()));
            Assert.Contains("Store API OAuth Token is missing or invalid", ex.Message);
        }





    }
}
