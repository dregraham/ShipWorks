using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using ShipWorks.Tests.Shared;
using Moq;
using Xunit;

namespace Interapptive.Shared.Tests.Collections
{
    public class ObservableExtensionsTest
    {
        readonly Subject<int> stream = new Subject<int>();
        readonly Subject<int> open = new Subject<int>();
        readonly Subject<int> close = new Subject<int>();

        [Fact]
        public void ContinueAfter_Continues_WhenOneDependentMessageReceivedAndDoesNotTimeout()
        {
            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            Subject<int> sourceSubect = new Subject<int>();
            Subject<int> dependentSubect = new Subject<int>();

            IObservable<int> testObject = sourceSubect.ContinueAfter(dependentSubect, TimeSpan.FromDays(50), testScheduler.WindowsFormsEventLoop);

            bool wasCalled = false;
            testObject.Subscribe(value => wasCalled = true);

            sourceSubect.OnNext(1);
            Assert.False(wasCalled);

            testScheduler.WindowsFormsEventLoop.AdvanceBy(TimeSpan.FromMilliseconds(1).Ticks);
            Assert.False(wasCalled);

            dependentSubect.OnNext(1);
            Assert.True(wasCalled);
        }

        [Fact]
        public void ContinueAfter_DoesNotContinue_WhenZeroDependentMessagesReceivedAndDoesNotTimeout()
        {
            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            Subject<int> sourceSubect = new Subject<int>();
            Subject<int> dependentSubect = new Subject<int>();

            IObservable<int> testObject = sourceSubect.ContinueAfter(dependentSubect, TimeSpan.FromDays(50), testScheduler.WindowsFormsEventLoop);

            bool wasCalled = false;
            testObject.Subscribe(value => wasCalled = true);

            sourceSubect.OnNext(1);
            Assert.False(wasCalled);

            testScheduler.WindowsFormsEventLoop.AdvanceBy(TimeSpan.FromMilliseconds(1).Ticks);
            Assert.False(wasCalled);
        }

        [Fact]
        public void ContinueAfter_ContinuesListeningToSource_WhenAnotherSourceMessageArrives_AndOneDependentMessageReceivedAndDoesNotTimeout()
        {
            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            Subject<int> sourceSubect = new Subject<int>();
            Subject<int> dependentSubect = new Subject<int>();

            IObservable<int> testObject = sourceSubect.ContinueAfter(dependentSubect, TimeSpan.FromDays(50), testScheduler.WindowsFormsEventLoop);

            bool wasCalled = false;
            testObject.Subscribe(value => wasCalled = true);

            sourceSubect.OnNext(1);
            Assert.False(wasCalled);

            testScheduler.WindowsFormsEventLoop.AdvanceBy(TimeSpan.FromMilliseconds(1).Ticks);
            Assert.False(wasCalled);

            dependentSubect.OnNext(1);
            Assert.True(wasCalled);

            wasCalled = false;
            sourceSubect.OnNext(1);
            Assert.False(wasCalled);

            testScheduler.WindowsFormsEventLoop.AdvanceBy(TimeSpan.FromMilliseconds(1).Ticks);
            Assert.False(wasCalled);

            dependentSubect.OnNext(1);
            Assert.True(wasCalled);
        }

        [Fact]
        public void ContinueAfter_Continues_WhenZeroDependentMessageReceivedAndDoesTimeout()
        {
            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            Subject<int> sourceSubect = new Subject<int>();
            Subject<int> dependentSubect = new Subject<int>();

            IObservable<int> testObject = sourceSubect.ContinueAfter(dependentSubect, TimeSpan.FromDays(50), testScheduler.WindowsFormsEventLoop);

            bool wasCalled = false;
            testObject.Subscribe(value => wasCalled = true);

            sourceSubect.OnNext(1);
            Assert.False(wasCalled);

            testScheduler.WindowsFormsEventLoop.AdvanceBy(TimeSpan.FromDays(49).Ticks);
            Assert.False(wasCalled);

            testScheduler.WindowsFormsEventLoop.AdvanceBy(TimeSpan.FromDays(51).Ticks);
            Assert.True(wasCalled);
        }
        
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
