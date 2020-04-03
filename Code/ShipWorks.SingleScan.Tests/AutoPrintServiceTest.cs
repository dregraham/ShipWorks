﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Shipping;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class AutoPrintServiceTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger messenger;
        private AutoPrintService testObject;
        private readonly List<ShipmentEntity> shipments;
        private SingleScanFilterUpdateCompleteMessage singleScanFilterUpdateCompleteMessage;

        public AutoPrintServiceTest()
        {
            shipments = new List<ShipmentEntity>();

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            mock.Mock<ISingleScanConfirmationService>()
                .Setup(service => service.ConfirmOrder(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(true);

            mock.Mock<ISingleScanConfirmationService>()
                .Setup(service => service.GetShipments(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(shipments);

            mock.Mock<IFilterNodeContentEntity>()
                .SetupGet(node => node.Count)
                .Returns(1);

            singleScanFilterUpdateCompleteMessage = new SingleScanFilterUpdateCompleteMessage(this,
                mock.Mock<IFilterNodeContentEntity>().Object, 5);
        }

        [Fact]
        public async Task OrderScanned_SendsProcessMessage_WhenOrderHasOneUnprocessedShipment()
        {
            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            SetReturnValueOfApplyWeight(true);

            shipments.Add(new ShipmentEntity(1));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            Assert.Equal(1, messenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
        }

        [Fact]
        public async Task OrderScanned_SendsReprintLabelsMessage_WhenOrderHasOnlyProcessedShipments()
        {
            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);

            shipments.Add(new ShipmentEntity(1) { Processed = true });
            shipments.Add(new ShipmentEntity(2) { Processed = true });

            mock.Mock<ISingleScanConfirmationService>().
                Setup(s => s.GetShipments(It.IsAny<long>(), It.IsAny<string>())).
                ReturnsAsync(shipments);

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            Assert.Equal(1, messenger.SentMessages.OfType<ReprintLabelsMessage>().Count());
        }

        [Fact]
        public async Task Print_ReturnsResultFromError_WhenOrderHasOnlyProcessedShipments()
        {
            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);

            shipments.Add(new ShipmentEntity(1) { Processed = true });
            shipments.Add(new ShipmentEntity(2) { Processed = true });

            mock.Mock<ISingleScanConfirmationService>().
                Setup(s => s.GetShipments(It.IsAny<long>(), It.IsAny<string>())).
                ReturnsAsync(shipments);

            var result = await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            Assert.True(result.Failure);
            Assert.Equal(new AutoPrintResult("A", 5), result.Value);
        }

        [Fact]
        public async Task OrderScanned_OrderDoesNotPrint_WhenOrderServiceReturnsFalse()
        {
            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            SetReturnValueOfApplyWeight(true);
            shipments.Add(new ShipmentEntity(1));

            mock.Mock<ISingleScanConfirmationService>()
                .Setup(service => service.ConfirmOrder(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(false);

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            Assert.False(messenger.SentMessages.OfType<ProcessShipmentsMessage>().Any());
        }

        [Fact]
        public async Task OrderScanned_DelegatesScanInformationToConfirmationService()
        {
            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            shipments.Add(new ShipmentEntity(44));

            mock.Mock<IFilterNodeContentEntity>()
                .SetupGet(node => node.Count)
                .Returns(72);

            singleScanFilterUpdateCompleteMessage = new SingleScanFilterUpdateCompleteMessage(this,
                mock.Mock<IFilterNodeContentEntity>().Object, 101);

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            mock.Mock<ISingleScanConfirmationService>()
                .Verify(service => service.ConfirmOrder(101L, 72, "A"));
        }

        [Fact]
        public async Task OrderScanned_ShipmentNotProcessed_WhenMultipleOrdersMatchAndUserCancels()
        {
            mock.Mock<IFilterNodeContentEntity>()
                .SetupGet(node => node.Count)
                .Returns(2);

            mock.Mock<ISingleScanConfirmationService>().
                Setup(s => s.ConfirmOrder(It.IsAny<long>(), 2, "A")).
                Returns(false);

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            shipments.Add(new ShipmentEntity(1));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            Assert.False(messenger.SentMessages.OfType<ProcessShipmentsMessage>().Any());
        }

        [Fact]
        public async Task OrderScanned_ShipmentProcessed_WhenMultipleOrdersMatchAndUserConfirms()
        {
            mock.Mock<IFilterNodeContentEntity>()
                .SetupGet(node => node.Count)
                .Returns(2);

            mock.Mock<ISingleScanConfirmationService>().
                Setup(s => s.ConfirmOrder(It.IsAny<long>(), 2, "A")).
                Returns(true);

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            SetReturnValueOfApplyWeight(true);
            shipments.Add(new ShipmentEntity(1));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            Assert.True(messenger.SentMessages.OfType<ProcessShipmentsMessage>().Any());
        }

        [Fact]
        public async Task MultipleOrdersScanned_SecondScanProcesses_WhenShipmentConfirmationServiceReturnsNoShipmentsTheFirstTime()
        {
            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            SetReturnValueOfApplyWeight(true);

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "FirstScan", IntPtr.Zero))));

            shipments.Add(new ShipmentEntity(1));
            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "SecondScan", IntPtr.Zero))));

            Assert.Equal(1, messenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
        }

        private void SetReturnValueOfApplyWeight(bool applyWeightResult)
        {
            mock.Mock<IAutoWeighService>()
                .Setup(s => s.ApplyWeight(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<ITrackedDurationEvent>()))
                .Returns(applyWeightResult);
        }

        [Fact]
        public async Task OrderScanned_ShipmentNotProcessed_WhenApplyWeightReturnsFalse()
        {
            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            SetReturnValueOfApplyWeight(false);
            shipments.AddRange(Enumerable.Range(1, 3).Select(o => new ShipmentEntity(o)));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            Assert.True(messenger.SentMessages.OfType<ProcessShipmentsMessage>().None());
        }

        [Fact]
        public async Task OrderScanned_AllUnprocessedShipmentsProcessed_WhenShipmentConfirmationServiceReturnsMultipleShipments()
        {
            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            SetReturnValueOfApplyWeight(true);
            shipments.AddRange(Enumerable.Range(1, 3).Select(o => new ShipmentEntity(o)));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            Assert.Equal(1, messenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
            var shipmentsMessage = messenger.SentMessages.OfType<ProcessShipmentsMessage>().Single();
            Assert.NotNull(shipmentsMessage);

            Assert.Equal(3, shipmentsMessage.Shipments.Count());
            Assert.Equal(shipments, shipmentsMessage.Shipments);
            Assert.Equal(shipments, shipmentsMessage.ShipmentsInContext);
        }

        [Fact]
        public async Task MultipleOrdersScanned_SendsMultipleProcessMessage_WhenOrderHasOneUnprocessedShipment()
        {
            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            SetReturnValueOfApplyWeight(true);
            shipments.Add(new ShipmentEntity(1));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));
            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            Assert.Equal(2, messenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
        }

        [Fact]
        public async Task AddTelemetryData_SetsShippingProvidersToNA_WhenNoShipmentsProcessed()
        {
            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            trackedDurationEvent.Verify(
                            e => e.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.ShippingProviders", "N/A"), Times.Once);
        }

        [Fact]
        public async Task AddTelemetryData_SetsShippingProvidersToCarrier_WhenShipmentProcessed()
        {
            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            shipments.Add(new ShipmentEntity(1) { ShipmentTypeCode = ShipmentTypeCode.Usps });

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            trackedDurationEvent.Verify(
                            e => e.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.ShippingProviders", "USPS"), Times.Once);
        }

        [Fact]
        public async Task AddTelemetryData_SetsShippingProvidersToListOfCarriers_WhenMutlipleShipmentsProcessedWithMutipleCarriers()
        {
            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            shipments.Add(new ShipmentEntity(1) { ShipmentTypeCode = ShipmentTypeCode.Usps });
            shipments.Add(new ShipmentEntity(2) { ShipmentTypeCode = ShipmentTypeCode.FedEx });

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            trackedDurationEvent.Verify(
                            e => e.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.ShippingProviders", "USPS, FedEx"), Times.Once);
        }

        [Fact]
        public async Task AddTelemetryData_SetsRequiredConfirmationToNo_WhenSingleOrderFoundWithSingleShipment()
        {
            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            shipments.Add(new ShipmentEntity(1));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            trackedDurationEvent.Verify(
                            e => e.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.RequiredConfirmation", "No"), Times.Once);
        }

        [Fact]
        public async Task AddTelemetryData_SetsRequiredConfirmationToYes_WhenSingleOrderFoundWithMultipleShipments()
        {
            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            shipments.Add(new ShipmentEntity(1));
            shipments.Add(new ShipmentEntity(2));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            trackedDurationEvent.Verify(
                            e => e.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.RequiredConfirmation", "Yes"), Times.Once);
        }

        [Fact]
        public async Task AddTelemetryData_SetsRequiredConfirmationToYes_WhenMultipleOrdersFound()
        {
            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            mock.Mock<IFilterNodeContentEntity>()
                .SetupGet(node => node.Count)
                .Returns(2);

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            shipments.Add(new ShipmentEntity(1));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            trackedDurationEvent.Verify(
                            e => e.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.RequiredConfirmation", "Yes"), Times.Once);
        }

        [Fact]
        public async Task AddTelemetryData_SetsPrintAbortedToNo_WhenUserConfirmsPrint()
        {
            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            mock.Mock<ISingleScanConfirmationService>().
                Setup(s => s.ConfirmOrder(It.IsAny<long>(), 1, "A")).
                Returns(true);

            mock.Mock<ISingleScanConfirmationService>()
                .Setup(s => s.GetShipments(1, "A"))
                .ReturnsAsync(new List<ShipmentEntity>() { new ShipmentEntity(1) });

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            shipments.Add(new ShipmentEntity(1));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            trackedDurationEvent.Verify(
                            e => e.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.PrintAborted", "No"), Times.Once);
        }

        [Fact]
        public async Task AddTelemetryData_SetsPrintAbortedToYes_WhenUserCancelsPrintThroughOrderConfirmationService()
        {
            mock.Mock<IFilterNodeContentEntity>()
                .SetupGet(node => node.Count)
                .Returns(2);

            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            mock.Mock<ISingleScanConfirmationService>().
                Setup(s => s.ConfirmOrder(It.IsAny<long>(), 2, "A")).
                Returns(false);

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            shipments.Add(new ShipmentEntity(1));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            trackedDurationEvent.Verify(
                            e => e.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.PrintAborted", "Yes"), Times.Once);
        }

        [Fact]
        public async Task AddTelemetryData_SetsPrintAbortedToYes_WhenUserCancelsPrintThroughShipmentConfirmationService()
        {
            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            mock.Mock<ISingleScanConfirmationService>().
                Setup(s => s.ConfirmOrder(It.IsAny<long>(), 1, "A")).
                Returns(true);

            mock.Mock<ISingleScanConfirmationService>()
                .Setup(s => s.GetShipments(1, "A"))
                .ReturnsAsync(new List<ShipmentEntity>());

            testObject = mock.Create<AutoPrintService>();

            SetAutoPrintSetting(SingleScanSettings.AutoPrint);
            shipments.Add(new ShipmentEntity(1));

            await testObject.Print(new AutoPrintServiceDto(singleScanFilterUpdateCompleteMessage, new SingleScanMessage(this, new ScanMessage(this, "A", IntPtr.Zero))));

            trackedDurationEvent.Verify(
                            e => e.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.PrintAborted", "No"), Times.Once);
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
            messenger?.Dispose();
            mock?.Dispose();
        }
    }
}