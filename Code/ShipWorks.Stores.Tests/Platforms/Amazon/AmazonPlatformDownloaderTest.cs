using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.ShipEngine;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Amazon
{
    public class AmazonPlatformDownloaderTest
    {
        private readonly AutoMock mock;
        private readonly GetOrdersDTO ordersDto;
        private readonly TrackedDurationEvent durationEvent;
        private readonly Mock<IStoreManager> storeManager;
        private readonly Mock<IProgressReporter> progressItem;
        private readonly Mock<IPlatformOrderWebClient> webClient;
        private readonly AmazonStoreEntity amazonStore;
        private readonly AmazonPlatformDownloaderChild unitUnderTest;
        private readonly string orderSourceId = "OrderSourceId";

        public AmazonPlatformDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            durationEvent = mock.Create<TrackedDurationEvent>(TypedParameter.From("Unit Test"));
            storeManager = mock.Mock<IStoreManager>();
            progressItem = mock.Mock<IProgressReporter>();
            webClient = mock.Mock<IPlatformOrderWebClient>();

            amazonStore = new AmazonStoreEntity() { OrderSourceID = orderSourceId };
            ordersDto = CreateOrderDTOResponse(string.Empty, 1);

            webClient.SetupSequence(x => x.GetOrders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ordersDto)
                .ReturnsAsync(CreateOrderDTOResponse("CT", 0));

            Func<AmazonStoreEntity, IPlatformOrderWebClient> createWebClient = storeEntity => webClient.Object;

            unitUnderTest = mock.Create<AmazonPlatformDownloaderChild>(TypedParameter.From((StoreEntity) amazonStore), TypedParameter.From(createWebClient));
        }

        [Fact]
        public async Task AmazonPlatformDownloader_Download()
        {
            // Arrange
            ordersDto.Orders.ContinuationToken = "Continue";

            // Act
            await unitUnderTest.ProtectedDownload(durationEvent);

            // Assert
            storeManager.Verify(x => x.SaveStoreAsync(It.Is<AmazonStoreEntity>(store => store.ContinuationToken == ordersDto.Orders.ContinuationToken)), Times.Once);
        }

        [Fact]
        public async Task AmazonPlatformDownloader_Download_OrderError()
        {
            // Arrange
            webClient.SetupSequence(x => x.GetOrders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateOrderDTOResponse("CT", 1, true))
                .ReturnsAsync(CreateOrderDTOResponse("CT", 0, true));

            // Act
            async Task Act() => await unitUnderTest.ProtectedDownload(durationEvent);

            // Assert
            var result = await Assert.ThrowsAsync<DownloadException>(Act);
            Assert.IsType<Exception>(result.InnerException);
            storeManager.Verify(x => x.SaveStoreAsync(It.IsAny<StoreEntity>()), Times.Once);
        }

        [Fact]
        public async Task AmazonPlatformDownloader_Download_GetsNextPageOfOrders()
        {
            // Arrange 
            string firstToken = "FirstToken";
            string secondToken = "SecondToken";
            string thirdToken = "ThirdToken";

            webClient.Setup(x => x.GetOrders(orderSourceId, null, CancellationToken.None))
               .ReturnsAsync(CreateOrderDTOResponse(firstToken,1));
            webClient.Setup(x => x.GetOrders(orderSourceId, firstToken, CancellationToken.None))
                .ReturnsAsync(CreateOrderDTOResponse(secondToken, 1));
            webClient.Setup(x => x.GetOrders(orderSourceId, secondToken, CancellationToken.None))
                .ReturnsAsync(CreateOrderDTOResponse(thirdToken, 1));
            webClient.Setup(x => x.GetOrders(orderSourceId, thirdToken, CancellationToken.None))
                            .ReturnsAsync(CreateOrderDTOResponse(thirdToken, 0));
            // Act
            await unitUnderTest.ProtectedDownload(durationEvent);

            // Assert
            webClient.Verify(x => x.GetOrders(orderSourceId, null, CancellationToken.None), Times.Once);
            webClient.Verify(x => x.GetOrders(orderSourceId, firstToken, CancellationToken.None), Times.Once);
            webClient.Verify(x => x.GetOrders(orderSourceId, secondToken, CancellationToken.None), Times.Once);
            webClient.Verify(x => x.GetOrders(orderSourceId, thirdToken, CancellationToken.None), Times.Once);
            webClient.Verify(x => x.GetOrders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
        }

        private GetOrdersDTO CreateOrderDTOResponse(string continuationToken, int orderCount, bool error = false) => 
            new GetOrdersDTO
            {
                Error = error,
                Orders = new PaginatedPlatformServiceResponseOfOrderSourceApiSalesOrder
                {
                    Data = Enumerable.Range(0,orderCount).Select(_=>CreateOrder()).ToArray(),
                    ContinuationToken = continuationToken,
                    Errors = Array.Empty<PlatformError>()
                }
            };

        private OrderSourceApiSalesOrder CreateOrder() =>
            new OrderSourceApiSalesOrder()
            {
                RequestedFulfillments = Array.Empty<OrderSourceRequestedFulfillment>(),
                Payment = new OrderSourcePayment { CouponCodes = Array.Empty<string>() }
            };
        
    }

    /// <summary>
    /// This is a helper class to allow testing specifically the protected Download functionality of the store
    /// </summary>
    internal class AmazonPlatformDownloaderChild : AmazonPlatformDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonPlatformDownloaderChild(IProgressReporter progress, StoreEntity store, IStoreTypeManager storeTypeManager,
            IStoreManager storeManager, Func<StoreEntity, IPlatformOrderWebClient> createWebClient) :
            base(store, storeTypeManager, storeManager, createWebClient)
        {
            Progress = progress;
        }

        /// <summary>
        /// Exposes the Download function of the AmazonPlatformDownloader
        /// </summary>
        internal async Task ProtectedDownload(TrackedDurationEvent trackedDurationEvent)
        {
            await base.Download(trackedDurationEvent).ConfigureAwait(false);
        }
    }
}
