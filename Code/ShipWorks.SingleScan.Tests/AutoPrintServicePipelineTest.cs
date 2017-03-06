using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
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
                .Verify(a => a.Print(It.IsAny<AutoPrintServiceDto>()), Times.Once);
        }

        [Fact]
        public void SavesShipmentsThatFailedToProcess()
        {
            Mock<ISqlAdapter> sqlAdapter = MockSqlAdapter();

            SetAllowAutoPrint(true);

            testObject.InitializeForCurrentSession();

            SetupAutoPrintService("foo", 42, string.Empty, true);

            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            messenger.Send(singleScanFilterUpdateCompleteMessage);
            windowsScheduler.Start();

            ProcessShipmentResult unprocessedShipment = new ProcessShipmentResult(new ShipmentEntity(), new Exception());
            ProcessShipmentResult processedShipment = new ProcessShipmentResult(new ShipmentEntity());

            messenger.Send(new ShipmentsProcessedMessage(null, new[] { unprocessedShipment, processedShipment, unprocessedShipment }));
            windowsScheduler.Start();

            sqlAdapter.Verify(a => a.SaveAndRefetch(unprocessedShipment.Shipment), Times.Exactly(2));
            sqlAdapter.Verify(a => a.SaveAndRefetch(It.IsAny<ShipmentEntity>()), Times.Exactly(2));
            sqlAdapter.Verify(a => a.Commit(), Times.Once);
            mock.Mock<ISqlAdapterFactory>().Verify(f => f.CreateTransacted(), Times.Once);
        }

        private void SetupAutoPrintService(string scannedBarcode, int orderID, string errorMessage, bool success)
        {
            var result = success ? 
                GenericResult.FromSuccess(new AutoPrintResult(scannedBarcode, orderID)) :
                GenericResult.FromError(errorMessage, new AutoPrintResult(scannedBarcode, orderID));

            mock.Mock<IAutoPrintService>()
                .Setup(a => a.Print(It.IsAny<AutoPrintServiceDto>()))
                .ReturnsAsync(result);
        }

        private Mock<ISqlAdapter> MockSqlAdapter()
        {
            var sqlAdapter = mock.CreateMock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.CreateTransacted()).Returns(sqlAdapter.Object);
            return sqlAdapter;
        }

        [Fact]
        public void SendsOrderSelectionChangingMessage_AfterDelegatingToAutoPrintService_AndShipmentsProcessedMessageReceived()
        {
            SetAllowAutoPrint(true);
            SetupAutoPrintService("foo", 42, string.Empty, true);

            testObject.InitializeForCurrentSession();

            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            messenger.Send(singleScanFilterUpdateCompleteMessage);

            windowsScheduler.Start();

            messenger.Send(new ShipmentsProcessedMessage(null, new ProcessShipmentResult[1] { new ProcessShipmentResult(new ShipmentEntity()) }));
            windowsScheduler.Start();

            Assert.Equal(1, messenger.SentMessages.OfType<OrderSelectionChangingMessage>().Count());
        }

        [Fact]
        public void IsListeningForScans_ReturnsTrue_AfterDelegatingToAutoPrintService_AndShipmentsProcessedMessageReceived()
        {
            SetAllowAutoPrint(true);
            SetupAutoPrintService("foo", 42, string.Empty, true);

            testObject.InitializeForCurrentSession();
            
            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            messenger.Send(singleScanFilterUpdateCompleteMessage);

            windowsScheduler.Start();

            Assert.False(testObject.IsListeningForScans);
            messenger.Send(new ShipmentsProcessedMessage(null, new ProcessShipmentResult[0]));
            windowsScheduler.Start();

            Assert.True(testObject.IsListeningForScans);
        }

        [Fact]
        public void IsListeningForScans_ReturnsTrue_AfterAutoPrintServiceReturnsResultWithFalse()
        {
            SetAllowAutoPrint(true);
            SetupAutoPrintService("foo", 42, "oops", false);

            testObject.InitializeForCurrentSession();

            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            messenger.Send(singleScanFilterUpdateCompleteMessage);

            Assert.False(testObject.IsListeningForScans);
            windowsScheduler.Start();
            Assert.True(testObject.IsListeningForScans);
        }

        [Fact]
        public void ShipmentsNotSaved_WhenAutoPrintServiceReturnsResultWithFalse()
        {
            SetAllowAutoPrint(true);
            SetupAutoPrintService("foo", 42, "oops", false);

            testObject.InitializeForCurrentSession();

            messenger.Send(new ScanMessage(this, "foo", IntPtr.Zero));
            messenger.Send(singleScanFilterUpdateCompleteMessage);
            windowsScheduler.Start();

            mock.Mock<ISqlAdapterFactory>().Verify(f => f.CreateTransacted(), Times.Never);
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