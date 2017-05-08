using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ShopSite
{
    public class ShopSiteWebClientTest
    {
        private readonly AutoMock mock;

        public ShopSiteWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData("",        "username", "password", "CGI Path")]
        [InlineData("www.com", "",         "password", "Merchant ID")]
        [InlineData("www.com", "username", "",         "Password")]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiPath(string url, string username, string pwd, string expectedValue)
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.Authentication = ShopSiteAuthenticationType.Basic;
            store.ApiUrl = url;
            store.Password = pwd;
            store.Username = username;

            ShopSiteException ex = Assert.Throws<ShopSiteException>(() => new ShopSiteWebClient(store));
            Assert.Contains($"{expectedValue} is missing or invalid", ex.Message);
        }

        [Theory]
        [InlineData("",        "clientID", "secretKey", "authCode", "Authorization URL")]
        [InlineData("www.com", "",         "secretKey", "authCode", "Client ID")]
        [InlineData("www.com", "clientID", "",          "authCode", "Secret Key")]
        [InlineData("www.com", "clientID", "secretKey", "",         "Authorization Code")]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiPath(string url, string clientID, string secretKey, string authCode, string expectedValue)
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.Authentication = ShopSiteAuthenticationType.Oauth;
            store.ApiUrl = url;
            store.OauthClientID = clientID;
            store.OauthSecretKey = secretKey;
            store.AuthorizationCode = authCode;

            ShopSiteException ex = Assert.Throws<ShopSiteException>(() => new ShopSiteWebClient(store));
            Assert.Contains($"{expectedValue} is missing or invalid", ex.Message);
        }
        
    }
}
