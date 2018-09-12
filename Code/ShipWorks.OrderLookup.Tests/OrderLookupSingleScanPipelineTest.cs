using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Settings;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupSingleScanPipelineTest
    {
        private readonly AutoMock mock;
        readonly TestMessenger testMessenger;
        private readonly Mock<IOrderRepository> orderRepository;
        private readonly Mock<IMainForm> mainForm;
        private readonly Mock<ICurrentUserSettings> currentUserSettings;
        private readonly OrderLookupSingleScanPipeline testObject;
        private readonly TestScheduler scheduler;

        public OrderLookupSingleScanPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);


            orderRepository = mock.Mock<IOrderRepository>();
            mainForm = mock.Mock<IMainForm>();
            currentUserSettings = mock.Mock<ICurrentUserSettings>();

            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            currentUserSettings.Setup(u => u.GetUIMode()).Returns(UIMode.OrderLookup);

            testObject = mock.Create<OrderLookupSingleScanPipeline>();
            testObject.InitializeForCurrentSession();
        }

        [Fact]
        public void InitializeForCurrentSession_DelegatesToOrderLookupService_WhenNoAdditionalFormsAreNotOpenAndUIModeIsOrderLookup()
        {
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            orderRepository.Verify(o => o.FindOrder("Foo"), Times.Once);
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegateToOrderLookupService_WhenNoAdditionalFormsAreOpenAndUIModeIsBatch()
        {
            currentUserSettings.Setup(u => u.GetUIMode()).Returns(UIMode.Batch);

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            orderRepository.Verify(o => o.FindOrder(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegateToOrderLookupService_WhenAdditionalFormsAreOpenAndUIModeIsOrderLookup()
        {
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(true);

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            orderRepository.Verify(o => o.FindOrder(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurentSession_SendsOrderLookupSingleScanMessageFromOrderLookupService()
        {
            OrderEntity order = new OrderEntity() { IsNew = false };
            orderRepository.Setup(o => o.FindOrder("Foo")).ReturnsAsync(order);

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            scheduler.Start();
            
            Assert.Equal(order, testMessenger.SentMessages.OfType<OrderLookupSingleScanMessage>().Single().Order);
        }

        [Fact]
        public void InitializeForCurentSession_DoesNotSendOrderLookupSingleScanMessageFromOrderLookupService_WhenOrderIsNull()
        {
            orderRepository.Setup(o => o.FindOrder("Foo")).ReturnsAsync((OrderEntity) null);

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            Assert.Null(testMessenger.SentMessages.OfType<OrderLookupSingleScanMessage>().Single().Order);
        }
    }
}
