using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupShipmentModelTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private OrderLookupShipmentModel testObject;
        private OrderEntity order;
        private ShipmentEntity shipment;
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private Mock<IShippingManager> shippingManager;

        public OrderLookupShipmentModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);
        }

        public void CreateTestObject(bool isProcessed)
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
            testObject = mock.Create<OrderLookupShipmentModel>();
            Assert.PropertyChanged(testObject, "FooBar", () => testObject.RaisePropertyChanged("FooBar"));
        }

        [Fact]
        public void CreateLabel_DoesNotReturnMessage_WhenOrderIsProcessed()
        {
            CreateTestObject(true);
            
            testObject = mock.Create<OrderLookupShipmentModel>();
            testObject.LoadOrder(order);
            testObject.CreateLabel(); 

            Assert.Empty(testMessenger.SentMessages.OfType<ProcessShipmentsMessage>());
        }

        [Fact]
        public void CreateLabel_DoesReturnMessage_WhenOrderIsNotProcessed()
        {
            CreateTestObject(false);

            testObject = mock.Create<OrderLookupShipmentModel>();
            testObject.LoadOrder(order);
            testObject.CreateLabel();

            Assert.Equal(1, testMessenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
        }
    }
}
