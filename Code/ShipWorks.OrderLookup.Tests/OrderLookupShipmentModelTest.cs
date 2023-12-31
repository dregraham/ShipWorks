﻿using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupShipmentModelTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly OrderLookupShipmentModel testObject;
        private OrderEntity order;
        private ShipmentEntity shipment;
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private Mock<IShippingManager> shippingManager;

        public OrderLookupShipmentModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(PermissionType.ShipmentsCreateEditProcess, AnyLong))
                .Returns(true);

            testObject = mock.Create<OrderLookupShipmentModel>();
            testObject.CreateLabelWrapper = x => Task.FromResult(x());
        }

        private void SetupTestMocks(bool isProcessed)
        {
            order = new OrderEntity { OrderNumber = 1234 };
            shipment = new ShipmentEntity { ShipmentID = 4567, Processed = isProcessed };
            shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shippingManager = mock.Mock<IShippingManager>();

            shipmentAdapter.Setup(x => x.Shipment).Returns(shipment);

            shippingManager.Setup(x => x.GetShipmentAdapter(It.IsAny<ShipmentEntity>())).Returns(shipmentAdapter.Object);

            order.Shipments.Add(shipmentAdapter.Object.Shipment);
        }

        [Fact]
        public void RaisePropertyChanged_RaisesPropertyChangedWithNameOfProperty()
        {
            Assert.PropertyChanged(testObject, "FooBar", () => testObject.RaisePropertyChanged("FooBar"));
        }

        [Fact]
        public async Task CreateLabel_DoesNotReturnMessage_WhenOrderIsProcessedAsync()
        {
            SetupTestMocks(true);

            testObject.LoadOrder(order);
            await testObject.CreateLabel();

            Assert.Empty(testMessenger.SentMessages.OfType<ProcessShipmentsMessage>());
        }

        [Fact]
        public async Task CreateLabel_DoesReturnMessage_WhenOrderIsNotProcessedAsync()
        {
            SetupTestMocks(false);

            testObject.LoadOrder(order);
            await testObject.CreateLabel();

            Assert.Equal(1, testMessenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
        }
    }
}
