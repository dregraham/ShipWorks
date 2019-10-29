using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;
using ShipWorks.OrderLookup.ScanPack;
using ShipWorks.OrderLookup.ScanToShip;
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
        private readonly Mock<ILicenseService> licenseService;
        private readonly Mock<IOrderLookupOrderRepository> orderRepository;
        private readonly Mock<IMainForm> mainForm;
        private readonly Mock<IOrderLookupOrderIDRetriever> orderIdRetriever;
        private readonly OrderLookupSingleScanPipeline testObject;
        private readonly TestScheduler scheduler;
        private readonly Mock<IOnDemandDownloader> downloader;
        private readonly Mock<IOrderLookupAutoPrintService> autoPrintService;
        private readonly OrderEntity order;
        private readonly Mock<IScanPackViewModel> scanPackViewModel;
        private bool isPackTabActive = true;
               
        public OrderLookupSingleScanPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);

            licenseService = mock.Mock<ILicenseService>();

            order = new OrderEntity(123);

            orderRepository = mock.Mock<IOrderLookupOrderRepository>();
            orderRepository.Setup(o => o.GetOrderIDs(AnyString)).Returns(new List<long> { 123 });
            orderRepository.Setup(o => o.GetOrder(123, true)).ReturnsAsync(order);

            mock.Mock<IOrderLookupConfirmationService>().Setup(o => o.ConfirmOrder(AnyString, It.IsAny<List<long>>())).ReturnsAsync(123);

            Mock<IOnDemandDownloaderFactory> downloadFactory = mock.Mock<IOnDemandDownloaderFactory>();
            downloader = mock.Mock<IOnDemandDownloader>();
            downloadFactory.Setup(d => d.CreateOnDemandDownloader()).Returns(downloader);

            mainForm = mock.Mock<IMainForm>();

            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);

            var processShipmentResult = new ProcessShipmentResult(new ShipmentEntity() { Order = order });
            var printResult = new AutoPrintCompletionResult(123, new List<ProcessShipmentResult> { processShipmentResult });
            autoPrintService = mock.Mock<IOrderLookupAutoPrintService>();
            autoPrintService
                .Setup(p => p.AutoPrintShipment(AnyLong, It.IsAny<string>()))
                .ReturnsAsync(printResult);


            scanPackViewModel = mock.Mock<IScanPackViewModel>();
            var scanToShipViewModel = mock.Mock<IScanToShipViewModel>();
            
            scanToShipViewModel.SetupGet(m => m.IsPackTabActive).Returns(() => isPackTabActive);
            scanToShipViewModel.SetupGet(m => m.ScanPackViewModel).Returns(scanPackViewModel.Object);
            scanToShipViewModel.SetupGet(m => m.SearchViewModel).Returns(mock.Mock<IOrderLookupSearchViewModel>().Object);

            var telemetricResult = new TelemetricResult<long?>("");
            telemetricResult.SetValue(123);

            orderIdRetriever = mock.Mock<IOrderLookupOrderIDRetriever>();
            orderIdRetriever.Setup(o => o.GetOrderID("Foo", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => telemetricResult);

            testObject = mock.Create<OrderLookupSingleScanPipeline>();
        }

        [Fact]
        public void InitializeForCurrentSession_DelegatesToOnDemandDownlader_WhenUIModeIsOrderLookupAndMessageIsSingleScan()
        {
            testObject.InitializeForCurrentScope();

            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            RetryAssertion(() => downloader.Verify(d => d.Download("Foo")));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegatesToOnDemandDownlader_WhenUIModeIsBatchMessageIsSingleScan()
        {
            testObject.InitializeForCurrentScope();

            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.Batch);
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            RetryAssertion(() => downloader.Verify(d => d.Download(It.IsAny<string>()), Times.Never));
        }

        [Fact]
        public async Task OnOrderLookupSearchMessage_DelegatesToOnDemandDownlader_WhenUIModeIsOrderLookupAndMessageIsOrderLookupSearch()
        {
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);

            await testObject.OnOrderLookupSearchMessage(new OrderLookupSearchMessage(this, "Foo"));
            downloader.Verify(d => d.Download("Foo"));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegatesToOnDemandDownlader_WhenUIModeIsBatchMessageIsOrderLookupSearch()
        {
            testObject.InitializeForCurrentScope();

            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.Batch);
            testMessenger.Send(new OrderLookupSearchMessage(this, "Foo"));

            RetryAssertion(() => downloader.Verify(d => d.Download(It.IsAny<string>()), Times.Never));
        }

        [Fact]
        public async Task InitializeForCurrentSession_DelegatesToAutoPrintService_WhenUIModeIsOrderLookupAndMessageIsSingleScan()
        {
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);
            orderRepository.Setup(o => o.GetOrderIDs("Foo")).Returns(new List<long> { 123 });

            SingleScanMessage singleScanMessage = new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero));

            await testObject.OnSingleScanMessage(singleScanMessage);

            autoPrintService.Verify(a => a.AutoPrintShipment(123, singleScanMessage.ScannedText));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegateToAutoPrintService_WhenUIModeIsOrderLookupAndMessageIsOrderLookupSearch()
        {
            testObject.InitializeForCurrentScope();

            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);
            orderRepository.Setup(o => o.GetOrderIDs("Foo")).Returns(new List<long> { 123 });
            OrderLookupSearchMessage message = new OrderLookupSearchMessage(this, "Foo");
            testMessenger.Send(message);

            RetryAssertion(() => autoPrintService.Verify(a => a.AutoPrintShipment(It.IsAny<long>(), It.IsAny<string>()), Times.Never));
        }

        [Fact]
        public void InitializeForCurrentSession_DelegatesToOrderLookupOrderRepository_WhenUIModeIsOrderLookupAndMessageIsSingleScan()
        {
            testObject.InitializeForCurrentScope();

            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);
            SingleScanMessage singleScanMessage = new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero));
            testMessenger.Send(singleScanMessage);

            RetryAssertion(() => orderRepository.Verify(o => o.GetOrderIDs("Foo")));
        }

        [Fact]
        public void InitializeForCurrentScope_DisablesScanPack_WhenFeatureRestricted()
        {
            licenseService
                .Setup(l => l.CheckRestriction(EditionFeature.Warehouse, null))
                .Returns(EditionRestrictionLevel.Forbidden);

            testObject.InitializeForCurrentScope();

            scanPackViewModel.VerifySet(s => s.Enabled = false);
        }

        [Fact]
        public void InitializeForCurrentScope_EnablesScanPack_WhenFeatureRestricted()
        {
            licenseService
                .Setup(l => l.CheckRestriction(EditionFeature.Warehouse, null))
                .Returns(EditionRestrictionLevel.None);

            testObject.InitializeForCurrentScope();

            scanPackViewModel.VerifySet(s => s.Enabled = true);
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesSingleScanMessage_WhenScanPackIsNotActive()
        {
            isPackTabActive = false;

            testObject.InitializeForCurrentScope();

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            scheduler.Start();

            Assert.True(testMessenger.SentMessages.OfType<OrderLookupLoadOrderMessage>().Any(m => m.Order.Equals(order)));
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesSingleScanMessage_WhenScanPackIsActive()
        {
            testObject.InitializeForCurrentScope();

            isPackTabActive = true;

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            scheduler.Start();

            Assert.True(testMessenger.SentMessages.OfType<OrderLookupLoadOrderMessage>().Any(m => m.Order.Equals(order)));
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesOrderLookupSearchMessage_WhenScanPackIsNotActive()
        {
            isPackTabActive = false;

            testMessenger.Send(new OrderLookupSearchMessage(this, "blah"));

            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<OrderLookupLoadOrderMessage>());
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesOrderLookupSearchMessage_WhenScanPackIsActive()
        {
            isPackTabActive = true;

            testObject.InitializeForCurrentScope();

            testMessenger.Send(new OrderLookupSearchMessage(this, "blah"));

            scheduler.Start();

            RetryAssertion(() => Assert.True(testMessenger.SentMessages.OfType<OrderLookupLoadOrderMessage>().Any(m => m.Order.Equals(order))));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void InitializeForCurrentScope_HandlesOrderLookupLoadOrderMessage_WhenScanPackIsNotActive(bool isPackTabActive)
        {
            testObject.InitializeForCurrentScope();

            this.isPackTabActive = isPackTabActive;

            order.OrderNumber = 123;

            testMessenger.Send(new OrderLookupLoadOrderMessage(this, order));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.LoadOrder(order));
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesOrderLookupClearOrderMessage_WhenClearReasonIsReset()
        {
            testObject.InitializeForCurrentScope();
            testMessenger.Send(new OrderLookupClearOrderMessage(this, OrderClearReason.Reset));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.Reset());
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesOrderLookupClearOrderMessage_WhenClearReasonIsNotReset()
        {
            testObject.InitializeForCurrentScope();

            testMessenger.Send(new OrderLookupClearOrderMessage(this, OrderClearReason.OrderNotFound));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.Reset(), Times.Never);
        }

        /// <summary>
        /// Retry a given assertion, since we may need to wait for actions to complete
        /// </summary>
        /// <param name="action"></param>
        private Task RetryAssertion(Action action)
        {
            return Functional.RetryAsync(() =>
            {
                action();
                return Task.FromResult(Unit.Default);
            }, 5, TimeSpan.FromSeconds(250), ex => true);
        }
    }
}
