using System.ComponentModel;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;
using ShipWorks.OrderLookup.ScanToShip;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupSearchViewModelTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ISecurityContext> securityContext;

        public OrderLookupSearchViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            securityContext = mock.Mock<ISecurityContext>();
        }

        [Theory]
        [InlineData(true, false, true)]
        [InlineData(false, false, false)]
        [InlineData(true, true, false)]
        [InlineData(false, true, false)]
        public void ShowCreateLabel_ReturnsCorrectValue_BasedOnCreateLabelPermission(bool hasPermission, bool shipmentIsProcessed, bool expectedValue)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(new ShipmentEntity() {Processed = shipmentIsProcessed });

            var orderLookupShipmentModel = mock.Mock<IOrderLookupShipmentModel>();
            orderLookupShipmentModel.Setup(d => d.SelectedOrder).Returns(new OrderEntity() { OrderNumber = 123 });
            orderLookupShipmentModel.Setup(d => d.ShipmentAdapter).Returns(shipmentAdapter.Object);

            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();
            testObject.SelectedTab = ScanToShipTab.ShipTab;

            orderLookupShipmentModel.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("SelectedOrder"));

            Assert.Equal(expectedValue, testObject.ShowCreateLabel);
        }

        [Fact]
        public void UpdateOrder_SetsOrderNumber_WhenOrderIsFound()
        {
            var dataService = mock.Mock<IOrderLookupShipmentModel>();
            dataService.Setup(d => d.SelectedOrder).Returns(new OrderEntity()
            {
                OrderNumber = 123
            });
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("SelectedOrder"));

            Assert.Equal("123", testObject.OrderNumber);
        }

        [Fact]
        public void UpdateOrder_SetsErrorMessageToEmptyString_WhenOrderIsFound()
        {
            var dataService = mock.Mock<IOrderLookupShipmentModel>();
            dataService.Setup(d => d.SelectedOrder).Returns(new OrderEntity()
            {
                OrderNumber = 123
            });
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("SelectedOrder"));

            Assert.Equal(string.Empty, testObject.SearchMessage);
        }

        [Fact]
        public void UpdateOrder_SetsSearchErrorToFalse_WhenOrderIsFound()
        {
            var dataService = mock.Mock<IOrderLookupShipmentModel>();
            dataService.Setup(d => d.SelectedOrder).Returns(new OrderEntity()
            {
                OrderNumber = 123
            });
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("SelectedOrder"));

            Assert.False(testObject.ShowSearchMessage);
        }

        [Fact]
        public void UpdateOrder_SetsOrderNumberToEmptyString_WhenNoOrderIsFound()
        {
            var dataService = mock.Mock<IOrderLookupShipmentModel>();
            dataService.Setup(d => d.SelectedOrder).Returns<OrderEntity>(null);
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("SelectedOrder"));

            Assert.Equal(string.Empty, testObject.OrderNumber);
        }

        [Fact]
        public void UpdateOrder_SetsErrorMessage_WhenOrderIsNotFound()
        {
            var dataService = mock.Mock<IOrderLookupShipmentModel>();
            dataService.Setup(d => d.SelectedOrder).Returns<OrderEntity>(null);
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("SelectedOrder"));

            Assert.Equal("No matching orders were found.", testObject.SearchMessage);
        }

        [Fact]
        public void UpdateOrder_SetsErrorToTrue_WhenOrderIsNotFound()
        {
            var dataService = mock.Mock<IOrderLookupShipmentModel>();
            dataService.Setup(d => d.SelectedOrder).Returns<OrderEntity>(null);

            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();
            testObject.SelectedTab = ScanToShipTab.ShipTab;

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("SelectedOrder"));

            Assert.True(testObject.ShowSearchMessage);
        }

        [Fact]
        public void UpdateOrder_DoesNothing_WhenPropertyChangesThatIsNotOrder()
        {
            var dataService = mock.Mock<IOrderLookupShipmentModel>();
            dataService.Setup(d => d.SelectedOrder).Returns(new OrderEntity()
            {
                OrderNumber = 123
            });
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("Foo"));

            Assert.Equal(string.Empty, testObject.OrderNumber);
            Assert.Equal(string.Empty, testObject.SearchMessage);
        }

        [Fact]
        public void GetOrderCommand_SendsOrderLookupSearchMessage()
        {
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            testObject.GetOrderCommand.Execute(null);

            mock.Mock<IMessenger>().Verify(x => x.Send(It.IsAny<OrderLookupSearchMessage>(), It.IsAny<string>()));
        }

        [Fact]
        public void GetOrderCommand_DoesNotResetOrderNumber()
        {
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            testObject.OrderNumber = "1";

            testObject.GetOrderCommand.Execute(null);

            Assert.Equal("1", testObject.OrderNumber);
        }

        [Fact]
        public void Reset_ClearsOrderNumber()
        {
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            testObject.OrderNumber = "1";

            testObject.ResetCommand.Execute(null);

            Assert.Equal(string.Empty, testObject.OrderNumber);
        }

    }
}
