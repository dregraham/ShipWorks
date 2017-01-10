using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    [SuppressMessage("Code Analysis", "S3236")]
    public class ScanBufferTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<IMessenger> messenger;
        private readonly ScanBuffer testObject;
        private readonly IntPtr deviceHandle = (IntPtr) 42;

        public ScanBufferTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
            messenger = mock.Mock<IMessenger>();

            testObject = mock.Create<ScanBuffer>();
        }

        [Fact]
        public void Append_DoesNotSendMessage_WhenBlankInput()
        {
            testObject.Append(deviceHandle, string.Empty);

            // Wait for the delay buffer to complete before checking for sent messages
            WaitForBufferComplete();

            messenger.Verify(x => x.Send(It.IsAny<IShipWorksMessage>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Append_DoesNotSendMessage_WhenNullInput()
        {
            testObject.Append(deviceHandle, null);

            // Wait for the delay buffer to complete before checking for sent messages
            WaitForBufferComplete();

            messenger.Verify(x => x.Send(It.IsAny<IShipWorksMessage>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Append_DoesSendMessage_WhenInputIsSingleCharacter()
        {
            testObject.Append(deviceHandle, "1");

            // Wait for the delay buffer to complete before checking for sent messages
            WaitForBufferComplete();

            messenger.Verify(x => x.Send(It.IsAny<IShipWorksMessage>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Append_MessageBarcodeMatches_WhenInputIsSingleCharacter()
        {
            messenger.Setup(m => m.Send(It.Is<ScanMessage>(msg => msg.ScannedText == "1"), It.IsAny<string>())).Verifiable();

            testObject.Append(deviceHandle, "1");

            // Wait for the delay buffer to complete before checking for sent messages
            WaitForBufferComplete();

            messenger.Verify(x => x.Send(It.Is<ScanMessage>(msg => msg.ScannedText == "1"), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Append_DoesSendMessage_WhenInputIsMultipleCharacters()
        {
            foreach (int i in Enumerable.Range(0, 25))
            {
                testObject.Append(deviceHandle, i.ToString());
            }

            // Wait for the delay buffer to complete before checking for sent messages
            WaitForBufferComplete();

            messenger.Verify(x => x.Send(It.IsAny<IShipWorksMessage>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Append_SendsDeviceHandle()
        {
            foreach (int i in Enumerable.Range(0, 25))
            {
                testObject.Append(deviceHandle, i.ToString());
            }

            // Wait for the delay buffer to complete before checking for sent messages
            WaitForBufferComplete();

            messenger.Verify(x => x.Send(It.Is<ScanMessage>(msg=>msg.DeviceHandle == deviceHandle), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Append_MessageBarcodeMatches_WhenInputIsMultipleCharacters()
        {
            string barcode = "0123456789101112131415161718192021222324";

            messenger.Setup(m => m.Send(It.Is<ScanMessage>(msg => msg.ScannedText == barcode), It.IsAny<string>())).Verifiable();

            foreach (int i in Enumerable.Range(0, 25))
            {
                testObject.Append(deviceHandle, i.ToString());
            }

            // Wait for the delay buffer to complete before checking for sent messages
            WaitForBufferComplete();

            messenger.Verify(x => x.Send(It.Is<ScanMessage>(msg => msg.ScannedText == barcode), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Wait for the delay buffer to complete before checking for sent messages
        /// </summary>
        private static void WaitForBufferComplete()
        {
            for (int i = 0; i < 15; i++)
            {
                Thread.Sleep(10);
            }
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
