using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Tests.Shared;
using Xunit;
using Autofac;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Tests.Platforms.ShopSite.Responses;


namespace ShipWorks.Stores.Tests.Platforms.ShopSite
{
    public class ShopSiteWebClientTest
    {
        private readonly AutoMock mock;

        public ShopSiteWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_ReturnsException_WhenStoreIsBasicOauth()
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;

            ShopSiteException ex = Assert.Throws<ShopSiteException>(() => new ShopSiteWebClient(store,
                mock.Create<IEncryptionProviderFactory>(),
                () => mock.Create<IHttpVariableRequestSubmitter>(),
                (x, y) => mock.Create<IApiLogEntry>()));

            Assert.Contains("configured to use OAuth authentication", ex.Message);

        }

        [Theory]
        [InlineData("",        "username", "password", "CGI Path")]
        [InlineData("www.com", "",         "password", "Merchant ID")]
        [InlineData("www.com", "username", "",         "Password")]
        public void Constructor_ReturnsException_WhenBasicAuthAndMissingInfo(string url, string username, string pwd, string expectedValue)
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Basic;
            store.ApiUrl = url;
            store.Password = pwd;
            store.Username = username;

            ShopSiteException ex = Assert.Throws<ShopSiteException>(() => new ShopSiteWebClient(store,
                 mock.Create<IEncryptionProviderFactory>(),
                () => mock.Create<IHttpVariableRequestSubmitter>(),
                (x, y) => mock.Create<IApiLogEntry>()));

            Assert.Contains($"{expectedValue} is missing or invalid", ex.Message);
        }

        [Fact]
        public void TestConnection_DelegatesToHttpVariableRequestSubmitter()
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Basic;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.Username = "username";
            store.Password = "pwd";
            store.RequireSSL = true;

            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(x => x.ReadResult())
                .Returns(ShopSiteResponseHelper.GetTestConnectionXml());

            Mock<IHttpVariableRequestSubmitter> submitter = mock.Mock<IHttpVariableRequestSubmitter>();
            submitter.Setup(x => x.GetResponse())
                .Returns(responseReader.Object);

            IShopSiteWebClient webClient = mock.Create<ShopSiteWebClient>(TypedParameter.From(store as IShopSiteStoreEntity));

            webClient.TestConnection();

            submitter.Verify(s => s.GetResponse(), Times.Once);
        }

        [Fact]
        public void TestConnection_AddsHttpVariableInTheCorrectOrder()
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Basic;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.Username = "username";
            store.Password = "pwd";
            store.RequireSSL = true;

            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(x => x.ReadResult())
                .Returns(ShopSiteResponseHelper.GetTestConnectionXml());

            Mock<IHttpVariableRequestSubmitter> submitter = mock.Mock<IHttpVariableRequestSubmitter>();
            submitter.Setup(x => x.GetResponse())
                .Returns(responseReader.Object);

            int callOrder = 0;
            submitter.Setup(x => x.Variables.Add("maxorder", "1")).Callback(() => Assert.Equal(callOrder++, 0));
            submitter.Setup(x => x.Variables.Add("version", "12.0")).Callback(() => Assert.Equal(callOrder++, 1));

            IShopSiteWebClient webClient = mock.Create<ShopSiteWebClient>(TypedParameter.From(store as IShopSiteStoreEntity));

            webClient.TestConnection();

            submitter.Verify(s => s.Variables.Add("maxorder", "1"), Times.Once);
            submitter.Verify(s => s.Variables.Add("version", "12.0"), Times.Once);
            submitter.Verify(s => s.Variables.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void GetOrders_DelegatesToHttpVariableRequestSubmitter()
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Basic;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.Username = "username";
            store.Password = "pwd";
            store.RequireSSL = true;

            string getOrdersXml = ShopSiteResponseHelper.GetOrdersXml();

            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(x => x.ReadResult())
                .Returns(getOrdersXml);

            Mock<IHttpVariableRequestSubmitter> submitter = mock.Mock<IHttpVariableRequestSubmitter>();
            submitter.Setup(x => x.GetResponse())
                .Returns(responseReader.Object);

            IShopSiteWebClient webClient = mock.Create<ShopSiteWebClient>(TypedParameter.From(store as IShopSiteStoreEntity));

            webClient.GetOrders(0);

            submitter.Verify(s => s.GetResponse(), Times.Once);
        }

        [Fact]
        public void GetOrders_AddsHttpVariableInTheCorrectOrder()
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity(1);
            store.StoreName = "Test store";
            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Basic;
            store.ApiUrl = "https://www.foo.com/authorize.cgi";
            store.Username = "username";
            store.Password = "pwd";
            store.RequireSSL = true;

            string getOrdersXml = ShopSiteResponseHelper.GetOrdersXml();

            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(x => x.ReadResult())
                .Returns(getOrdersXml);

            Mock<IHttpVariableRequestSubmitter> submitter = mock.Mock<IHttpVariableRequestSubmitter>();
            submitter.Setup(x => x.GetResponse())
                .Returns(responseReader.Object);

            int callOrder = 0;
            submitter.Setup(x => x.Variables.Add("maxorder", store.DownloadPageSize.ToString())).Callback(() => Assert.Equal(0, callOrder++));
            submitter.Setup(x => x.Variables.Add("pay", "yes")).Callback(() => Assert.Equal(1, callOrder++));
            submitter.Setup(x => x.Variables.Add("startorder", "0")).Callback(() => Assert.Equal(2, callOrder++));
            submitter.Setup(x => x.Variables.Add("version", "12.0")).Callback(() => Assert.Equal(3, callOrder++));

            IShopSiteWebClient webClient = mock.Create<ShopSiteWebClient>(TypedParameter.From(store as IShopSiteStoreEntity));

            webClient.GetOrders(0);

            submitter.Verify(s => s.GetResponse(), Times.Once);
            submitter.Verify(s => s.Variables.Add("maxorder", store.DownloadPageSize.ToString()), Times.Once);
            submitter.Verify(s => s.Variables.Add("pay", "yes"), Times.Once);
            submitter.Verify(s => s.Variables.Add("startorder", "0"), Times.Once);
            submitter.Verify(s => s.Variables.Add("version", "12.0"), Times.Once);
            submitter.Verify(s => s.Variables.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(4));
        }
    }
}
