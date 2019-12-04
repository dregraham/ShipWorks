using System;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using RestSharp;
using ShipWorks.Data.Model.EntityClasses;
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
        private readonly RakutenStoreEntity store;

        public RakutenWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            store = new RakutenStoreEntity(1)
            {
                AuthKey = "test",
                ShopURL = "test",
                MarketplaceID = "test"
            };
        }

        [Fact]
        public async Task GetOrders_ReturnsResponse_WhenNoErrors()
        {
            IRestResponse<RakutenOrdersResponse> response = new RestResponse<RakutenOrdersResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null,
                Data = new RakutenOrdersResponse
                {
                    Errors = null
                }
            };

            mock.FromFactory<IRakutenRestClientFactory>()
                .Mock(x => x.Create(It.IsAny<string>()))
                .Setup(x => x.ExecuteTaskAsync<RakutenOrdersResponse>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response);

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>();

            RakutenOrdersResponse testResponse = await webClient.GetOrders(store, DateTime.Now);

            Assert.True(testResponse.Equals(response.Data));
        }

        [Fact]
        public async Task ConfirmShipping_ReturnsResponse_WhenNoErrors()
        {
            RakutenOrderEntity order = new RakutenOrderEntity
            {
                RakutenPackageID = ""
            };

            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentTypeCode = Shipping.ShipmentTypeCode.FedEx,
                Order = order
            };

            IRestResponse<RakutenBaseResponse> response = new RestResponse<RakutenBaseResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null,
                Data = new RakutenBaseResponse
                {
                    Errors = null
                }
            };

            mock.FromFactory<IRakutenRestClientFactory>()
                .Mock(x => x.Create(It.IsAny<string>()))
                .Setup(x => x.ExecuteTaskAsync<RakutenBaseResponse>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response);

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>();

            RakutenBaseResponse testResponse = await webClient.ConfirmShipping(store, shipment);

            Assert.True(testResponse.Equals(response.Data));
        }

        [Fact]
        public async Task ConfirmGetProducts_ReturnsResponse_WhenNoErrors()
        {
            IRestResponse<RakutenProductsResponse> response = new RestResponse<RakutenProductsResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null,
                Data = new RakutenProductsResponse
                {
                    Errors = null
                }
            };

            mock.FromFactory<IRakutenRestClientFactory>()
                .Mock(x => x.Create(It.IsAny<string>()))
                .Setup(x => x.ExecuteTaskAsync<RakutenProductsResponse>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response));

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>();

            RakutenProductsResponse testResponse = await webClient.GetProduct(store, "");

            Assert.True(testResponse.Equals(response.Data));
        }

        [Fact]
        public async Task TestConnection_ReturnsTrueWhenNoError()
        {
            IRestResponse<RakutenBaseResponse> response = new RestResponse<RakutenBaseResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null,
                Data = mock.Create<RakutenBaseResponse>()
            };

            mock.FromFactory<IRakutenRestClientFactory>()
               .Mock(x => x.Create(It.IsAny<string>()))
               .Setup(x => x.ExecuteTaskAsync<RakutenBaseResponse>(It.IsAny<IRestRequest>()))
               .Returns(Task.FromResult(response));

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>();

            Interapptive.Shared.Utility.GenericResult<bool> result = await webClient.TestConnection(store);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task TestConnection_ReturnsFalseWhenError()
        {
            mock.FromFactory<IRakutenRestClientFactory>()
               .Mock(x => x.Create(It.IsAny<string>()))
               .Setup(x => x.ExecuteTaskAsync<RakutenBaseResponse>(It.IsAny<IRestRequest>()))
               .Throws(new WebException());

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>();

            Interapptive.Shared.Utility.GenericResult<bool> result = await webClient.TestConnection(store);

            Assert.False(result.Success);
        }


        [Fact]
        public async Task GetOrders_ThrowsIfErrorResponse()
        {
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

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>();

            await Assert.ThrowsAsync<WebException>(() => webClient.GetOrders(store, DateTime.Now));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
