using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Communication
{
    public class OnDemandDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OnDemandDownloader testObject;

        public OnDemandDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<OnDemandDownloader>();
        }

        [Fact]
        public async Task Download_DelegatesToDownloadManager()
        {
            var downloadManager = mock.Mock<IDownloadManager>();
            downloadManager.Setup(m => m.Download("1")).ReturnsAsync(new List<Exception>());
            await testObject.Download("1");

            downloadManager.Verify(m => m.Download("1"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task Download_DoesNotDelegateToDownload_WhenStringIsNullOrWhitespace(string blankOrderNumber)
        {
            var downloadManager = mock.Mock<IDownloadManager>();
            await testObject.Download(blankOrderNumber);

            downloadManager.Verify(m => m.Download(AnyString), Times.Never);
        }

        [Fact]
        public async Task Download_DoesNotDelegateToDownload_WhenStringIsOverFiftyCharacters()
        {
            var downloadManager = mock.Mock<IDownloadManager>();
            await testObject.Download(new string('1', 51));

            
            downloadManager.Verify(m => m.Download(AnyString), Times.Never);
        }

        [Fact]
        public async Task Download_DelegatesToDownload_WhenStringIsFiftyCharacters()
        {
            var downloadManager = mock.Mock<IDownloadManager>();
            downloadManager.Setup(m => m.Download(AnyString)).ReturnsAsync(new List<Exception>());

            await testObject.Download(new string('1', 50));


            downloadManager.Verify(m => m.Download(AnyString), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
