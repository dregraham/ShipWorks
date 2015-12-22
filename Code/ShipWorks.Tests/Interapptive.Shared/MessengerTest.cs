using System;
using Xunit;
using Moq;
using Interapptive.Shared.Messaging;
using System.Reactive.Linq;

namespace ShipWorks.Tests.Interapptive.Shared
{
    public class MessengerTest
    {
        private Messenger messenger;

        public MessengerTest()
        {
            messenger = new Messenger();
        }

        [Fact]
        public void Send_DoesNotCallAnything_WhenNoHandlersAreSetup()
        {
            messenger.Send(new TestMessage());
        }

        [Fact]
        public void Send_DoesNotCallHandler_WhenHandlerIsForDifferentMessage()
        {
            bool wasCalled = false;
            messenger.OfType<TestMessage>().Subscribe(x => wasCalled = true);
            messenger.Send(Mock.Of<IShipWorksMessage>());
            Assert.False(wasCalled);
        }

        [Fact]
        public void Handle_GetsCalled_OnMessageSend()
        {
            bool wasCalled = false;
            messenger.OfType<TestMessage>().Subscribe(x => wasCalled = true);
            messenger.Send(new TestMessage());
            Assert.True(wasCalled);
        }

        [Fact]
        public void Handle_WithTwoHandlers_BothGetCalled()
        {
            bool wasCalled1 = false;
            bool wasCalled2 = false;
            messenger.OfType<TestMessage>().Subscribe(x => wasCalled1 = true);
            messenger.OfType<TestMessage>().Subscribe(x => wasCalled2 = true);
            messenger.Send(new TestMessage());
            Assert.True(wasCalled1);
            Assert.True(wasCalled2);
        }

        [Fact]
        public void Handle_HandlerDoesNotGetCalled_WhenRemoved()
        {
            bool wasCalled1 = false;
            bool wasCalled2 = false;
            IDisposable token = messenger.OfType<TestMessage>().Subscribe(x => wasCalled1 = true);
            messenger.OfType<TestMessage>().Subscribe(x => wasCalled2 = true);
            token.Dispose();
            messenger.Send(new TestMessage());
            Assert.False(wasCalled1);
            Assert.True(wasCalled2);
        }

        [Fact]
        public void Send_DoesNotThrow_WhenHandlerHasBeenDisposed()
        {
            DisposableHandler handler = new DisposableHandler();
            messenger.OfType<TestMessage>().Subscribe(handler.Handler);
            handler.Dispose();
            GC.Collect();
            messenger.Send(new TestMessage());
        }

        [Fact]
        public void Send_RoutesMessagesThroughObservable_WhenMessageIsOfSpecifiedType()
        {
            bool wasCalled = false;
            messenger.OfType<TestMessage>().Subscribe(x => wasCalled = true);
            messenger.Send(new TestMessage());
            Assert.True(wasCalled);
        }

        [Fact]
        public void Send_DoesNotRouteMessagesThroughObservable_WhenMessageIsNotOfSpecifiedType()
        {
            bool wasCalled = false;
            messenger.OfType<OtherMessage>().Subscribe(x => wasCalled = true);
            messenger.Send(new TestMessage());
            Assert.False(wasCalled);
        }

        private class TestMessage : IShipWorksMessage
        {
            public object Sender { get; private set; }
        }

        private class OtherMessage : IShipWorksMessage
        {
            public object Sender { get; private set; }
        }

        private class DisposableHandler : IDisposable
        {
            public DisposableHandler()
            {
                Handler = x => Calls++;
            }

            public DisposableHandler(Action<TestMessage> handler)
            {
                Handler = handler;
            }

            public Action<TestMessage> Handler { get; private set; }

            public int Calls { get; private set; }

            public void Dispose()
            {
                Handler = null;
            }
        }
    }
}
