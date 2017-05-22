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
using ShipWorks.Stores.Platforms.BigCommerce.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class BigCommerceWebClientTest
    {
        private readonly AutoMock mock;
        private readonly ILogEntryFactory logFactory;

        public BigCommerceWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            logFactory = mock.Create<ILogEntryFactory>();
        }

        [Fact]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiPath()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            store.ApiToken = "token";
            store.ApiUserName = "username";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, mock.Create<BigCommerceRestClientFactory>(), logFactory));
            Assert.Contains("Store API Path is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiUsername()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            store.ApiToken = "token";
            store.ApiUrl = "url";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, mock.Create<BigCommerceRestClientFactory>(), logFactory));
            Assert.Contains("Store API Username is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingBasicAuthApiToken()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            store.ApiUserName = "username";
            store.ApiUrl = "url";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, mock.Create<BigCommerceRestClientFactory>(), logFactory));
            Assert.Contains("Store API Token is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenOauthAndMissingOauthClientId()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            store.ApiUrl = "url";
            store.OauthToken = "token";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, mock.Create<BigCommerceRestClientFactory>(), logFactory));
            Assert.Contains("Store API OAuth Client ID is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenOauthAndMissingOauthToken()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            store.ApiUrl = "url";
            store.OauthClientId = "client id";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, mock.Create<BigCommerceRestClientFactory>(), logFactory));
            Assert.Contains("Store API OAuth Token is missing or invalid", ex.Message);
        }

        [Fact]
        public void Constructor_ReturnsException_WhenOauthAndMissingBasicAuthApiPath()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            store.OauthClientId = "client id";
            store.OauthToken = "token";

            BigCommerceException ex = Assert.Throws<BigCommerceException>(() => new BigCommerceWebClient(store, mock.Create<BigCommerceRestClientFactory>(), logFactory));
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

            mock.FromFactory<IBigCommerceRestClientFactory>()
                .Mock<IRestClient>(x => x.Create(It.IsAny<IBigCommerceStoreEntity>()))
                .Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(response);

            IBigCommerceWebClient webClient = mock.Create<BigCommerceWebClient>(TypedParameter.From((IBigCommerceStoreEntity) store));

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

            mock.FromFactory<IBigCommerceRestClientFactory>()
                .Mock<IRestClient>(x => x.Create(It.IsAny<IBigCommerceStoreEntity>()))
                .Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(response);

            IBigCommerceWebClient webClient = mock.Create<BigCommerceWebClient>(TypedParameter.From((IBigCommerceStoreEntity) store));

            webClient.UploadOrderShipmentDetails(1, 1, "asdf", new Tuple<string, string>("1", "2"), new List<BigCommerceItem>());
        }
    }
}
