using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class MainGridControlPipelineTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger messenger;
        private readonly TestScheduler windowsScheduler;
        private readonly TestScheduler defaultScheduler;
        private readonly Mock<IOnDemandDownloader> downloader;
        private readonly Mock<IMainGridControl> mainGridControl;
        private MainGridControlPipeline testObject;

        public MainGridControlPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            windowsScheduler = new TestScheduler();
            defaultScheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(windowsScheduler);
            scheduleProvider.Setup(s => s.Default).Returns(defaultScheduler);

            downloader = mock.Mock<IOnDemandDownloader>();

            mainGridControl = mock.Mock<IMainGridControl>();
        }
    }
}
