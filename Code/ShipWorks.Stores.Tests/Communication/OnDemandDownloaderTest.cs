using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Utility;
using ShipWorks.Stores.Platforms.Odbc.Download;
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

        [Fact]
        public async Task Download_ShowsWarning_WhenSqlAppResourceLockExceptionThrown()
        {
            var downloadManager = mock.Mock<IDownloadManager>();
            downloadManager.Setup(m => m.Download(AnyString))
                .ThrowsAsync(new SqlAppResourceLockException("blah"));

            await testObject.Download("blah");

            mock.Mock<IMessageHelper>().Verify(m => m.ShowWarning("Someone else just searched for this order. Please search again."), Times.Once);
        }

        [Fact]
        public async Task Download_DoesNotShowError_WhenNoExceptionsFromDownloader()
        {
            var downloadManager = mock.Mock<IDownloadManager>();
            downloadManager.Setup(m => m.Download(AnyString)).ReturnsAsync(new List<Exception>());

            await testObject.Download("blah");

            mock.Mock<IMessageHelper>().Verify(m=>m.ShowPopup(AnyString), Times.Never);
        }

        [Fact]
        public async Task Download_ShowsPopup_WhenGeneralExceptionFromDownloader()
        {
            var downloadManager = mock.Mock<IDownloadManager>();
            downloadManager.Setup(m => m.Download(AnyString)).ReturnsAsync(new[] { new DownloadException() });

            await testObject.Download("blah");

            mock.Mock<IMessageHelper>().Verify(m => m.ShowPopup("There was an error downloading 'blah.' Please see the download log for additional information."), Times.Once);
        }

        [Fact]
        public async Task Download_ShowsPopup_WhenOnDemandDownloadExceptionFromDownloader_AndShowPopupTrue()
        {
            var downloadManager = mock.Mock<IDownloadManager>();
            downloadManager.Setup(m => m.Download(AnyString)).ReturnsAsync(new[] { new OnDemandDownloadException(true, "msg", new DownloadException()) });

            await testObject.Download("blah");

            mock.Mock<IMessageHelper>().Verify(m => m.ShowPopup("There was an error downloading 'blah.' Please see the download log for additional information."), Times.Once);
        }

        [Fact]
        public async Task Download_NoPopup_WhenOnDemandDownloadExceptionFromDownloader_AndShowPopupFalse()
        {
            var downloadManager = mock.Mock<IDownloadManager>();
            downloadManager.Setup(m => m.Download(AnyString)).ReturnsAsync(new[] { new OnDemandDownloadException(false, "msg", new DownloadException()) });

            await testObject.Download("blah");

            mock.Mock<IMessageHelper>().Verify(m => m.ShowPopup(AnyString), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
