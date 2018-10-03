using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class ViewModelOrchestratorTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;

        public ViewModelOrchestratorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);
        }

        [Fact]
        public void InitializeForCurrentSession_SubscribesToOrderFoundMessage()
        {
            ViewModelOrchestrator testObject = mock.Create<ViewModelOrchestrator>();

            testObject.InitializeForCurrentSession();

            OrderEntity order = new OrderEntity();
            order.Shipments.Add(new ShipmentEntity());
            testMessenger.Send(new OrderLookupSingleScanMessage(this, order));
            
            Assert.Equal(order, testObject.SelectedOrder);
        }

        [Fact]
        public void RaisePropertyChanged_RaisesPropertyChangedWithNameOfProperty()
        {
            ViewModelOrchestrator testObject = mock.Create<ViewModelOrchestrator>();
            Assert.PropertyChanged(testObject, "FooBar", () => testObject.RaisePropertyChanged("FooBar"));
        }
    }
}
