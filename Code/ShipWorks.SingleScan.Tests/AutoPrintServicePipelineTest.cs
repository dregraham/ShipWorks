using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class AutoPrintServicePipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger messenger;
        private readonly TestScheduler windowsScheduler;
        private readonly TestScheduler defaultScheduler;
        private readonly AutoPrintServicePipeline testObject;
        private readonly SingleScanFilterUpdateCompleteMessage singleScanFilterUpdateCompleteMessage;

        public AutoPrintServicePipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            windowsScheduler = new TestScheduler();
            defaultScheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(windowsScheduler);
            scheduleProvider.Setup(s => s.Default).Returns(defaultScheduler);
            testObject = mock.Create<AutoPrintServicePipeline>();

            singleScanFilterUpdateCompleteMessage = new SingleScanFilterUpdateCompleteMessage(this,
                mock.Mock<IFilterNodeContentEntity>().Object, 5);

            SetAllowAutoPrint(true);
        }

        [Fact]
        public void IsListeningForScans_ReturnsTrue_WhenInitializeForCurrentSessionIsCalled()
        {
            testObject.InitializeForCurrentSession();

            Assert.True(testObject.IsListeningForScans);
        }


        [Fact]
        public void IsListeningForScans_ReturnsFalse_WhenScanReceivedAndAutoPrintIsOn()
        {
            testObject.InitializeForCurrentSession();

            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            defaultScheduler.Start();

            Assert.False(testObject.IsListeningForScans);
        }

        [Fact]
        public void IsListeningForScans_ReturnsTrue_WhenScanReceivedAndAutoPrintIsOff()
        {
            SetAllowAutoPrint(false);

            testObject.InitializeForCurrentSession();

            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            defaultScheduler.Start();

            Assert.True(testObject.IsListeningForScans);
        }

        [Fact]
        public void DelegatesToAutoPrintService_WhenAutoPrintIsOn_AndScanMessageReceived_AndSingleScanFilterUpdateCompleteMessageReceived()
        {
            SetAllowAutoPrint(true);

            testObject.InitializeForCurrentSession();

            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            messenger.Send(singleScanFilterUpdateCompleteMessage);

            windowsScheduler.Start();

            mock.Mock<IAutoPrintService>()
                .Verify(a=>a.Print(It.IsAny<AutoPrintServiceDto>()), Times.Once);
        }

        [Fact]
        public void SendsOrderSelectionChangingMessage_AfterDelegatingToAutoPrintService_AndShipmentsProcessedMessageReceived()
        {
            SetAllowAutoPrint(true);

            testObject.InitializeForCurrentSession();

            mock.Mock<IAutoPrintService>()
                .Setup(a => a.Print(It.IsAny<AutoPrintServiceDto>()))
                .ReturnsAsync(GenericResult.FromSuccess("foo"));

            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            messenger.Send(singleScanFilterUpdateCompleteMessage);

            windowsScheduler.Start();

            messenger.Send(new ShipmentsProcessedMessage(null, new ProcessShipmentResult[0]));

            Assert.Equal(1, messenger.SentMessages.OfType<OrderSelectionChangingMessage>().Count());
        }

        [Fact]
        public void IsListeningForScans_ReturnsTrue_AfterDelegatingToAutoPrintService_AndShipmentsProcessedMessageReceived()
        {
            SetAllowAutoPrint(true);

            testObject.InitializeForCurrentSession();

            mock.Mock<IAutoPrintService>()
                .Setup(a => a.Print(It.IsAny<AutoPrintServiceDto>()))
                .ReturnsAsync(GenericResult.FromSuccess("foo"));

            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            messenger.Send(singleScanFilterUpdateCompleteMessage);

            windowsScheduler.Start();

            Assert.False(testObject.IsListeningForScans);
            messenger.Send(new ShipmentsProcessedMessage(null, new ProcessShipmentResult[0]));
            Assert.True(testObject.IsListeningForScans);
        }

        [Fact]
        public void IsListeningForScans_ReturnsTrue_AfterAutoPrintServiceThrows()
        {
            SetAllowAutoPrint(true);

            testObject.InitializeForCurrentSession();

            mock.Mock<IAutoPrintService>()
                .Setup(a => a.Print(It.IsAny<AutoPrintServiceDto>()))
                .Throws<Exception>();

            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            messenger.Send(singleScanFilterUpdateCompleteMessage);

            Assert.False(testObject.IsListeningForScans);
            windowsScheduler.Start();
            Assert.True(testObject.IsListeningForScans);
        }

        private void SetAllowAutoPrint(bool allow)
        {
            mock.Mock<IAutoPrintService>()
                .Setup(s => s.AllowAutoPrint(It.IsAny<ScanMessage>()))
                .Returns(allow);
        }

        public void Dispose()
        {
            testObject.Dispose();
            messenger.Dispose();
            mock.Dispose();
        }

    }
}