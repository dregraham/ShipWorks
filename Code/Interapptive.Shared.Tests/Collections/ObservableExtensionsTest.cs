using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Interapptive.Shared.Collections;
using Xunit;

namespace Interapptive.Shared.Tests.Collections
{
    public class ObservableExtensionsTest
    {
        Subject<int> stream = new Subject<int>();
        Subject<int> open = new Subject<int>();
        Subject<int> close = new Subject<int>();

        [Fact]
        public void IgnoreBetweenMessages_IgnoresMessages_BeforeWindowIsOpen()
        {
            Observable.Return(1)
                .IgnoreBetweenMessages(close, open)
                .Subscribe(_ => Assert.False(true, "This should not have been called"));
        }

        [Fact]
        public void IgnoreBetweenMessages_AllowsMessages_WhenWindowIsOpenedBeforeTheClosedMessage()
        {
            int value = 0;

            stream.IgnoreBetweenMessages(close, open)
                .Subscribe(x => value = x);

            open.OnNext(3);
            stream.OnNext(1);

            Assert.Equal(1, value);
        }

        [Fact]
        public void IgnoreBetweenMessages_AllowsMessages_WhenWindowIsOpenedBeforeTheClosedMessageUsingReferenceType()
        {
            int value = 0;

            var newClose = new Subject<object>();
            var newOpen = new Subject<object>();

            stream.IgnoreBetweenMessages(newClose, newOpen)
                .Subscribe(x => value = x);

            newOpen.OnNext(3);
            stream.OnNext(1);

            Assert.Equal(1, value);
        }

        [Fact]
        public void IgnoreBetweenMessages_IgnoresMessages_WhenWindowIsOpenedThenClosed()
        {
            stream.IgnoreBetweenMessages(close, open)
                .Subscribe(_ => Assert.False(true, "This should not have been called"));

            open.OnNext(3);
            close.OnNext(3);
            stream.OnNext(1);
        }

        [Fact]
        public void IgnoreBetweenMessages_AllowsMessages_WhenWindowIsOpenedThenClosedThenOpened()
        {
            int value = 0;

            stream.IgnoreBetweenMessages(close, open)
                .Subscribe(x => value = x);

            open.OnNext(3);
            close.OnNext(3);
            open.OnNext(3);
            stream.OnNext(1);

            Assert.Equal(1, value);
        }

        [Fact]
        public void IgnoreBetweenMessages_SendsSingleMessage_WhenWindowIsOpenedMultipleTimes()
        {
            int value = 0;

            stream.IgnoreBetweenMessages(close, open)
                .Subscribe(_ => value += 1);

            open.OnNext(3);
            open.OnNext(3);
            open.OnNext(3);
            stream.OnNext(1);

            Assert.Equal(1, value);
        }

        [Fact]
        public void IgnoreBetweenMessages_SendsSingleMessage_WhenWindowIsClosedAndOpenedMultipleTimes()
        {
            int value = 0;

            stream.IgnoreBetweenMessages(close, open)
                .Subscribe(_ => value += 1);

            close.OnNext(3);
            close.OnNext(3);
            close.OnNext(3);
            open.OnNext(3);
            open.OnNext(3);
            open.OnNext(3);
            stream.OnNext(1);

            Assert.Equal(1, value);
        }

        [Fact]
        public void IgnoreBetweenMessages_SendsSingleMessage_WhenWindowIsClosedAndOpenedMultipleTimesInSequence()
        {
            int value = 0;

            stream.IgnoreBetweenMessages(close, open)
                .Subscribe(_ => value += 1);

            open.OnNext(3);
            close.OnNext(3);
            open.OnNext(3);
            close.OnNext(3);
            open.OnNext(3);

            stream.OnNext(1);

            Assert.Equal(1, value);
        }
    }
}
