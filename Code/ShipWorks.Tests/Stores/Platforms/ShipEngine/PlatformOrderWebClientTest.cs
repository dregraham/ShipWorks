using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.ShipEngine;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.ShipEngine
{
    public class PlatformOrderWebClientTest
    {
        private readonly AutoMock mock;
        private readonly PlatformOrderWebClient platformOrderWebClient;
        private readonly Mock<IStoreManager> storeManager;

        public PlatformOrderWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            storeManager = mock.Mock<IStoreManager>();
            platformOrderWebClient = mock.Create<PlatformOrderWebClient>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("{}")]
        [InlineData("{\"error\":false,\"orders\":{\"continuationToken\":null,\"data\":[],\"errors\":[]}")]
        public async Task GetOrders_EmptyValue(string content)
        {
            var warehouseRequestClient = mock.Mock<IWarehouseRequestClient>();
            
            var restResponse= (IRestResponse) mock.Create<RestResponse>();
            restResponse.Content = content;

            var response = GenericResult.FromSuccess(restResponse);
            
            warehouseRequestClient.Setup(x => x.MakeRequest(It.IsAny<IRestRequest>(), It.IsAny<string>(), ApiLogSource.Amazon))
                .ReturnsAsync(response);

            var result = await platformOrderWebClient.GetOrders(string.Empty, string.Empty);

            Assert.NotNull(result);
            Assert.NotNull(result.Orders.ContinuationToken);
            Assert.NotNull(result.Orders.Data);
            Assert.NotNull(result.Orders.Errors);
        }
    }
}
