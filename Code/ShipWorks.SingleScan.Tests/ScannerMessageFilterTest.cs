using System;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using log4net;
using Moq;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class ScannerMessageFilterTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly RegisteredScannerInputHandler testObject;

        public ScannerMessageFilterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Mock<IScannerIdentifier>().Setup(x => x.IsRegisteredScanner(It.IsAny<IntPtr>())).Returns(true);
            testObject = mock.Create<RegisteredScannerInputHandler>();
        }

        [Fact]
        public void PreFilterMessage_ReturnsFalse_WhenMessageIsNotInput()
        {
            var message = new Message();
            var result = testObject.PreFilterMessage(ref message);
            Assert.False(result);
        }

        [Fact]
        public void PreFilterMessage_DelegatesToScannerIdentifier()
        {
            var message = new Message { Msg = (int) WindowsMessage.INPUT, LParam = new IntPtr(456) };
            testObject.PreFilterMessage(ref message);

            mock.Mock<IScannerIdentifier>().Verify(x => x.IsRegisteredScanner(It.IsAny<IntPtr>()));
        }

        [Fact]
        public void PreFilterMessage_ReturnsFalse_WhenInputIsNotScanner()
        {
            mock.Mock<IScannerIdentifier>().Setup(x => x.IsRegisteredScanner(It.IsAny<IntPtr>())).Returns(false);

            var message = new Message { Msg = (int) WindowsMessage.INPUT };
            var result = testObject.PreFilterMessage(ref message);

            Assert.False(result);
        }

        [Fact]
        public void PreFilterMessage_DelegatesToUser32Input_ToGetData()
        {
            var handle = new IntPtr(123);
            var message = new Message { Msg = (int) WindowsMessage.INPUT, LParam = handle };
            testObject.PreFilterMessage(ref message);
            mock.Mock<IUser32Input>().Verify(x => x.GetRawInputData(handle, RawInputCommand.Input));
        }

        [Fact]
        public void PreFilterMessage_LogsErrorMessage_WhenDataCouldNotBeRetrieved()
        {
            mock.Mock<IUser32Input>()
                .Setup(x => x.GetRawInputData(It.IsAny<IntPtr>(), It.IsAny<RawInputCommand>()))
                .Returns(GenericResult.FromError<RawInput>("Foo error"));

            var message = new Message { Msg = (int) WindowsMessage.INPUT };
            var result = testObject.PreFilterMessage(ref message);

            Assert.False(result);
            mock.Mock<ILog>().Verify(x => x.Error("Foo error"));
        }

        [Fact]
        public void PreFilterMessage_DelegatesToUser32Input_ToGetCharacters()
        {
            mock.Mock<IUser32Input>()
                .Setup(x => x.GetRawInputData(It.IsAny<IntPtr>(), It.IsAny<RawInputCommand>()))
                .Returns(CreateRawInputData(WindowsMessage.KEYFIRST, VirtualKeys.X));

            var message = new Message { Msg = (int) WindowsMessage.INPUT };
            testObject.PreFilterMessage(ref message);

            mock.Mock<IUser32Input>().Verify(x => x.GetCharactersFromKeys(VirtualKeys.X, false, false));
        }

        [Fact]
        public void PreFilterMessage_DelegatesToUser32Input_ToGetCharactersWhenShiftIsHeld()
        {
            mock.Mock<IUser32Input>()
                .SetupSequence(x => x.GetRawInputData(It.IsAny<IntPtr>(), It.IsAny<RawInputCommand>()))
                .Returns(CreateRawInputData(WindowsMessage.KEYFIRST, VirtualKeys.Shift))
                .Returns(CreateRawInputData(WindowsMessage.KEYFIRST, VirtualKeys.X));

            var message = new Message { Msg = (int) WindowsMessage.INPUT };
            testObject.PreFilterMessage(ref message);
            testObject.PreFilterMessage(ref message);

            mock.Mock<IUser32Input>().Verify(x => x.GetCharactersFromKeys(VirtualKeys.X, true, false));
        }

        [Fact]
        public void PreFilterMessage_DelegatesToScanBuffer_WithCharacters()
        {
            mock.Mock<IUser32Input>()
                .Setup(x => x.GetRawInputData(It.IsAny<IntPtr>(), It.IsAny<RawInputCommand>()))
                .Returns(CreateRawInputData(WindowsMessage.KEYFIRST, VirtualKeys.X));
            mock.Mock<IUser32Input>()
                .Setup(x => x.GetCharactersFromKeys(It.IsAny<VirtualKeys>(), It.IsAny<bool>(), false))
                .Returns("X");

            var message = new Message { Msg = (int) WindowsMessage.INPUT };
            testObject.PreFilterMessage(ref message);

            mock.Mock<IScanBuffer>().Verify(x => x.Append(It.IsAny<IntPtr>(), "X"));
        }

        [Theory]
        [InlineData(WindowsMessage.KEYFIRST)]
        [InlineData(WindowsMessage.KEYUP)]
        [InlineData(WindowsMessage.CHAR)]
        [InlineData(WindowsMessage.DEADCHAR)]
        [InlineData(WindowsMessage.SYSKEYDOWN)]
        [InlineData(WindowsMessage.SYSKEYUP)]
        [InlineData(WindowsMessage.SYSCHAR)]
        [InlineData(WindowsMessage.SYSDEADCHAR)]
        [InlineData(WindowsMessage.KEYLAST)]
        public void PreFilterMessage_ReturnsFalse_WhenScannerKeyIsDownAndDifferentKeyboardKeyIsPressed(WindowsMessage windowsMessage)
        {
            mock.Mock<IUser32Input>()
                .Setup(x => x.GetRawInputData(It.IsAny<IntPtr>(), It.IsAny<RawInputCommand>()))
                .Returns(CreateRawInputData(WindowsMessage.KEYFIRST, VirtualKeys.X));

            var inputMessage = new Message { Msg = (int) WindowsMessage.INPUT };
            testObject.PreFilterMessage(ref inputMessage);

            var message = new Message { Msg = (int) windowsMessage, WParam = new IntPtr((int) VirtualKeys.B) };
            var result = testObject.PreFilterMessage(ref message);

            Assert.False(result);
        }

        [Theory]
        [InlineData(WindowsMessage.KEYFIRST)]
        [InlineData(WindowsMessage.KEYUP)]
        [InlineData(WindowsMessage.CHAR)]
        [InlineData(WindowsMessage.DEADCHAR)]
        [InlineData(WindowsMessage.SYSKEYDOWN)]
        [InlineData(WindowsMessage.SYSKEYUP)]
        [InlineData(WindowsMessage.SYSCHAR)]
        [InlineData(WindowsMessage.SYSDEADCHAR)]
        [InlineData(WindowsMessage.KEYLAST)]
        public void PreFilterMessage_ReturnsTrue_WhenScannerKeyIsDownAndSameKeyboardKeyIsPressed(WindowsMessage windowsMessage)
        {
            mock.Mock<IUser32Input>()
                .Setup(x => x.GetRawInputData(It.IsAny<IntPtr>(), It.IsAny<RawInputCommand>()))
                .Returns(CreateRawInputData(WindowsMessage.KEYFIRST, VirtualKeys.X));

            var inputMessage = new Message { Msg = (int) WindowsMessage.INPUT };
            testObject.PreFilterMessage(ref inputMessage);

            var message = new Message { Msg = (int) windowsMessage, WParam = new IntPtr((int) VirtualKeys.X) };
            var result = testObject.PreFilterMessage(ref message);

            Assert.True(result);
        }

        [Theory]
        [InlineData(WindowsMessage.KEYFIRST)]
        [InlineData(WindowsMessage.KEYUP)]
        [InlineData(WindowsMessage.CHAR)]
        [InlineData(WindowsMessage.DEADCHAR)]
        [InlineData(WindowsMessage.SYSKEYDOWN)]
        [InlineData(WindowsMessage.SYSKEYUP)]
        [InlineData(WindowsMessage.SYSCHAR)]
        [InlineData(WindowsMessage.SYSDEADCHAR)]
        [InlineData(WindowsMessage.KEYLAST)]
        public void PreFilterMessage_ReturnsFalse_WhenScannerKeysWerePressedAndReleased(WindowsMessage windowsMessage)
        {
            mock.Mock<IUser32Input>()
                .SetupSequence(x => x.GetRawInputData(It.IsAny<IntPtr>(), It.IsAny<RawInputCommand>()))
                .Returns(CreateRawInputData(WindowsMessage.KEYFIRST, VirtualKeys.Shift))
                .Returns(CreateRawInputData(WindowsMessage.KEYFIRST, VirtualKeys.X))
                .Returns(CreateRawInputData(WindowsMessage.KEYLAST, VirtualKeys.Shift))
                .Returns(CreateRawInputData(WindowsMessage.KEYLAST, VirtualKeys.X));

            var inputMessage = new Message { Msg = (int) WindowsMessage.INPUT };
            testObject.PreFilterMessage(ref inputMessage);
            testObject.PreFilterMessage(ref inputMessage);
            testObject.PreFilterMessage(ref inputMessage);
            testObject.PreFilterMessage(ref inputMessage);

            var message = new Message { Msg = (int) windowsMessage, WParam = new IntPtr((int) VirtualKeys.X) };
            var result = testObject.PreFilterMessage(ref message);

            Assert.False(result);
        }

        [Fact]
        public void PreFilterMessage_DelegatesToScannerIdentifierAdded_WhenInputChangeIsRecieved()
        {
            var message = new Message
            {
                Msg = (int) WindowsMessage.INPUT_DEVICE_CHANGE,
                WParam = (IntPtr) 1,
                LParam = (IntPtr) 1234,
            };

            testObject.PreFilterMessage(ref message);

            mock.Mock<IScannerIdentifier>().Verify(x => x.HandleDeviceAdded((IntPtr) 1234));
            mock.Mock<IScannerIdentifier>().Verify(x => x.HandleDeviceRemoved((IntPtr) 1234), Times.Never);
        }

        [Fact]
        public void PreFilterMessage_DelegatesToScannerIdentifierRemoved_WhenInputChangeIsRecieved()
        {
            var message = new Message
            {
                Msg = (int) WindowsMessage.INPUT_DEVICE_CHANGE,
                WParam = (IntPtr) 0,
                LParam = (IntPtr) 1234,
            };

            testObject.PreFilterMessage(ref message);

            mock.Mock<IScannerIdentifier>().Verify(x => x.HandleDeviceRemoved((IntPtr) 1234));
            mock.Mock<IScannerIdentifier>().Verify(x => x.HandleDeviceAdded((IntPtr) 1234), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void PreFilterMessage_ReturnsFalse_WhenInputChangeIsRecieved(int input)
        {
            var message = new Message
            {
                Msg = (int) WindowsMessage.INPUT_DEVICE_CHANGE,
                WParam = (IntPtr) input,
                LParam = (IntPtr) 1234,
            };

            var result = testObject.PreFilterMessage(ref message);

            Assert.False(result);
        }

        private GenericResult<RawInput> CreateRawInputData(WindowsMessage message, VirtualKeys key)
        {
            return GenericResult.FromSuccess(new RawInput
            {
                Header = new RawInputHeader
                {
                    DeviceHandle = new IntPtr(456),
                    Type = RawInputDeviceType.Keyboard
                },
                Data = new RawInput.Union
                {
                    Keyboard = new RawInputKeyboard
                    {
                        Message = message,
                        VirtualKey = key
                    }
                }
            });
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
