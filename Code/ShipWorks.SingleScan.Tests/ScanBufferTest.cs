using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    [SuppressMessage("Code Analysis", "S3236",
        Justification = "Moq method matching will not work if we do not explicitly pass caller member info params")]
    public class ScanBufferTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<IMessenger> messenger;
        private readonly IntPtr deviceHandle = (IntPtr) 42;

        public ScanBufferTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
            messenger = mock.Mock<IMessenger>();
        }

        [Fact]
        public void Append_DoesNotSendMessage_WhenBlankInput()
        {
            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(testScheduler);

            ScanBuffer testObject = mock.Create<ScanBuffer>();
            testObject.Append(deviceHandle, string.Empty);

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(101).Ticks);
            testScheduler.WindowsFormsEventLoop.Start();

            messenger.Verify(x => x.Send(It.IsAny<IShipWorksMessage>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Append_DoesNotSendMessage_WhenNullInput()
        {
            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(testScheduler);
            ScanBuffer testObject = mock.Create<ScanBuffer>();

            testObject.Append(deviceHandle, null);

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(101).Ticks);
            testScheduler.WindowsFormsEventLoop.Start();

            messenger.Verify(x => x.Send(It.IsAny<IShipWorksMessage>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Append_DoesSendMessage_WhenInputIsSingleCharacter()
        {
            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(testScheduler);

            ScanBuffer testObject = mock.Create<ScanBuffer>();

            testObject.Append(deviceHandle, "1");

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(101).Ticks);
            testScheduler.WindowsFormsEventLoop.Start();

            messenger.Verify(x => x.Send(It.IsAny<IShipWorksMessage>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Append_MessageBarcodeMatches_WhenInputIsSingleCharacter()
        {
            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(testScheduler);
            messenger.Setup(m => m.Send(It.Is<ScanMessage>(msg => msg.ScannedText == "1"), It.IsAny<string>())).Verifiable();
            ScanBuffer testObject = mock.Create<ScanBuffer>();

            testObject.Append(deviceHandle, "1");

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(101).Ticks);
            testScheduler.WindowsFormsEventLoop.Start();

            messenger.Verify(x => x.Send(It.Is<ScanMessage>(msg => msg.ScannedText == "1"), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Append_DoesSendMessage_WhenInputIsMultipleCharacters()
        {
            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(testScheduler);

            ScanBuffer testObject = mock.Create<ScanBuffer>();

            foreach (int i in Enumerable.Range(0, 25))
            {
                testObject.Append(deviceHandle, i.ToString());
            }

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(101).Ticks);
            testScheduler.WindowsFormsEventLoop.Start();

            messenger.Verify(x => x.Send(It.IsAny<IShipWorksMessage>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Append_SendsDeviceHandle()
        {
            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(testScheduler);

            ScanBuffer testObject = mock.Create<ScanBuffer>();

            foreach (int i in Enumerable.Range(0, 25))
            {
                testObject.Append(deviceHandle, i.ToString());
            }

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(101).Ticks);
            testScheduler.WindowsFormsEventLoop.Start();

            messenger.Verify(x => x.Send(It.Is<ScanMessage>(msg => msg.DeviceHandle == deviceHandle), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Append_MessageBarcodeMatches_WhenInputIsMultipleCharacters()
        {
            string barcode = "0123456789101112131415161718192021222324";

            TestSchedulerProvider testScheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(testScheduler);

            ScanBuffer testObject = mock.Create<ScanBuffer>();
            foreach (int i in Enumerable.Range(0, 25))
            {
                testObject.Append(deviceHandle, i.ToString());
            }

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(101).Ticks);
            testScheduler.WindowsFormsEventLoop.Start();

            messenger.Verify(x => x.Send(It.Is<ScanMessage>(msg => msg.ScannedText == barcode), It.IsAny<string>()), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
