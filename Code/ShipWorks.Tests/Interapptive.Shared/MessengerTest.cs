using System;
using Interapptive.Shared.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ShipWorks.Tests.Interapptive.Shared
{
    [TestClass]
    public class MessengerTest
    {
        private Messenger messenger;

        [TestInitialize]
        public void Initialize()
        {
            messenger = new Messenger();
        }

        [TestMethod]
        public void Send_DoesNotCallAnything_WhenNoHandlersAreSetup()
        {
            messenger.Send(new TestMessage());
        }

        [TestMethod]
        public void Send_DoesNotCallHandler_WhenHandlerIsForDifferentMessage()
        {
            bool wasCalled = false;
            messenger.Handle<TestMessage>(x => wasCalled = true);
            messenger.Send(Mock.Of<IShipWorksMessage>());
            Assert.IsFalse(wasCalled);
        }

        [TestMethod]
        public void Handle_GetsCalled_OnMessageSend()
        {
            bool wasCalled = false;
            messenger.Handle<TestMessage>(x => wasCalled = true);
            messenger.Send(new TestMessage());
            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        public void Handle_WithTwoHandlers_BothGetCalled()
        {
            bool wasCalled1 = false;
            bool wasCalled2 = false;
            messenger.Handle<TestMessage>(x => wasCalled1 = true);
            messenger.Handle<TestMessage>(x => wasCalled2 = true);
            messenger.Send(new TestMessage());
            Assert.IsTrue(wasCalled1);
            Assert.IsTrue(wasCalled2);
        }

        [TestMethod]
        public void Handle_WithExistingHandler_ReturnsExistingToken()
        {
            using (DisposableHandler handler = new DisposableHandler())
            {
                MessengerToken firstToken = messenger.Handle(handler.Handler);
                MessengerToken secondToken = messenger.Handle(handler.Handler);

                Assert.AreEqual(firstToken, secondToken);
            }
        }

        [TestMethod]
        public void Handle_WithExistingHandler_CallsHandlerOnceOnSend()
        {
            using (DisposableHandler handler = new DisposableHandler())
            {
                messenger.Handle(handler.Handler);
                messenger.Handle(handler.Handler);
                messenger.Send(new TestMessage());

                Assert.AreEqual(1, handler.Calls);
            }
        }

        [TestMethod]
        public void Handle_HandlerDoesNotGetCalled_WhenRemoved()
        {
            bool wasCalled1 = false;
            bool wasCalled2 = false;
            MessengerToken token = messenger.Handle<TestMessage>(x => wasCalled1 = true);
            messenger.Handle<TestMessage>(x => wasCalled2 = true);
            messenger.Remove(token);
            messenger.Send(new TestMessage());
            Assert.IsFalse(wasCalled1);
            Assert.IsTrue(wasCalled2);
        }

        [TestMethod]
        public void Send_DoesNotThrow_WhenHandlerHasBeenDisposed()
        {
            DisposableHandler handler = new DisposableHandler();
            messenger.Handle(handler.Handler);
            handler.Dispose();
            GC.Collect();
            messenger.Send(new TestMessage());
        }

        private class TestMessage : IShipWorksMessage
        {
            public object Sender { get; private set; }
        }

        private class DisposableHandler : IDisposable
        {
            public DisposableHandler()
            {
                Handler = x => Calls++;
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
