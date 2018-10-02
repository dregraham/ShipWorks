using System.ComponentModel;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupSearchViewModelTest
    {
        private readonly AutoMock mock;

        public OrderLookupSearchViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }
        
        [Fact]
        public void UpdateOrder_SetsOrderNumber_WhenOrderIsFound()
        {
            var dataService = mock.Mock<IViewModelOrchestrator>();
            dataService.Setup(d => d.Order).Returns(new OrderEntity()
            {
                OrderNumber = 123
            });
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();
            
            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("Order"));
            
            Assert.Equal("123", testObject.OrderNumber);
        }

        [Fact]
        public void UpdateOrder_SetsErrorMessageToEmptyString_WhenOrderIsFound()
        {
            var dataService = mock.Mock<IViewModelOrchestrator>();
            dataService.Setup(d => d.Order).Returns(new OrderEntity()
            {
                OrderNumber = 123
            });
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("Order"));

            Assert.Equal(string.Empty, testObject.SearchErrorMessage);
        }

        [Fact]
        public void UpdateOrder_SetsSearchErrorToFalse_WhenOrderIsFound()
        {
            var dataService = mock.Mock<IViewModelOrchestrator>();
            dataService.Setup(d => d.Order).Returns(new OrderEntity()
            {
                OrderNumber = 123
            });
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("Order"));

            Assert.False(testObject.SearchError);
        }

        [Fact]
        public void UpdateOrder_SetsOrderNumberToEmptyString_WhenNoOrderIsFound()
        {
            var dataService = mock.Mock<IViewModelOrchestrator>();
            dataService.Setup(d => d.Order).Returns<OrderEntity>(null);
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("Order"));

            Assert.Equal(string.Empty, testObject.OrderNumber);
        }

        [Fact]
        public void UpdateOrder_SetsErrorMessage_WhenOrderIsNotFound()
        {
            var dataService = mock.Mock<IViewModelOrchestrator>();
            dataService.Setup(d => d.Order).Returns<OrderEntity>(null);
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("Order"));

            Assert.Equal("No matching orders were found.", testObject.SearchErrorMessage);
        }

        [Fact]
        public void UpdateOrder_SetsErrorToTrue_WhenOrderIsNotFound()
        {
            var dataService = mock.Mock<IViewModelOrchestrator>();
            dataService.Setup(d => d.Order).Returns<OrderEntity>(null);

            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("Order"));

            Assert.True(testObject.SearchError);
        }

        [Fact]
        public void UpdateOrder_DoesNothing_WhenPropertyChangesThatIsNotOrder()
        {
            var dataService = mock.Mock<IViewModelOrchestrator>();
            dataService.Setup(d => d.Order).Returns(new OrderEntity()
            {
                OrderNumber = 123
            });
            OrderLookupSearchViewModel testObject = mock.Create<OrderLookupSearchViewModel>();

            dataService.Raise(d => d.PropertyChanged += null, new PropertyChangedEventArgs("Foo"));

            Assert.Equal(string.Empty, testObject.OrderNumber);
            Assert.Equal(string.Empty, testObject.SearchErrorMessage);
        }
    }
}