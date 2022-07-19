using System;
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

        public AmazonPlatformDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            durationEvent = mock.Create<TrackedDurationEvent>(TypedParameter.From("Unit Test"));
            storeManager = mock.Mock<IStoreManager>();
            progressItem = mock.Mock<IProgressReporter>();
            webClient = mock.Mock<IPlatformOrderWebClient>();

            amazonStore = new AmazonStoreEntity();
            ordersDto = new GetOrdersDTO
            {
                Error = false,
                Orders = new PaginatedPlatformServiceResponseOfOrderSourceApiSalesOrder
                {
                    Data = Array.Empty<OrderSourceApiSalesOrder>(),
                    ContinuationToken = string.Empty,
                    Errors = Array.Empty<PlatformError>()
                }
            };

            webClient.Setup(x => x.GetOrders(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(ordersDto);

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
            ordersDto.Error = true;

            // Act
            async Task Act() => await unitUnderTest.ProtectedDownload(durationEvent);

            // Assert
            var result = await Assert.ThrowsAsync<DownloadException>(Act);
            Assert.IsType<Exception>(result.InnerException);
            storeManager.Verify(x => x.SaveStoreAsync(It.IsAny<StoreEntity>()), Times.Once);
        }
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
            IStoreManager storeManager, Func<AmazonStoreEntity, IPlatformOrderWebClient> createWebClient) :
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
