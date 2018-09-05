using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using System;
using ShipWorks.Settings;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

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

        private SingleScanSettings singleScanSettings = SingleScanSettings.Scan;
        private UIMode uiMode = UIMode.Batch;

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

            mock.Mock<ICurrentUserSettings>()
                .Setup(s => s.GetSingleScanSettings())
                .Returns(() => singleScanSettings);

            mock.Mock<ICurrentUserSettings>()
                .Setup(s => s.GetUIMode())
                .Returns(() => uiMode);

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
        public void DownloadOnDemand_DoesNotDelgateToDownloader_WhenUIModeIsNotBatch()
        {
            uiMode = UIMode.OrderLookup;

            testObject.Register(mainGridControl.Object);
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "  foo  ", IntPtr.Zero)));
            scheduler.Start();

            downloader.Verify(d => d.Download(AnyString), Times.Never);
        }

        [Fact]
        public void DownloadOnDemand_DoesNotDelegatesToOnDemandDownloader_WhenSingleScanIsDisabled()
        {
            singleScanSettings = SingleScanSettings.Disabled;

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

        [Fact]
        public void PerformBarcodeSearchAsync_DoesNotDelegateToGridControlWithScannedBarcode_WhenNotBatchMode()
        {
            uiMode = UIMode.OrderLookup;

            testObject.Register(mainGridControl.Object);
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "foo", IntPtr.Zero)));
            scheduler.Start();

            mainGridControl.Verify(g => g.BeginInvoke((Action<string>) mainGridControl.Object.PerformBarcodeSearch, AnyString), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
            testMessenger.Dispose();
        }
    }
}
