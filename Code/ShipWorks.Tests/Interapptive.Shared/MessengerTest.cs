using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Messaging;
using Moq;
using ShipWorks.Core.Messaging;
using Xunit;

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
        public void LoadTest_GetsCalled_OnMessageSend()
        {
            int[] nums = Enumerable.Range(1, 1000000).ToArray();
            long total = 0;
            long count = (long) nums.Length;
            long expectedResult = ((count * count) + count) / 2;
            Stopwatch sw = new Stopwatch();

            messenger.OfType<TestMessage>().Subscribe(x =>
            {
                x.Update();
            });

            sw.Start();
            // Use type parameter to make subtotal a long, not an int
            Parallel.For<long>(0, nums.Length, () => 0,
                (j, loop, subtotal) =>
                {
                    TestMessage message = new TestMessage();
                    message.Payload = j;
                    message.Update = () =>
                    {
                        subtotal += nums[j];
                    };

                    messenger.Send(message);

                    return subtotal;
                },
                (x) => Interlocked.Add(ref total, x)
            );
            sw.Stop();
            long totalMilliseconds = sw.ElapsedMilliseconds;

            Assert.Equal(expectedResult, total);
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

            public Guid MessageId { get; set; }

            public int Payload { get; set; }

            public Action Update { get; set; }
        }

        private class OtherMessage : IShipWorksMessage
        {
            public object Sender { get; private set; }

            public Guid MessageId { get; set; }
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
