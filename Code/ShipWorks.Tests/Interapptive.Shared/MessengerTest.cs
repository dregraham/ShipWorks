﻿using System;
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
            messenger.Handle<TestMessage>(this, x => wasCalled = true);
            messenger.Send(Mock.Of<IShipWorksMessage>());
            Assert.IsFalse(wasCalled);
        }

        [TestMethod]
        public void Handle_GetsCalled_OnMessageSend()
        {
            bool wasCalled = false;
            messenger.Handle<TestMessage>(this, x => wasCalled = true);
            messenger.Send(new TestMessage());
            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        public void Handle_WithTwoHandlers_BothGetCalled()
        {
            bool wasCalled1 = false;
            bool wasCalled2 = false;
            messenger.Handle<TestMessage>(this, x => wasCalled1 = true);
            messenger.Handle<TestMessage>(this, x => wasCalled2 = true);
            messenger.Send(new TestMessage());
            Assert.IsTrue(wasCalled1);
            Assert.IsTrue(wasCalled2);
        }

        [TestMethod]
        public void Handle_WithExistingHandler_ReturnsExistingToken()
        {
            using (DisposableHandler handler = new DisposableHandler())
            {
                MessengerToken firstToken = messenger.Handle(handler, handler.Handler);
                MessengerToken secondToken = messenger.Handle(handler, handler.Handler);

                Assert.AreEqual(firstToken, secondToken);
            }
        }

        [TestMethod]
        public void Handle_WithExistingHandler_CallsHandlerOnceOnSend()
        {
            using (DisposableHandler handler = new DisposableHandler())
            {
                messenger.Handle(handler, handler.Handler);
                messenger.Handle(handler, handler.Handler);
                messenger.Send(new TestMessage());

                Assert.AreEqual(1, handler.Calls);
            }
        }

        [TestMethod]
        public void Handle_HandlerDoesNotGetCalled_WhenRemoved()
        {
            bool wasCalled1 = false;
            bool wasCalled2 = false;
            MessengerToken token = messenger.Handle<TestMessage>(this, x => wasCalled1 = true);
            messenger.Handle<TestMessage>(this, x => wasCalled2 = true);
            messenger.Remove(token);
            messenger.Send(new TestMessage());
            Assert.IsFalse(wasCalled1);
            Assert.IsTrue(wasCalled2);
        }

        [TestMethod]
        public void Handle_HandlerDoesNotGetCalled_WhenRemovedByReference()
        {
            bool wasCalled1 = false;
            bool wasCalled2 = false;

            Action<TestMessage> handler1 = x => wasCalled1 = true;

            messenger.Handle(this, handler1);
            messenger.Handle<TestMessage>(this, x => wasCalled2 = true);
            messenger.Remove(this, handler1);
            messenger.Send(new TestMessage());
            Assert.IsFalse(wasCalled1);
            Assert.IsTrue(wasCalled2);
        }

        [TestMethod]
        public void Send_DoesNotThrow_WhenHandlerHasBeenDisposed()
        {
            DisposableHandler handler = new DisposableHandler();
            messenger.Handle(handler, handler.Handler);
            handler.Dispose();
            GC.Collect();
            messenger.Send(new TestMessage());
        }

        [TestMethod]
        public void Send_DoesNotCallMethod_WhenObjectHasBeenCollected()
        {
            bool wasCalled = false;
            DisposableHandler handler = new DisposableHandler(x => wasCalled = true);
            messenger.Handle(handler, handler.Handler);
            handler.Dispose();
            handler = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            messenger.Send(new TestMessage());
            Assert.IsFalse(wasCalled);
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