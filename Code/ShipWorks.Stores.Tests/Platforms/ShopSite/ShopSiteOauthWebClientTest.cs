using System;
using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Stores.Platforms.ShopSite.Dto;
using ShipWorks.Stores.Tests.Platforms.ShopSite.Responses;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ShopSite
{
    public class ShopSiteOauthWebClientTest
    {
        private readonly AutoMock mock;
        private readonly AccessResponse accessResponse;

        public ShopSiteOauthWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            accessResponse = ShopSiteResponseHelper.GetAccessTokenJson();
        }

        [Fact]
        public void Constructor_ReturnsException_WhenStoreIsBasicAuth()
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Basic;

            ShopSiteException ex = Assert.Throws<ShopSiteException>(() =>
                new ShopSiteOauthWebClient(store,
                    mock.Create<IDatabaseSpecificEncryptionProvider>(),
                    (s) => mock.Create<IShopSiteOauthAccessTokenWebClient>(TypedParameter.From(s)),
                    () => new HttpVariableRequestSubmitter(),
                    mock.Create<ILogEntryFactory>()));

            Assert.Contains("configured to use Basic authentication but the OAuth", ex.Message);

        }

        [Theory]
        [InlineData("", "clientID", "secretKey", "authCode", "Authorization URL")]
        [InlineData("www.com", "", "secretKey", "authCode", "Client ID")]
        [InlineData("www.com", "clientID", "", "authCode", "Secret Key")]
        [InlineData("www.com", "clientID", "secretKey", "", "Authorization Code")]
        public void Constructor_ReturnsException_WhenOauthAuthAndMissingInfo(string url, string clientID, string secretKey, string authCode, string expectedValue)
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;
            store.ApiUrl = url;
            store.OauthClientID = clientID;
            store.OauthSecretKey = secretKey;
            store.OauthAuthorizationCode = authCode;

            DependencyResolutionException exOuter = Assert.Throws<DependencyResolutionException>(() =>
                mock.Create<ShopSiteOauthWebClient>(TypedParameter.From<IShopSiteStoreEntity>(store)));
            ShopSiteException ex = exOuter.GetBaseException() as ShopSiteException;
            Assert.Contains($"{expectedValue} is missing or invalid", ex.Message);
        }

        [Fact]
        public void TestConnection_DelegatesToHttpVariableRequestSubmitter()
        {
            mock.Mock<IDatabaseSpecificEncryptionProvider>().SetReturnsDefault("secretKey");

            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.OauthSecretKey = "encrypted_secretKey";
            store.OauthClientID = "clientID";
            store.OauthAuthorizationCode = "authCode";

            Mock<IHttpVariableRequestSubmitter> submitter = CreateMockedSubmitter(ShopSiteResponseHelper.GetTestConnectionXml(), store);
            IShopSiteWebClient webClient = CreateWebClient(submitter, store);

            webClient.TestConnection();

            submitter.Verify(s => s.GetResponse(), Times.Exactly(1));
        }

        [Fact]
        public void TestConnection_AddsHttpVariableInTheCorrectOrder()
        {
            mock.Mock<IDatabaseSpecificEncryptionProvider>().SetReturnsDefault("secretKey");

            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.OauthSecretKey = "encrypted_secretKey";
            store.OauthClientID = "clientID";
            store.OauthAuthorizationCode = "authCode";

            Mock<IHttpVariableRequestSubmitter> submitter = CreateMockedSubmitter(ShopSiteResponseHelper.GetTestConnectionXml(), store);

            int callOrder = 0;
            submitter.Setup(x => x.Variables.Add("clientApp", "1")).Callback(() => Assert.Equal(callOrder++, 0));
            submitter.Setup(x => x.Variables.Add("dbname", "orders")).Callback(() => Assert.Equal(callOrder++, 1));
            submitter.Setup(x => x.Variables.Add("maxorder", "1")).Callback(() => Assert.Equal(callOrder++, 2));
            submitter.Setup(x => x.Variables.Add("signature", It.IsAny<string>())).Callback(() => Assert.Equal(callOrder++, 3));
            submitter.Setup(x => x.Variables.Add("token", accessResponse.access_token)).Callback(() => Assert.Equal(callOrder++, 4));
            submitter.Setup(x => x.Variables.Add("timestamp", It.IsAny<string>())).Callback(() => Assert.Equal(callOrder++, 5));
            submitter.Setup(x => x.Variables.Add("nonce", It.IsAny<string>())).Callback(() => Assert.Equal(callOrder++, 6));

            IShopSiteWebClient webClient = CreateWebClient(submitter, store);

            webClient.TestConnection();

            submitter.Verify(s => s.Variables.Add("clientApp", "1"), Times.Once);
            submitter.Verify(s => s.Variables.Add("dbname", "orders"), Times.Once);
            submitter.Verify(s => s.Variables.Add("maxorder", "1"), Times.Once);
            submitter.Verify(s => s.Variables.Add("signature", It.IsAny<string>()), Times.Once);
            submitter.Verify(s => s.Variables.Add("token", accessResponse.access_token), Times.Once);
            submitter.Verify(s => s.Variables.Add("timestamp", It.IsAny<string>()), Times.Once);
            submitter.Verify(s => s.Variables.Add("nonce", It.IsAny<string>()), Times.Once);

            submitter.Verify(s => s.Variables.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(7));
        }

        [Fact]
        public void GetOrders_DelegatesToHttpVariableRequestSubmitter()
        {
            mock.Mock<IDatabaseSpecificEncryptionProvider>().SetReturnsDefault("secretKey");

            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.OauthSecretKey = "encrypted_secretKey";
            store.OauthClientID = "clientID";
            store.OauthAuthorizationCode = "authCode";

            Mock<IHttpVariableRequestSubmitter> submitter = CreateMockedSubmitter(ShopSiteResponseHelper.GetOrdersXml(), store);

            IShopSiteWebClient webClient = CreateWebClient(submitter, store);

            webClient.GetOrders(0);

            submitter.Verify(s => s.GetResponse(), Times.Once);
        }

        [Fact]
        public void GetOrders_AddsHttpVariableInTheCorrectOrder()
        {
            mock.Mock<IDatabaseSpecificEncryptionProvider>().SetReturnsDefault("secretKey");

            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.OauthSecretKey = "encrypted_secretKey";
            store.OauthClientID = "clientID";
            store.OauthAuthorizationCode = "authCode";

            Mock<IHttpVariableRequestSubmitter> submitter = CreateMockedSubmitter(ShopSiteResponseHelper.GetOrdersXml(), store);

            int callOrder = 0;
            submitter.Setup(x => x.Variables.Add("clientApp", "1")).Callback(() => Assert.Equal(0, callOrder++));
            submitter.Setup(x => x.Variables.Add("dbname", "orders")).Callback(() => Assert.Equal(1, callOrder++));
            submitter.Setup(x => x.Variables.Add("maxorder", store.DownloadPageSize.ToString())).Callback(() => Assert.Equal(2, callOrder++));
            submitter.Setup(x => x.Variables.Add("pay", "no_cvv")).Callback(() => Assert.Equal(3, callOrder++));
            submitter.Setup(x => x.Variables.Add("startorder", "0")).Callback(() => Assert.Equal(4, callOrder++));
            submitter.Setup(x => x.Variables.Add("signature", It.IsAny<string>())).Callback(() => Assert.Equal(5, callOrder++));
            submitter.Setup(x => x.Variables.Add("token", accessResponse.access_token)).Callback(() => Assert.Equal(6, callOrder++));
            submitter.Setup(x => x.Variables.Add("timestamp", It.IsAny<string>())).Callback(() => Assert.Equal(7, callOrder++));
            submitter.Setup(x => x.Variables.Add("nonce", It.IsAny<string>())).Callback(() => Assert.Equal(8, callOrder++));

            IShopSiteWebClient webClient = CreateWebClient(submitter, store);

            webClient.GetOrders(0);

            submitter.Verify(s => s.Variables.Add("clientApp", "1"), Times.Once);
            submitter.Verify(s => s.Variables.Add("dbname", "orders"), Times.Once);
            submitter.Verify(s => s.Variables.Add("maxorder", store.DownloadPageSize.ToString()), Times.Once);
            submitter.Verify(s => s.Variables.Add("pay", "no_cvv"), Times.Once);
            submitter.Verify(s => s.Variables.Add("startorder", "0"), Times.Once);
            submitter.Verify(s => s.Variables.Add("signature", It.IsAny<string>()), Times.Once);
            submitter.Verify(s => s.Variables.Add("token", accessResponse.access_token), Times.Once);
            submitter.Verify(s => s.Variables.Add("timestamp", It.IsAny<string>()), Times.Once);
            submitter.Verify(s => s.Variables.Add("nonce", It.IsAny<string>()), Times.Once);

            submitter.Verify(s => s.Variables.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(9));
        }

        private Mock<IHttpVariableRequestSubmitter> CreateMockedSubmitter(string response, ShopSiteStoreEntity store)
        {
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(x => x.ReadResult())
                .Returns(response);

            Mock<IHttpVariableRequestSubmitter> submitter = mock.Mock<IHttpVariableRequestSubmitter>();
            submitter.Setup(x => x.GetResponse())
                .Returns(responseReader.Object);

            submitter.Setup(s => s.Uri).Returns(new Uri(store.ApiUrl));

            return submitter;
        }

        private IShopSiteWebClient CreateWebClient(Mock<IHttpVariableRequestSubmitter> submitter, ShopSiteStoreEntity store)
        {
            Mock<IShopSiteOauthAccessTokenWebClient> accessTokenWebClient = mock.Mock<IShopSiteOauthAccessTokenWebClient>();
            accessTokenWebClient.Setup(a => a.FetchAuthAccessResponse())
                .Returns(accessResponse);

            return mock.Create<ShopSiteOauthWebClient>(TypedParameter.From((IShopSiteStoreEntity) store),
                TypedParameter.From(submitter.Object),
                TypedParameter.From(accessTokenWebClient.Object));
        }
    }
}
