using System;
using System.Collections.Generic;
using System.Net;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Moq;
using RestSharp;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication.Throttling;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Tests.Shared;
using Xunit;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class BigCommerceWebClientTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IApiLogEntry> mockedLogger;
        private readonly Mock<ILogEntryFactory> mockedLogFactory;

        public BigCommerceWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            //Setup Logger
            mockedLogger = new Mock<IApiLogEntry>();

            mockedLogFactory = new Mock<ILogEntryFactory>();
            mockedLogFactory
                .Setup(f => f.GetLogEntry(It.IsAny<ApiLogSource>(), It.IsAny<string>(), It.IsAny<LogActionType>()))
                .Returns(mockedLogger.Object);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiPath()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            store.ApiToken = "token";
            store.ApiUserName = "username";

            BigCommerceAuthenticatorFactory authenticatorFactory = new BigCommerceAuthenticatorFactory();
            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceRestClientFactory(authenticatorFactory), mockedLogFactory.Object));
            Assert.Contains("Store API Path is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiUsername()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            store.ApiToken = "token";
            store.ApiUrl = "url";

            BigCommerceAuthenticatorFactory authenticatorFactory = new BigCommerceAuthenticatorFactory();
            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceRestClientFactory(authenticatorFactory), mockedLogFactory.Object));
            Assert.Contains("Store API Username is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiToken()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            store.ApiUserName = "username";
            store.ApiUrl = "url";

            BigCommerceAuthenticatorFactory authenticatorFactory = new BigCommerceAuthenticatorFactory();
            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceRestClientFactory(authenticatorFactory), mockedLogFactory.Object));
            Assert.Contains("Store API Token is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenOauthAndMissingOauthClientId()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            store.ApiUrl = "url";
            store.OauthToken = "token";

            BigCommerceAuthenticatorFactory authenticatorFactory = new BigCommerceAuthenticatorFactory();
            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceRestClientFactory(authenticatorFactory), mockedLogFactory.Object));
            Assert.Contains("Store API OAuth Client ID is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenOauthAndMissingOauthToken()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            store.ApiUrl = "url";
            store.OauthClientId = "client id";

            BigCommerceAuthenticatorFactory authenticatorFactory = new BigCommerceAuthenticatorFactory();
            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceRestClientFactory(authenticatorFactory), mockedLogFactory.Object));
            Assert.Contains("Store API OAuth Token is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenOauthAndMissingBasicAuthApiPath()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            store.OauthClientId = "client id";
            store.OauthToken = "token";

            BigCommerceAuthenticatorFactory authenticatorFactory = new BigCommerceAuthenticatorFactory();
            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, new BigCommerceRestClientFactory(authenticatorFactory), mockedLogFactory.Object));
            Assert.Contains("Store API Path is missing or invalid", ex.Message);
        }

        [Theory]
        [InlineData(BigCommerceAuthenticationType.Basic, "url", "BasicUsername", "BasicToken", "", "")]
        [InlineData(BigCommerceAuthenticationType.Oauth, "url", "", "", "OAuthClientID", "OAuthToken")]
        public void UpdateOrderStatus_Succeeds(BigCommerceAuthenticationType authenticationType, string url, string username, string basicToken, string clientID, string oAuthToken)
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = authenticationType;
            store.ApiToken = basicToken;
            store.ApiUserName = username;
            store.ApiUrl = url;
            store.OauthToken = oAuthToken;
            store.OauthClientId = clientID;

            IRestResponse response = new RestResponse()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null
            };

            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(c => c.Execute(It.IsAny<IRestRequest>())).Returns(response);
            mock.Provide(restClient.Object);

            IBigCommerceAuthenticatorFactory authenticatorFactory = new BigCommerceAuthenticatorFactory();

            Mock<IBigCommerceRestClientFactory> restClientFactory = new Mock<IBigCommerceRestClientFactory>();
            restClientFactory.Setup(f => f.Create(It.IsAny<IBigCommerceStoreEntity>())).Returns(restClient.Object);
            mock.Provide(restClientFactory);

            IBigCommerceWebClient webClient = mock.Create<BigCommerceWebClient>(new[]
            {
                new TypedParameter(typeof(IBigCommerceStoreEntity), store),
                new TypedParameter(typeof(IBigCommerceAuthenticatorFactory), authenticatorFactory),
                new TypedParameter(typeof(IBigCommerceRestClientFactory), restClientFactory.Object),
                new TypedParameter(typeof(ILogEntryFactory), mockedLogFactory.Object),
            });


            webClient.UpdateOrderStatus(1, 1);
        }

        [Theory]
        [InlineData(BigCommerceAuthenticationType.Basic, "url", "BasicUsername", "BasicToken", "", "")]
        [InlineData(BigCommerceAuthenticationType.Oauth, "url", "", "", "OAuthClientID", "OAuthToken")]
        public void UploadOrderShipmentDetails_Succeeds(BigCommerceAuthenticationType authenticationType, string url, string username, string basicToken, string clientID, string oAuthToken)
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = authenticationType;
            store.ApiToken = basicToken;
            store.ApiUserName = username;
            store.ApiUrl = url;
            store.OauthToken = oAuthToken;
            store.OauthClientId = clientID;

            IRestResponse response = new RestResponse()
            {
                StatusCode = HttpStatusCode.Created,
                ErrorException = null
            };

            Mock<IBigCommerceAuthenticatorFactory> authenticatorFactory = new Mock<IBigCommerceAuthenticatorFactory>();
            mock.Provide(authenticatorFactory);

            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(c => c.Execute(It.IsAny<IRestRequest>())).Returns(response);
            mock.Provide(restClient.Object);


            Mock<IBigCommerceRestClientFactory> restClientFactory = new Mock<IBigCommerceRestClientFactory>();
            restClientFactory.Setup(f => f.Create(It.IsAny<IBigCommerceStoreEntity>())).Returns(restClient.Object);
            mock.Provide(restClientFactory);

            IBigCommerceWebClient webClient = mock.Create<BigCommerceWebClient>(new[]
            {
                new TypedParameter(typeof(IBigCommerceStoreEntity), store),
                new TypedParameter(typeof(IBigCommerceAuthenticatorFactory), authenticatorFactory.Object),
                new TypedParameter(typeof(IBigCommerceRestClientFactory), restClientFactory.Object),
                new TypedParameter(typeof(ILogEntryFactory), mockedLogFactory.Object),
            });

            webClient.UploadOrderShipmentDetails(1, 1, "asdf", new Tuple<string, string>("1", "2"), new List<BigCommerceItem>());
        }
    }
}
