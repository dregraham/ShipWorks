using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using System;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class MainGridControlPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly Mock<IOnDemandDownloader> downloader;
        private readonly Mock<IMainGridControl> mainGridControl;
        private readonly TestScheduler scheduler;
        private readonly MainGridControlPipeline testObject;
        private readonly UserSettingsEntity userSettings;

        public MainGridControlPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);

            downloader = mock.FromFactory<IOnDemandDownloaderFactory>()
                .Mock(f => f.CreateSingleScanOnDemandDownloader());

            mainGridControl = mock.Mock<IMainGridControl>();
            mainGridControl.SetupGet(g => g.Visible).Returns(true);
            mainGridControl.SetupGet(g => g.CanFocus).Returns(true);

            userSettings = new UserSettingsEntity()
            { SingleScanSettings = (int) SingleScanSettings.Scan };

            var userSession = mock.Mock<IUserSession>();
            userSession.SetupGet(s => s.Settings).Returns(userSettings);

            testObject = mock.Create<MainGridControlPipeline>();
        }

        [Fact]
        public void DownloadOnDemand_DelegatesToOnDemandDownloaderWithSearchString()
        {
            testObject.Register(mainGridControl.Object);
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "  foo  ", IntPtr.Zero)));
            scheduler.Start();

            downloader.Verify(d => d.Download("foo"));
        }

        [Fact]
        public void DownloadOnDemand_DoesNotDelegatesToOnDemandDownloader_WhenSingleScanIsDisabled()
        {
            userSettings.SingleScanSettings = (int)SingleScanSettings.Disabled;

            testObject.Register(mainGridControl.Object);
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "  foo  ", IntPtr.Zero)));
            scheduler.Start();

            downloader.Verify(d => d.Download(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void DownloadOnDemand_DoesNotDelegatesToOnDemandDownloaderWithSearchString_WhenStringIsEmpty()
        {
            testObject.Register(mainGridControl.Object);
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "    ", IntPtr.Zero)));
            scheduler.Start();

            downloader.Verify(d => d.Download(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void PerformBarcodeSearchAsync_DelegatesToGridControlWithScannedBarcode()
        {
            testObject.Register(mainGridControl.Object);
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "foo", IntPtr.Zero)));
            scheduler.Start();

            mainGridControl.Verify(g => g.BeginInvoke((Action<string>)mainGridControl.Object.PerformBarcodeSearch, "foo"));
        }

        public void Dispose()
        {
            mock.Dispose();
            testMessenger.Dispose();
        }
    }
}
