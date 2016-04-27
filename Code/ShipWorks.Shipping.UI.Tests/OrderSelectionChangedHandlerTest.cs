using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.UI.MessageHandlers;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests
{
    public class OrderSelectionChangedHandlerTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> subject = new Subject<IShipWorksMessage>();

        public OrderSelectionChangedHandlerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);
        }

        [Fact]
        public void ShipmentLoadedStream_SendsChangedMessage_WhenChangingAndCorrespondingChangedReceived()
        {
            OrderSelectionChangedMessage calledMessage = default(OrderSelectionChangedMessage);
            var testObject = mock.Create<OrderSelectionChangedHandler>();
            testObject.ShipmentLoadedStream().Subscribe(x => calledMessage = x);
            var sentMessage = new OrderSelectionChangedMessage(this, new[] { CreateOrderSelection(2) });

            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 2L }));
            subject.OnNext(sentMessage);

            Assert.Equal(sentMessage, calledMessage);
        }

        [Fact]
        public void ShipmentLoadedStream_SendsOneChangedMessage_WhenTwoChangingAndOneCorrespondingChangedReceived()
        {
            int numberOfCalls = 0;
            var testObject = mock.Create<OrderSelectionChangedHandler>();
            testObject.ShipmentLoadedStream().Subscribe(_ => numberOfCalls++);
            var sentMessage = new OrderSelectionChangedMessage(this, new[] { CreateOrderSelection(2) });

            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 2L }));
            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 2L }));
            subject.OnNext(sentMessage);

            Assert.Equal(1, numberOfCalls);
        }

        [Fact]
        public void ShipmentLoadedStream_SendsOneChangedMessage_WhenChangingMessageIsReceivedAfterSendingChanged()
        {
            int numberOfCalls = 0;
            var testObject = mock.Create<OrderSelectionChangedHandler>();
            testObject.ShipmentLoadedStream().Subscribe(_ => numberOfCalls++);
            var sentMessage = new OrderSelectionChangedMessage(this, new[] { CreateOrderSelection(2) });

            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 2L }));
            subject.OnNext(sentMessage);
            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 2L }));

            Assert.Equal(1, numberOfCalls);
        }

        [Fact]
        public void ShipmentLoadedStream_SendsChangedMessageForNewestChangingMessage_WhenTwoChangingMessagesReceivedInARow()
        {
            int numberOfCalls = 0;
            var testObject = mock.Create<OrderSelectionChangedHandler>();
            testObject.ShipmentLoadedStream().Subscribe(_ => numberOfCalls++);
            var sentMessage = new OrderSelectionChangedMessage(this, new[] { CreateOrderSelection(2) });

            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 1L }));
            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 2L }));
            subject.OnNext(sentMessage);

            Assert.Equal(1, numberOfCalls);
        }

        [Fact]
        public void ShipmentLoadedStream_SendsTwoChangedMessages_WhenTwoSetsOfCorrespondingMessagesAreReceived()
        {
            var calledMessages = new List<OrderSelectionChangedMessage>();
            var testObject = mock.Create<OrderSelectionChangedHandler>();
            testObject.ShipmentLoadedStream().Subscribe(calledMessages.Add);

            var firstMessage = new OrderSelectionChangedMessage(this, new[] { CreateOrderSelection(2) });
            var secondMessage = new OrderSelectionChangedMessage(this, new[] { CreateOrderSelection(2) });

            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 2L }));
            subject.OnNext(firstMessage);
            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 2L }));
            subject.OnNext(secondMessage);

            Assert.Equal(calledMessages[0], firstMessage);
            Assert.Equal(calledMessages[1], secondMessage);
        }

        [Fact]
        public void ShipmentLoadedStream_DoesNotSendMessage_WhenOrderListsDontMatch()
        {
            int numberOfCalls = 0;
            var testObject = mock.Create<OrderSelectionChangedHandler>();
            testObject.ShipmentLoadedStream().Subscribe(_ => numberOfCalls++);

            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 1L, 2L }));
            subject.OnNext(new OrderSelectionChangedMessage(this, new[] { CreateOrderSelection(2) }));

            Assert.Equal(0, numberOfCalls);
        }

        [Fact]
        public void ShipmentLoadedStream_DoesNotSendMessage_WhenOrderListsDontMatch2()
        {
            int numberOfCalls = 0;
            var testObject = mock.Create<OrderSelectionChangedHandler>();
            testObject.ShipmentLoadedStream().Subscribe(_ => numberOfCalls++);

            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 2L }));
            subject.OnNext(new OrderSelectionChangedMessage(this, new[] {
                CreateOrderSelection(1),
                CreateOrderSelection(2)
            }));

            Assert.Equal(0, numberOfCalls);
        }

        [Fact]
        public void ShipmentLoadedStream_SendsMessage_WhenOrderListIsNotInSameOrder()
        {
            int numberOfCalls = 0;
            var testObject = mock.Create<OrderSelectionChangedHandler>();
            testObject.ShipmentLoadedStream().Subscribe(_ => numberOfCalls++);

            subject.OnNext(new OrderSelectionChangingMessage(this, new[] { 2L, 1L }));
            subject.OnNext(new OrderSelectionChangedMessage(this, new[]
            {
                CreateOrderSelection(1),
                CreateOrderSelection(2)
            }));

            Assert.Equal(1, numberOfCalls);
        }

        private IOrderSelection CreateOrderSelection(int orderID) =>
            mock.CreateMock<IOrderSelection>(x => x.SetupGet(o => o.OrderID).Returns(orderID)).Object;

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
