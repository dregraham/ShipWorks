using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Settings;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupSingleScanPipelineTest
    {
        private readonly AutoMock mock;
        TestMessenger testMessenger;
        private readonly Mock<IOrderLookupService> orderLookupService;
        private readonly Mock<IMainForm> mainForm;
        private readonly Mock<ICurrentUserSettings> currentUserSettings;
        private readonly OrderLookupSingleScanPipeline testObject;

        public OrderLookupSingleScanPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orderLookupService = mock.Mock<IOrderLookupService>();
            mainForm = mock.Mock<IMainForm>();
            currentUserSettings = mock.Mock<ICurrentUserSettings>();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            currentUserSettings.Setup(u => u.GetUIMode()).Returns(UIMode.OrderLookup);

            testObject = mock.Create<OrderLookupSingleScanPipeline>();
            testObject.InitializeForCurrentSession();
        }

        [Fact]
        public void InitializeForCurrentSession_DelegatesToOrderLookupService_WhenNoAdditionalFormsAreNotOpenAndUIModeIsOrderLookup()
        {
            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            orderLookupService.Verify(o => o.FindOrder("Foo"));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegateToOrderLookupService_WhenNoAdditionalFormsAreOpenAndUIModeIsBatch()
        {
            currentUserSettings.Setup(u => u.GetUIMode()).Returns(UIMode.Batch);

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            orderLookupService.Verify(o => o.FindOrder(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegateToOrderLookupService_WhenAdditionalFormsAreOpenAndUIModeIsOrderLookup()
        {
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(true);

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            orderLookupService.Verify(o => o.FindOrder(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurentSession_SendsOrderLookupSingleScanMessageFromOrderLookupService()
        {
            testMessenger = new TestMessenger();

            OrderEntity order = new OrderEntity() { IsNew = false };
            orderLookupService.Setup(o => o.FindOrder("Foo")).ReturnsAsync(order);

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            Assert.Equal(order, testMessenger.SentMessages.OfType<OrderLookupSingleScanMessage>().Single().Order);
        }

        [Fact]
        public void InitializeForCurentSession_DoesNotSendOrderLookupSingleScanMessageFromOrderLookupService_WhenOrderIsNew()
        {
            testMessenger = new TestMessenger();

            OrderEntity order = new OrderEntity() { IsNew = true };
            orderLookupService.Setup(o => o.FindOrder("Foo")).ReturnsAsync(order);

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "Foo", IntPtr.Zero)));

            Assert.Empty(testMessenger.SentMessages.OfType<OrderLookupSingleScanMessage>());
        }
    }
}
