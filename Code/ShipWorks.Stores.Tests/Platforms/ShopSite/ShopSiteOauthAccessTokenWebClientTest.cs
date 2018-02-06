﻿using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Stores.Tests.Platforms.ShopSite.Responses;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ShopSite
{
    public class ShopSiteOauthAccessTokenWebClientTest
    {
        private readonly AutoMock mock;

        public ShopSiteOauthAccessTokenWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData("", "clientID", "secretKey", "authCode", "Authorization URL")]
        [InlineData("www.com", "", "secretKey", "authCode", "Client ID")]
        [InlineData("www.com", "clientID", "", "authCode", "Secret Key")]
        [InlineData("www.com", "clientID", "secretKey", "", "Authorization Code")]
        public void FetchAuthAccessResponse_ReturnsException_WhenOauthAuthAndMissingInfo(string url, string clientID, string secretKey, string authCode, string expectedValue)
        {
            IShopSiteStoreEntity store = new ShopSiteStoreEntity(1)
            {
                StoreName = "Test store",
                ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth,
                ApiUrl = url,
                OauthClientID = clientID,
                OauthSecretKey = secretKey,
                OauthAuthorizationCode = authCode
            };

            var testObject = mock.Create<ShopSiteOauthAccessTokenWebClient>(TypedParameter.From(store));

            var ex = Assert.Throws<ShopSiteException>(() => testObject.FetchAuthAccessResponse());

            Assert.Contains($"{expectedValue} is missing or invalid", ex.Message);
        }

        [Fact]
        public void FetchAuthAccessResponse_ReturnsException_WhenStoreIsBasicAuth()
        {
            IShopSiteStoreEntity store = new ShopSiteStoreEntity(1)
            {
                StoreName = "Test store",
                ShopSiteAuthentication = ShopSiteAuthenticationType.Basic
            };

            var testObject = mock.Create<ShopSiteOauthAccessTokenWebClient>(TypedParameter.From(store));

            ShopSiteException ex = Assert.Throws<ShopSiteException>(() => testObject.FetchAuthAccessResponse());

            Assert.Contains("configured to use Basic authentication but the OAuth", ex.Message);
        }

        [Fact]
        public void FetchAuthAccessResponse_ReturnsException_WhenInvalidAccessResponseReceived()
        {
            mock.Mock<IDatabaseSpecificEncryptionProvider>().SetReturnsDefault("secretKey");

            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.OauthSecretKey = "encrypted_secretKey";
            store.OauthClientID = "clientID";
            store.OauthAuthorizationCode = "authCode";

            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(x => x.ReadResult())
                .Returns(ShopSiteResponseHelper.GetEmptyAccessTokenResponse);

            Mock<IHttpVariableRequestSubmitter> submitter = mock.Mock<IHttpVariableRequestSubmitter>();
            submitter.Setup(x => x.GetResponse())
                .Returns(responseReader.Object);

            submitter.Setup(s => s.Uri).Returns(new Uri(store.ApiUrl));

            IShopSiteOauthAccessTokenWebClient webClient = CreateWebClient(submitter, store);

            Assert.Throws<ShopSiteException>(() => webClient.FetchAuthAccessResponse());
        }

        [Fact]
        public void FetchAuthAccessResponse_DelegatesToHttpVariableRequestSubmitter()
        {
            mock.Mock<IDatabaseSpecificEncryptionProvider>().SetReturnsDefault("secretKey");

            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.OauthSecretKey = "encrypted_secretKey";
            store.OauthClientID = "clientID";
            store.OauthAuthorizationCode = "authCode";

            Queue<string> responses = new Queue<string>();
            responses.Enqueue(ShopSiteResponseHelper.GetAccessTokenResponse());

            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(x => x.ReadResult())
                .Returns(responses.Dequeue);

            Mock<IHttpVariableRequestSubmitter> submitter = mock.Mock<IHttpVariableRequestSubmitter>();
            submitter.Setup(x => x.GetResponse())
                .Returns(responseReader.Object);

            submitter.Setup(s => s.Uri).Returns(new Uri(store.ApiUrl));

            IShopSiteOauthAccessTokenWebClient webClient = CreateWebClient(submitter, store);

            webClient.FetchAuthAccessResponse();

            submitter.Verify(s => s.GetResponse(), Times.Exactly(1));
        }

        [Fact]
        public void FetchAuthAccessResponse_AddsHttpVariableInTheCorrectOrder()
        {
            mock.Mock<IDatabaseSpecificEncryptionProvider>().SetReturnsDefault("secretKey");

            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.OauthSecretKey = "encrypted_secretKey";
            store.OauthClientID = "clientID";
            store.OauthAuthorizationCode = "authCode";

            Mock<IHttpVariableRequestSubmitter> submitter = CreateMockedSubmitter(ShopSiteResponseHelper.GetAccessTokenResponse(), store);

            int callOrder = 0;
            submitter.Setup(x => x.Variables.Add("grant_type", "authorization_code")).Callback(() => Assert.Equal(0, callOrder++));
            submitter.Setup(x => x.Variables.Add("code", store.OauthAuthorizationCode)).Callback(() => Assert.Equal(1, callOrder++));
            submitter.Setup(x => x.Variables.Add("client_credentials", It.IsAny<string>())).Callback(() => Assert.Equal(2, callOrder++));
            submitter.Setup(x => x.Variables.Add("signature", It.IsAny<string>())).Callback(() => Assert.Equal(3, callOrder++));

            IShopSiteOauthAccessTokenWebClient webClient = CreateWebClient(submitter, store);

            webClient.FetchAuthAccessResponse();

            submitter.Verify(s => s.Variables.Add("grant_type", "authorization_code"), Times.Once);
            submitter.Verify(s => s.Variables.Add("code", store.OauthAuthorizationCode), Times.Once);
            submitter.Verify(s => s.Variables.Add("client_credentials", It.IsAny<string>()), Times.Once);
            submitter.Verify(s => s.Variables.Add("signature", It.IsAny<string>()), Times.Once);

            submitter.Verify(s => s.Variables.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(4));
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

        private IShopSiteOauthAccessTokenWebClient CreateWebClient(Mock<IHttpVariableRequestSubmitter> submitter, ShopSiteStoreEntity store)
        {
            return mock.Create<ShopSiteOauthAccessTokenWebClient>(TypedParameter.From((IShopSiteStoreEntity) store),
                TypedParameter.From(submitter.Object));
        }
    }
}
