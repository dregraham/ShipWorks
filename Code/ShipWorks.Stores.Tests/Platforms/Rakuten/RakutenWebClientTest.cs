using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using RestSharp;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Rakuten;
using ShipWorks.Stores.Platforms.Rakuten.DTO;
using ShipWorks.Tests.Shared;
using Xunit;
using It = Moq.It;

namespace ShipWorks.Stores.Tests.Platforms.Rakuten
{
    public class RakutenWebClientTest : IDisposable
    {
        private readonly AutoMock mock;

        public RakutenWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.FromFactory<ILogEntryFactory>()
                .Mock(x => x.GetLogEntry(It.IsAny<ApiLogSource>(), It.IsAny<string>(), It.IsAny<LogActionType>()));

            //logger.Setup(x => x.LogRequest(It.IsAny<IRestRequest>(), It.IsAny<IRestClient>(), It.IsAny<string>()))
            //  .Returns()
        }

        [Theory]
        [InlineData("url", "123456", "123456")]
        [InlineData("url", "", "")]
        public void GetOrders_Succeeds(string shopUrl, string authKey, string marketplaceID)
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1)
            {
                AuthKey = authKey,
                ShopURL = shopUrl,
                MarketplaceID = marketplaceID
            };


            IRestResponse<RakutenOrdersResponse> response = new RestResponse<RakutenOrdersResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null
            };

            mock.FromFactory<IRakutenRestClientFactory>()
                .Mock(x => x.Create(It.IsAny<string>()))
                .Setup(x => x.ExecuteTaskAsync<RakutenOrdersResponse>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response));

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>(TypedParameter.From((IRakutenStoreEntity) store));

            webClient.GetOrders(store, DateTime.Now);
        }

        [Theory]
        [InlineData("url", "123456", "123456")]
        [InlineData("url", "", "")]
        public void ConfirmShipping_Succeeds(string shopUrl, string authKey, string marketplaceID)
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1)
            {
                AuthKey = authKey,
                ShopURL = shopUrl,
                MarketplaceID = marketplaceID
            };

            RakutenOrderEntity order = new RakutenOrderEntity
            {
                RakutenPackageID = ""
            };

            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentTypeCode = Shipping.ShipmentTypeCode.Other,
                Order = order
            };

            IRestResponse<RakutenBaseResponse> response = new RestResponse<RakutenBaseResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null
            };

            mock.FromFactory<IRakutenRestClientFactory>()
                .Mock<IRestClient>(x => x.Create(It.IsAny<string>()))
                .Setup(x => x.ExecuteTaskAsync<RakutenBaseResponse>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response));

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>(TypedParameter.From((IRakutenStoreEntity) store));

            webClient.ConfirmShipping(store, shipment);
        }

        [Theory]
        [InlineData("url", "123456", "123456")]
        [InlineData("url", "", "")]
        public void ConfirmGetProducts_Succeeds(string shopUrl, string authKey, string marketplaceID)
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1)
            {
                AuthKey = authKey,
                ShopURL = shopUrl,
                MarketplaceID = marketplaceID
            };

            IRestResponse<RakutenProductsResponse> response = new RestResponse<RakutenProductsResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null
            };

            mock.FromFactory<IRakutenRestClientFactory>()
                .Mock<IRestClient>(x => x.Create(It.IsAny<string>()))
                .Setup(x => x.ExecuteTaskAsync<RakutenProductsResponse>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response));

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>(TypedParameter.From((IRakutenStoreEntity) store));

            webClient.GetProduct(store, "");
        }

        [Fact]
        public async Task TestConnection_ReturnsTrueWhenNoError()
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1)
            {
                AuthKey = "key",
                ShopURL = "url",
                MarketplaceID = "id"
            };

            IRestResponse<RakutenBaseResponse> response = new RestResponse<RakutenBaseResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null,
            };

            mock.FromFactory<IRakutenRestClientFactory>()
               .Mock(x => x.Create(It.IsAny<string>()))
               .Setup(x => x.ExecuteTaskAsync<RakutenBaseResponse>(It.IsAny<IRestRequest>()))
               .Returns(Task.FromResult(response));

            mock.Mock<IInterapptiveOnly>()
                .Setup(x => x.UseFakeAPI(It.IsAny<string>()))
                .Returns(false);

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>();

            Assert.True(await webClient.TestConnection(store));
        }

        [Fact]
        public async Task TestConnection_ReturnsFalseWhenError()
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1)
            {
                AuthKey = "key",
                ShopURL = "url",
                MarketplaceID = "id"
            };

            mock.Mock<IInterapptiveOnly>()
                .Setup(x => x.UseFakeAPI(It.IsAny<string>()))
                .Returns(false);

            mock.FromFactory<IRakutenRestClientFactory>()
               .Mock<IRestClient>(x => x.Create(It.IsAny<string>()))
               .Setup(x => x.ExecuteTaskAsync<RakutenBaseResponse>(It.IsAny<IRestRequest>()))
               .Throws(new WebException());

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>(TypedParameter.From((IRakutenStoreEntity) store));

            Assert.False(await webClient.TestConnection(store));
        }


        [Fact]
        public async Task GetOrders_ThrowsIfErrorResponse()
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1)
            {
                AuthKey = "",
                ShopURL = "",
                MarketplaceID = ""
            };

            IRestResponse<RakutenOrdersResponse> response = new RestResponse<RakutenOrdersResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null
            };

            response.Data = new RakutenOrdersResponse
            {
                Errors = new RakutenErrors()
            };

            Mock<IRestClient> client = mock.FromFactory<IRakutenRestClientFactory>()
                .Mock(x => x.Create(It.IsAny<string>()));

            client.Setup(x => x.ExecuteTaskAsync<RakutenOrdersResponse>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response));

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>(TypedParameter.From((IRakutenStoreEntity) store));

            await Assert.ThrowsAsync<WebException>(() => webClient.GetOrders(store, DateTime.Now));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
