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
    public class MainGridControlPipelineTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly Mock<IOnDemandDownloader> downloader;
        private readonly Mock<IMainGridControl> mainGridControl;
        private readonly TestScheduler scheduler;
        private readonly MainGridControlPipeline testObject;

        public MainGridControlPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);

            downloader = mock.Mock<IOnDemandDownloader>();

            mainGridControl = mock.Mock<IMainGridControl>();
            mainGridControl.SetupGet(g => g.Visible).Returns(true);
            mainGridControl.SetupGet(g => g.CanFocus).Returns(true);

            var userSettings = new UserSettingsEntity()
            { SingleScanSettings = (int)SingleScanSettings.Scan };

            var userSession = mock.Mock<IUserSession>();
            userSession.SetupGet(s => s.Settings).Returns(userSettings);

            testObject = mock.Create<MainGridControlPipeline>();
        }

        [Fact]
        public void ScanMessageTextIsSentToDownloader()
        {
            testObject.Register(mainGridControl.Object);
            testMessenger.Send(new ScanMessage(this, "  foo  ", IntPtr.Zero));
            scheduler.Start();

            downloader.Verify(d => d.Download("foo"));
        }
    }
}
