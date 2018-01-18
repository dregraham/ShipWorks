using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using System;
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
        public void Download_DelegatesToDownloadManager()
        {
            var downloadManager = mock.Mock<IDownloadManager>();

            testObject.Download("1");

            downloadManager.Verify(m => m.Download("1"));
        }

        [Fact]
        public void Download_DelegatesToMessageHelper_WhenDownloadFails()
        {
            mock.Mock<IDownloadManager>().Setup(m => m.Download("1")).Returns(Result.FromError("Error"));
            var messageHelper = mock.Mock<IMessageHelper>();
            testObject.Download("1");

            messageHelper.Verify(m => m.ShowError("Error"));
        }
        
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
