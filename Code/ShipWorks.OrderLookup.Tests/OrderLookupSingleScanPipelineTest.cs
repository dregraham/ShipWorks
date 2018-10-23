using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Settings;
using ShipWorks.SingleScan;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupSingleScanPipelineTest
    {
        private readonly AutoMock mock;
        readonly TestMessenger testMessenger;
        private readonly Mock<IOrderLookupOrderRepository> orderRepository;
        private readonly Mock<IMainForm> mainForm;
        private readonly OrderLookupSingleScanPipeline testObject;
        private readonly TestScheduler scheduler;
        private readonly Mock<IOnDemandDownloader> downloader;
        private readonly Mock<IOrderLookupAutoPrintService> autoPrintService;
        private readonly OrderEntity order;

        public OrderLookupSingleScanPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);

            orderRepository = mock.Mock<IOrderLookupOrderRepository>();
            orderRepository.Setup(o => o.GetOrderIDs(AnyString)).Returns(new List<long> { 123 });

            mock.Mock<IOrderLookupConfirmationService>().Setup(o => o.ConfirmOrder(It.IsAny<List<long>>())).ReturnsAsync(123);

            Mock<IOnDemandDownloaderFactory> downloadFactory = mock.Mock<IOnDemandDownloaderFactory>();
            downloader = mock.Mock<IOnDemandDownloader>();
            downloadFactory.Setup(d => d.CreateOnDemandDownloader()).Returns(downloader);

            mainForm = mock.Mock<IMainForm>();

            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);

            order = new OrderEntity(123);

            var processShipmentResult = new ProcessShipmentResult(new ShipmentEntity() { Order = order });
            var printResult = new AutoPrintCompletionResult(123, new List<ProcessShipmentResult> { processShipmentResult });
            autoPrintService = mock.Mock<IOrderLookupAutoPrintService>();
            autoPrintService
                .Setup(p => p.AutoPrintShipment(AnyLong, It.IsAny<SingleScanMessage>()))
                .ReturnsAsync(printResult);

            testObject = mock.Create<OrderLookupSingleScanPipeline>();
            testObject.InitializeForCurrentSession();
        }

        [Fact]
        public void InitializeForCurrentSession_DelegatesToOnDemandDownlader_WhenUIModeIsOrderLookupAndMessageIsSingleScan()
        {
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            downloader.Verify(d => d.Download("Foo"));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegatesToOnDemandDownlader_WhenUIModeIsBatchMessageIsSingleScan()
        {
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.Batch);
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            downloader.Verify(d => d.Download(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentSession_DelegatesToOnDemandDownlader_WhenUIModeIsOrderLookupAndMessageIsOrderLookupSearch()
        {
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);
            testMessenger.Send(new OrderLookupSearchMessage(this, "Foo"));

            downloader.Verify(d => d.Download("Foo"));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegatesToOnDemandDownlader_WhenUIModeIsBatchMessageIsOrderLookupSearch()
        {
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.Batch);
            testMessenger.Send(new OrderLookupSearchMessage(this, "Foo"));

            downloader.Verify(d => d.Download(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentSession_DelegatesToAutoPrintService_WhenUIModeIsOrderLookupAndMessageIsSingleScan()
        {
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);
            orderRepository.Setup(o => o.GetOrderIDs("Foo")).Returns(new List<long> { 123 });
            SingleScanMessage singleScanMessage = new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero));
            testMessenger.Send(singleScanMessage);

            autoPrintService.Verify(a => a.AutoPrintShipment(123, singleScanMessage));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegateToAutoPrintService_WhenUIModeIsOrderLookupAndMessageIsOrderLookupSearch()
        {
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);
            orderRepository.Setup(o => o.GetOrderIDs("Foo")).Returns(new List<long> { 123 });
            OrderLookupSearchMessage message = new OrderLookupSearchMessage(this, "Foo");
            testMessenger.Send(message);

            autoPrintService.Verify(a => a.AutoPrintShipment(It.IsAny<long>(), It.IsAny<SingleScanMessage>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentSession_DelegatesToOrderLookupOrderRepository_WhenUIModeIsOrderLookupAndMessageIsSingleScan()
        {
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);
            SingleScanMessage singleScanMessage = new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero));
            testMessenger.Send(singleScanMessage);

            orderRepository.Verify(o => o.GetOrderIDs("Foo"));
        }

        [Fact]
        public void InitializeForCurrentSession_LoadsOrderOnShipmentModel()
        {
            orderRepository.Setup(x => x.GetOrderIDs("Foo")).Returns(new List<long> { 1 });
            mock.Mock<IOrderLookupConfirmationService>().Setup(o => o.ConfirmOrder(It.IsAny<List<long>>())).ReturnsAsync(1);
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);

            SingleScanMessage singleScanMessage = new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero));
            testMessenger.Send(singleScanMessage);

            mock.Mock<IOrderLookupShipmentModel>().Verify(s => s.LoadOrder(order));
        }
    }
}
