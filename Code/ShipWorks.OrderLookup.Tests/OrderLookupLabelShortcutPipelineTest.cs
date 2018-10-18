using System;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Settings;
using ShipWorks.Shipping.Services;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupLabelShortcutPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestScheduler scheduler;
        private TestMessenger testMessenger;
        private OrderLookupLabelShortcutPipeline testObject;
        private Mock<IOrderLookupShipmentModel> viewModel;
        private Mock<ISecurityContext> securityContext;
        private Mock<ICurrentUserSettings> currentUserSettings;

        public OrderLookupLabelShortcutPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();
            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);
        }

        private void CreateTestObject(bool hasPermission, UIMode uiMode, bool shipmentIsProcessed)
        {
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            securityContext = mock.Mock<ISecurityContext>();
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            currentUserSettings = mock.Mock<ICurrentUserSettings>();
            currentUserSettings.Setup(cus => cus.GetUIMode()).Returns(uiMode);

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(new ShipmentEntity() { ShipmentID = 456, Processed = shipmentIsProcessed });

            viewModel = mock.CreateMock<IOrderLookupShipmentModel>();
            viewModel.Setup(d => d.SelectedOrder).Returns(new OrderEntity() { OrderNumber = 123 });
            viewModel.Setup(d => d.ShipmentAdapter).Returns(shipmentAdapter.Object);

            testObject = mock.Create<OrderLookupLabelShortcutPipeline>();

            testObject.Register(viewModel.Object);
        }

        [Fact]
        public void Register_SendsProcessShipmentsMessage_WhenShortcutMessageReceived_AndAppliesToCreateLabel()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.CreateLabel },
                ShortcutTriggerType.Barcode, "-PL-");

            CreateTestObject(true, UIMode.OrderLookup, false);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Equal(1, testMessenger.SentMessages.OfType<ProcessShipmentsMessage>()
                .Count(m => m.Shipments.First().ShipmentID == 456 && m.Sender == testObject));
        }
        
        [Theory]
        [InlineData(false, UIMode.OrderLookup, true, 0)]
        [InlineData(false, UIMode.OrderLookup, false, 0)]
        [InlineData(false, UIMode.Batch, true, 0)]
        [InlineData(false, UIMode.Batch, false, 0)]
        [InlineData(true, UIMode.OrderLookup, true, 0)]
        [InlineData(true, UIMode.OrderLookup, false, 1)]
        [InlineData(true, UIMode.Batch, true, 0)]
        [InlineData(true, UIMode.Batch, false, 0)]
        public void Register_SendsMessage_WhenAppropriate(bool hasPermission, 
                            UIMode uiMode, bool shipmentIsProcessed, int messagesSent)
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.CreateLabel },
                ShortcutTriggerType.Barcode, "-PL-");

            CreateTestObject(hasPermission, uiMode, shipmentIsProcessed);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Equal(messagesSent, testMessenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
        }

        [Fact]
        public void Register_DoesNotSendMessage_WhenShortcutActionDoesNotApply()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyWeight, RelatedObjectID = 789 },
                ShortcutTriggerType.Barcode, "123");

            CreateTestObject(true, UIMode.OrderLookup, false);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<ProcessShipmentsMessage>());
        }
        
        [Fact]
        public void Register_DoesNotSendMessage_ViewModelDoesNotHaveShipment()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyWeight, RelatedObjectID = 789 },
                ShortcutTriggerType.Barcode, "123");

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            securityContext = mock.Mock<ISecurityContext>();
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(true);

            currentUserSettings = mock.Mock<ICurrentUserSettings>();
            currentUserSettings.Setup(cus => cus.GetUIMode()).Returns(UIMode.OrderLookup);

            viewModel = mock.CreateMock<IOrderLookupShipmentModel>();
            viewModel.Setup(d => d.SelectedOrder).Returns(new OrderEntity() { OrderNumber = 123 });
            viewModel.Setup(d => d.ShipmentAdapter).Returns((ICarrierShipmentAdapter) null);

            testObject = mock.Create<OrderLookupLabelShortcutPipeline>();

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<ProcessShipmentsMessage>());
        }
        
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}