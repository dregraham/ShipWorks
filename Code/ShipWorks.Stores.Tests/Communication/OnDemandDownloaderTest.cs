using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Stores.Tests.Communication
{
    public class OnDemandDownloaderTest : IDisposable
    {
        private AutoMock mock;
        private OnDemandDownloader testObject;

        public OnDemandDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<OnDemandDownloader>();
        }

        [Fact]
        public async Task Download_DelegatesToDownloadManager()
        {
            var downloadManager = mock.Mock<IDownloadManager>();
            downloadManager.Setup(m => m.Download("1")).ReturnsAsync(Result.FromSuccess());
            await testObject.Download("1");

            downloadManager.Verify(m => m.Download("1"));
        }

        [Fact]
        public async Task Download_DelegatesToMessageHelper_WhenDownloadFails()
        {
            mock.Mock<IDownloadManager>().Setup(m => m.Download("1")).ReturnsAsync(Result.FromError("Error"));
            var messageHelper = mock.Mock<IMessageHelper>();
            await testObject.Download("1");

            messageHelper.Verify(m => m.ShowError("Error"));
        }
        
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
