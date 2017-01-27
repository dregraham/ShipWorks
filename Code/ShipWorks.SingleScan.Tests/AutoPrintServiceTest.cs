using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;
using Microsoft.Reactive.Testing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;

namespace ShipWorks.SingleScan.Tests
{
    public class AutoPrintServiceTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger messenger;
        private readonly AutoPrintService testObject;
        private readonly List<ShipmentEntity> shipments;
        private readonly Mock<ILog> mockLog;
        private readonly Mock<ISchedulerProvider> scheduleProvider;

        public AutoPrintServiceTest()
        {
            shipments = new List<ShipmentEntity>();

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            scheduleProvider = mock.WithMockImmediateScheduler();

            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            mock.Mock<ISingleScanShipmentConfirmationService>()
                .Setup(service => service.GetShipments(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(Task.FromResult((IEnumerable<ShipmentEntity>) shipments));

            mockLog = mock.MockRepository.Create<ILog>();
            mock.MockFunc<Type, ILog>(mockLog);

            testObject = mock.Create<AutoPrintService>();

            testObject.InitializeForCurrentSession();
        }

        [Fact]
        public void OrderScanned_SendsProcessMessage_WhenOrderHasOneUnprocessedShipment()
        {
            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            AddShipmentsToReturnByShipmentConfirmationService(1);

            SendScanMessage("A");
            SendFilterCountsUpdatedMessage(1, 5);

            Assert.Equal(1, messenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
        }

        [Fact]
        public void OrderScanned_ShipmentsProcessedMessageReceived_WhenAutoPrinting()
        {
            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            AddShipmentsToReturnByShipmentConfirmationService(1);

            SendScanMessage("A");
            SendFilterCountsUpdatedMessage(1, 5);
            SendShipmentsProcessedMessage();

            mockLog.Verify(l => l.Debug("ShipmentsProcessedMessage received from scan A"));
        }

        [Fact]
        public void OrderScanned_ErrorWrittenToLog_WhenMultipleOrdersMatch()
        {
            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            AddShipmentsToReturnByShipmentConfirmationService(1);

            SendScanMessage("A");
            SendFilterCountsUpdatedMessage(2, 2);
            SendShipmentsProcessedMessage();

            mockLog.Verify(l => l.Error("Error occurred while attempting to auto print.", It.IsAny<ShippingException>()), Times.Once);
        }

        [Fact]
        public void OrderScanned_ShipmentNotProcessed_WhenMultipleOrdersMatch()
        {
            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            AddShipmentsToReturnByShipmentConfirmationService(1);

            SendScanMessage("A");
            SendFilterCountsUpdatedMessage(2, 2);
            SendShipmentsProcessedMessage();

            Assert.False(messenger.SentMessages.OfType<ProcessShipmentsMessage>().Any());
        }

        [Fact]
        public void MultipleOrdersScanned_SecondScanProcesses_WhenShipmentConfirmationServiceReturnsNoShipmentsTheFirstTime()
        {
            TestScheduler windowsScheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(windowsScheduler);

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);

            SendScanMessage("FirstScan");
            SendFilterCountsUpdatedMessage(1, 5);

            windowsScheduler.Start();
                    
            AddShipmentsToReturnByShipmentConfirmationService(1);
            SendScanMessage("SecondScan");
            SendFilterCountsUpdatedMessage(1, 5);
            SendShipmentsProcessedMessage();

            Assert.Equal(1, messenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
            mockLog.Verify(l => l.Debug(It.Is<string>(s => s.EndsWith("SecondScan"))));
        }

        [Fact]
        public void OrderScanned_AllUnprocessedShipmentsProcessed_WhenShipmentConfirmationServiceReturnsMultipleShipments()
        {
            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            AddShipmentsToReturnByShipmentConfirmationService(3);

            SendScanMessage("A");
            SendFilterCountsUpdatedMessage(1, 5);

            Assert.Equal(1, messenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
            var shipmentsMessage = messenger.SentMessages.OfType<ProcessShipmentsMessage>().Single();
            Assert.NotNull(shipmentsMessage);

            Assert.Equal(3, shipmentsMessage.Shipments.Count());
            Assert.Equal(shipments, shipmentsMessage.Shipments);
            Assert.Equal(shipments, shipmentsMessage.ShipmentsInContext);
        }

        [Fact]
        public void OrderScanned_DoesNotSendProcessMessage_WhenAllowAutoPrintIsOff()
        {
            SetAutoPrintSetting(SingleScanSettings.Scan);
            AddShipmentsToReturnByShipmentConfirmationService(1);

            SendScanMessage("A");
            SendFilterCountsUpdatedMessage(1, 5);

            Assert.False(messenger.SentMessages.OfType<ProcessShipmentsMessage>().Any());
        }

        [Fact]
        public void MultipleOrdersScanned_SendsMultipleProcessMessage_WhenOrderHasOneUnprocessedShipment()
        {
            TestScheduler windowsScheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(windowsScheduler);

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            AddShipmentsToReturnByShipmentConfirmationService(1);

            SendScanMessage("A");
            SendFilterCountsUpdatedMessage(1, 5);
            SendShipmentsProcessedMessage();

            windowsScheduler.Start();

            SendScanMessage("A");
            SendFilterCountsUpdatedMessage(1, 5);
            SendShipmentsProcessedMessage();

            Assert.Equal(2, messenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
        }

        [Fact]
        public void MultipleOrdersScanned_ProcessesFirstMessage_WhenSecondScanOccursBeforeShipmentsProcessedMessage()
        {
            TestScheduler windowsScheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(windowsScheduler);

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            AddShipmentsToReturnByShipmentConfirmationService(1);

            SendScanMessage("FirstScan");
            SendFilterCountsUpdatedMessage(1, 5);

            windowsScheduler.Start();

            SendScanMessage("SecondScan");
            SendFilterCountsUpdatedMessage(1, 5);
            SendShipmentsProcessedMessage();

            Assert.Equal(1, messenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
            mockLog.Verify(l => l.Debug(It.Is<string>(s => s.EndsWith("FirstScan"))));
        }

        [Fact]
        public void OrderScanned_AfterBlankBarcodeScanned_SendsProcessMessage_WhenOrderHasOneUnprocessedShipment()
        {
            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            AddShipmentsToReturnByShipmentConfirmationService(1);

            SendScanMessage(string.Empty);

            SendScanMessage("SecondScan");
            SendFilterCountsUpdatedMessage(1, 5);

            Assert.Equal(1, messenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
            mockLog.Verify(l => l.Debug(It.Is<string>(s => s.EndsWith("SecondScan"))));
        }

        private void SendFilterCountsUpdatedMessage(int numberOfOrders, long? orderId)
        {
            mock.Mock<IFilterNodeContentEntity>()
                .SetupGet(node => node.Count)
                .Returns(numberOfOrders);

            messenger.Send(new FilterCountsUpdatedMessage(this, mock.Mock<IFilterNodeContentEntity>().Object, orderId));
        }

        private void SendScanMessage(string scannedText)
        {
            messenger.Send(new ScanMessage(this, scannedText, IntPtr.Zero));
        }

        private void SendShipmentsProcessedMessage()
        {
            messenger.Send(new ShipmentsProcessedMessage());
        }

        private void AddShipmentsToReturnByShipmentConfirmationService(int numberOfShipments)
        {
            for (int newShipmentId = 1; newShipmentId <= numberOfShipments; newShipmentId++)
            {
                shipments.Add(new ShipmentEntity(newShipmentId));
            }
        }

        private void SetAutoPrintSetting(SingleScanSettings autoPrint)
        {
            var userSettings = mock.Mock<IUserSettingsEntity>();

            userSettings.SetupGet(s => s.SingleScanSettings)
                .Returns((int) autoPrint);

            mock.Mock<IUserSession>()
                .SetupGet(s => s.Settings)
                .Returns(userSettings.Object);
        }

        public void Dispose()
        {
            testObject?.Dispose();
            messenger?.Dispose();
            mock?.Dispose();
        }

    }
}