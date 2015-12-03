using System;
using System.Linq;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.UI.MessageHandlers;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class LoadOrderOnSelectionChangedPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly ISubject<OrderSelectionChangingMessage> orderChangingSubject;
        readonly ISubject<OrderSelectionChangedMessage> orderChangedSubject;
        readonly TestSchedulerProvider schedulerProvider;

        public LoadOrderOnSelectionChangedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orderChangingSubject = new Subject<OrderSelectionChangingMessage>();
            orderChangedSubject = new Subject<OrderSelectionChangedMessage>();

            mock.Mock<IOrderSelectionChangedHandler>()
                .Setup(x => x.OrderChangingStream()).Returns(orderChangingSubject);
            mock.Mock<IOrderSelectionChangedHandler>()
                .Setup(x => x.ShipmentLoadedStream()).Returns(orderChangedSubject);

            schedulerProvider = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(schedulerProvider);
        }

        [Fact]
        public void Register_SetsAllowEditingToFalse_WhenOrderChangingMessageIsSent()
        {
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>();
            var testObject = mock.Create<LoadOrderOnSelectionChangedPipeline>();
            testObject.Register(viewModelMock.Object);

            orderChangingSubject.OnNext(new OrderSelectionChangingMessage(this, new[] { 1L }));

            viewModelMock.VerifySet(x => x.AllowEditing = false);
        }

        [Fact]
        public void Register_SetsAllowEditingToTrue_WhenOrderChangedMessageIsSent()
        {
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>();
            var testObject = mock.Create<LoadOrderOnSelectionChangedPipeline>();
            testObject.Register(viewModelMock.Object);

            orderChangedSubject.OnNext(new OrderSelectionChangedMessage(this, Enumerable.Empty<OrderSelectionLoaded>()));

            schedulerProvider.Dispatcher.AdvanceBy(1);

            viewModelMock.VerifySet(x => x.AllowEditing = true);
        }

        [Fact]
        public void Register_CallsLoadOrderOnViewModel_WhenOrderChangedMessageIsSent()
        {
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>();
            var testObject = mock.Create<LoadOrderOnSelectionChangedPipeline>();
            testObject.Register(viewModelMock.Object);

            var message = new OrderSelectionChangedMessage(this, Enumerable.Empty<OrderSelectionLoaded>());
            orderChangedSubject.OnNext(message);

            schedulerProvider.Dispatcher.AdvanceBy(1);

            viewModelMock.Verify(x => x.LoadOrder(message));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
