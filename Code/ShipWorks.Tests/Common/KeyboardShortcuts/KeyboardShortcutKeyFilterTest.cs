using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Win32.Native;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Core.Messaging;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Common.KeyboardShortcuts
{
    public class KeyboardShortcutKeyFilterTest : IDisposable
    {
        readonly AutoMock mock;

        public KeyboardShortcutKeyFilterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(VirtualKeys.W)]
        [InlineData(VirtualKeys.Control, VirtualKeys.W)]
        [InlineData(VirtualKeys.Control, VirtualKeys.Menu, VirtualKeys.W)]
        public void PreFilterMessage_DelegatesKeysToTranslator(params VirtualKeys[] keys)
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            foreach (var key in keys)
            {
                SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, key);
            }

            mock.Mock<IKeyboardShortcutTranslator>()
                .Verify(x => x.GetCommands(It.Is((IEnumerable<VirtualKeys> k) => keys.UnorderedSequenceEquals(k))));
        }

        [Theory]
        [InlineData(VirtualKeys.LeftShift, VirtualKeys.W, VirtualKeys.Shift)]
        [InlineData(VirtualKeys.RightShift, VirtualKeys.W, VirtualKeys.Shift)]
        [InlineData(VirtualKeys.LeftMenu, VirtualKeys.W, VirtualKeys.Menu)]
        [InlineData(VirtualKeys.RightMenu, VirtualKeys.W, VirtualKeys.Menu)]
        [InlineData(VirtualKeys.LeftControl, VirtualKeys.W, VirtualKeys.Control)]
        [InlineData(VirtualKeys.RightControl, VirtualKeys.W, VirtualKeys.Control)]
        public void PreFilterMessage_DelegatesKeysToTranslator_WithNonSpecificCommandKey(
            VirtualKeys commandKey, VirtualKeys actionKey, VirtualKeys expectedCommand)
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, commandKey);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, actionKey);

            mock.Mock<IKeyboardShortcutTranslator>()
                .Verify(x => x.GetCommands(It.Is((IEnumerable<VirtualKeys> k) => k.Contains(expectedCommand))));
        }

        [Fact]
        public void PreFilterMessage_DelegatesKeysToTranslator_WithoutReleasedKey()
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.Control);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.Menu);
            SendKeyboardMessage(testObject, WindowsMessage.KEYLAST, VirtualKeys.Menu);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W);

            mock.Mock<IKeyboardShortcutTranslator>()
                .Verify(x => x.GetCommands(It.Is((IEnumerable<VirtualKeys> k) =>
                new[] { VirtualKeys.Control, VirtualKeys.W }.UnorderedSequenceEquals(k))));
        }

        [Fact]
        public void PreFilterMessage_DelegatesKeysToTranslator_WithoutEachActionKey()
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.Control);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.A);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W);

            mock.Mock<IKeyboardShortcutTranslator>()
                .Verify(x => x.GetCommands(It.Is((IEnumerable<VirtualKeys> k) =>
                new[] { VirtualKeys.Control, VirtualKeys.A }.UnorderedSequenceEquals(k))));

            mock.Mock<IKeyboardShortcutTranslator>()
                .Verify(x => x.GetCommands(It.Is((IEnumerable<VirtualKeys> k) =>
                new[] { VirtualKeys.Control, VirtualKeys.W }.UnorderedSequenceEquals(k))));
        }

        [Fact]
        public void PreFilterMessage_DelegatesKeysToTranslatorTwice_WithSameKeyBackToBack()
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.Control);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W);
            SendKeyboardMessage(testObject, WindowsMessage.KEYLAST, VirtualKeys.W);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W);

            mock.Mock<IKeyboardShortcutTranslator>()
                .Verify(x => x.GetCommands(It.Is((IEnumerable<VirtualKeys> k) =>
                    new[] { VirtualKeys.Control, VirtualKeys.W }.UnorderedSequenceEquals(k))),
                Times.Exactly(2));
        }

        [Fact]
        public void PreFilterMessage_SendsMessage_WhenCommandsAreReturned()
        {
            var message = mock.Create<IShipWorksMessage>();

            mock.Mock<IKeyboardShortcutTranslator>().Setup(x => x.GetCommands(It.IsAny<IEnumerable<VirtualKeys>>()))
                .Returns(new Func<object, IShipWorksMessage>[] { x => message });

            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W);

            mock.Mock<IMessenger>().Verify(x => x.Send(message, It.IsAny<string>()));
        }

        [Fact]
        public void PreFilterMessage_ReturnsTrue_WhenKeyTriggersCommand()
        {
            mock.Mock<IKeyboardShortcutTranslator>().Setup(x => x.GetCommands(It.IsAny<IEnumerable<VirtualKeys>>()))
                .Returns(new Func<object, IShipWorksMessage>[] { x => mock.Create<IShipWorksMessage>() });

            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            Assert.True(SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W));
            Assert.True(SendKeyboardMessage(testObject, WindowsMessage.CHAR, VirtualKeys.W));
        }

        [Fact]
        public void PreFilterMessage_ReturnsFalse_WhenKeyDoesNotTriggerCommand()
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            Assert.False(SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W));
        }

        /// <summary>
        /// Send a keyboard message to the test object
        /// </summary>
        private static bool SendKeyboardMessage(KeyboardShortcutKeyFilter testObject, WindowsMessage windowsMessage, VirtualKeys key)
        {
            var message = new Message { Msg = (int) windowsMessage, WParam = (IntPtr) key };
            return testObject.PreFilterMessage(ref message);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
