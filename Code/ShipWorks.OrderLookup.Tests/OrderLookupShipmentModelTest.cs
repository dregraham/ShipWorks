using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupShipmentModelTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private Mock<IOrderLookupShipmentModel> viewModel;

        public OrderLookupShipmentModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);
        }

        [Fact]
        public void RaisePropertyChanged_RaisesPropertyChangedWithNameOfProperty()
        {
            OrderLookupShipmentModel testObject = mock.Create<OrderLookupShipmentModel>();
            Assert.PropertyChanged(testObject, "FooBar", () => testObject.RaisePropertyChanged("FooBar"));
        }

        [Fact]
        public void CreateLabel_DoesNotReturnMessage_WhenOrderIsNotProcessed()
        {
            OrderLookupShipmentModel testObject = mock.Create<OrderLookupShipmentModel>();

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(new ShipmentEntity { ShipmentID = 1, Processed = false });

            viewModel = mock.CreateMock<IOrderLookupShipmentModel>();
            viewModel.Setup(d => d.SelectedOrder).Returns(new OrderEntity { OrderNumber = 123 });
            viewModel.Setup(d => d.ShipmentAdapter).Returns(shipmentAdapter.Object);
            
            testObject.CreateLabel();
            
            Assert.Empty(testMessenger.SentMessages.OfType<ProcessShipmentsMessage>());
        }

        [Fact]
        public void CreateLabel_DoesReturnMessage_WhenOrderIsProcessed()
        {
            OrderLookupShipmentModel testObject = mock.Create<OrderLookupShipmentModel>();

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(new ShipmentEntity { ShipmentID = 1, Processed = true });

            viewModel = mock.CreateMock<IOrderLookupShipmentModel>();
            viewModel.Setup(d => d.SelectedOrder).Returns(new OrderEntity { OrderNumber = 123 });
            viewModel.Setup(d => d.ShipmentAdapter).Returns(shipmentAdapter.Object);

            testObject.CreateLabel();

            Assert.Equal(1, testMessenger.SentMessages.OfType<ProcessShipmentsMessage>().Count());
        }
    }
}
