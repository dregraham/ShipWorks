using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;
using ShipWorks.Filters.Search;
using ShipWorks.Stores.Communication;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Communication
{
    public class SingleScanOnDemandDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IOnDemandDownloader> downloader;
        private readonly SingleScanOnDemandDownloader testObject;

        public SingleScanOnDemandDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            downloader = mock.Mock<IOnDemandDownloader>();

            var downloaderIndex = mock.CreateMock<IIndex<OnDemandDownloaderType, IOnDemandDownloader>>();
            mock.Provide(downloaderIndex.Object);
            downloaderIndex.Setup(i => i[OnDemandDownloaderType.OnDemandDownloader]).Returns(downloader);

            testObject = mock.Create<SingleScanOnDemandDownloader>();
        }

        [Fact]
        public void Download_DelegatesToOnDemandDownloader_WhenOrderShortcutAppliesToReturnsFalse()
        {
            mock.Mock<ISingleScanOrderShortcut>().Setup(s => s.AppliesTo(AnyString)).Returns(false);

            testObject.Download("blah");

            downloader.Verify(d=>d.Download("blah"), Times.Once);
        }

        [Fact]
        public void Download_DoesNotDelegateToOnDemandDownloader_WhenOrderShortcutAppliesToReturnsTrue()
        {
            mock.Mock<ISingleScanOrderShortcut>().Setup(s => s.AppliesTo(AnyString)).Returns(true);

            testObject.Download("blah");

            downloader.Verify(d => d.Download(AnyString), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}