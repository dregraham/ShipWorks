using Autofac.Extras.Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupShipmentModelTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;

        public OrderLookupShipmentModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);
        }

        [Fact]
        public void InitializeForCurrentSession_SubscribesToOrderFoundMessage()
        {
            OrderLookupShipmentModel testObject = mock.Create<OrderLookupShipmentModel>();

            testObject.InitializeForCurrentSession();

            OrderEntity order = new OrderEntity();
            order.Shipments.Add(new ShipmentEntity());
            testMessenger.Send(new OrderLookupSingleScanMessage(this, order));
            
            Assert.Equal(order, testObject.SelectedOrder);
        }

        [Fact]
        public void RaisePropertyChanged_RaisesPropertyChangedWithNameOfProperty()
        {
            OrderLookupShipmentModel testObject = mock.Create<OrderLookupShipmentModel>();
            Assert.PropertyChanged(testObject, "FooBar", () => testObject.RaisePropertyChanged("FooBar"));
        }
    }
}
