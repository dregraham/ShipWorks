using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Rakuten;
using ShipWorks.Stores.Platforms.Rakuten.DTO;
using ShipWorks.Stores.Warehouse.StoreData;
using ShipWorks.Tests.Shared;
using Xunit;
using It = Moq.It;

namespace ShipWorks.Stores.Tests.Platforms.Rakuten
{
    public class RakutenWebClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<Uri> uri;
        private readonly Mock<NullApiLogEntry> logger;
        public RakutenWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            uri = new Mock<Uri>("https://openapi-rms.global.rakuten.com/2.0");

            logger = mock.CreateMock<NullApiLogEntry>();

            var logFactory = mock.CreateMock<Func<ApiLogSource, string, IApiLogEntry>>();
            logFactory.Setup(f => f(It.IsAny<ApiLogSource>(), It.IsAny<string>()))
                .Returns(logger.Object);
            mock.Provide(logFactory.Object);

        }
        
        [Theory]
        [InlineData( "url", "123456", "123456")]
        [InlineData( "url", "", "")]
        public void GetOrders_Succeeds(string shopUrl, string authKey, string marketplaceID)
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1);
            store.AuthKey = authKey;
            store.ShopURL = shopUrl;
            store.MarketplaceID= marketplaceID;


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

            webClient.GetOrders(store, DateTime.Now);
        }

        [Theory]
        [InlineData("url", "123456", "123456")]
        [InlineData("url", "", "")]
        public void ConfirmShipping_Succeeds(string shopUrl, string authKey, string marketplaceID)
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1);
            store.AuthKey = authKey;
            store.ShopURL = shopUrl;
            store.MarketplaceID = marketplaceID;

            var order = new RakutenOrderEntity
            {
                RakutenPackageID = ""
            };

            var shipment = new ShipmentEntity
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
            RakutenStoreEntity store = new RakutenStoreEntity(1);
            store.AuthKey = authKey;
            store.ShopURL = shopUrl;
            store.MarketplaceID = marketplaceID;

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

            webClient.GetProduct(store, "");
        }

        [Fact]
        public async Task TestConnection_ReturnsTrueWhenNoError()
        {

            RakutenStoreEntity store = new RakutenStoreEntity(1);
            store.AuthKey = "key";
            store.ShopURL = "url";
            store.MarketplaceID = "id";

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

            Assert.True(await webClient.TestConnection(store));
        }

        [Fact]
        public async Task TestConnection_ReturnsFalseWhenError()
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1);
            store.AuthKey = "key";
            store.ShopURL = "url";
            store.MarketplaceID = "id";

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
            RakutenStoreEntity store = new RakutenStoreEntity(1);
            store.AuthKey = "";
            store.ShopURL = "";
            store.MarketplaceID = "";


            IRestResponse<RakutenBaseResponse> response = new RestResponse<RakutenBaseResponse>();
            response.Data = new RakutenBaseResponse
            {
                Errors = new RakutenErrors()
            };

            mock.CreateMock<IRestRequest>()
                .SetupGet(e => e.JsonSerializer)
                .Returns(CreateSerializer());

            var client = mock.FromFactory<IRakutenRestClientFactory>()
                .Mock<IRestClient>(x => x.Create(It.IsAny<string>()));

            client.Setup(g => g.BuildUri(It.IsAny<IRestRequest>()))
            .Returns(new Uri("https://openapi-rms.global.rakuten.com/2.0"));
            client.Setup(x => x.ExecuteTaskAsync<RakutenBaseResponse>(It.IsAny<IRestRequest>()))
            .Returns(Task.FromResult(response));
            client.Setup(x => x.ExecuteTaskAsync<RakutenBaseResponse>(It.IsAny<IRestRequest>()))
            .Returns(Task.FromResult(response));

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>(TypedParameter.From((IRakutenStoreEntity) store));

            await Assert.ThrowsAsync<WebException>(() => webClient.GetOrders(store, DateTime.Now));
        }

        private RestSharpJsonNetSerializer CreateSerializer()
        {
            return new RestSharpJsonNetSerializer(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
            });
        }
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
