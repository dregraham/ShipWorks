using System;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Win32.Native;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.IO.KeyboardShortcuts.KeyboardShortcutModifiers;

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
        [InlineData(None, VirtualKeys.W)]
        [InlineData(Ctrl, VirtualKeys.W, VirtualKeys.Control)]
        [InlineData(Ctrl | Alt, VirtualKeys.W, VirtualKeys.Control, VirtualKeys.Menu)]
        public void PreFilterMessage_DelegatesKeysToShortcutManager(KeyboardShortcutModifiers expectedModifiers,
            VirtualKeys actionKey, params VirtualKeys[] keys)
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            foreach (var key in keys)
            {
                SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, key);
            }

            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, actionKey);

            mock.Mock<IShortcutManager>()
                .Verify(x => x.GetShortcut(actionKey, expectedModifiers));
        }

        [Fact]
        public void PreFilterMessage_DelegatesKeysToShortcutManager_WithoutReleasedKey()
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.Control);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.Menu);
            SendKeyboardMessage(testObject, WindowsMessage.KEYLAST, VirtualKeys.Menu);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W);

            mock.Mock<IShortcutManager>()
                .Verify(x => x.GetShortcut(VirtualKeys.W, Ctrl));
        }

        [Fact]
        public void PreFilterMessage_DelegatesKeysToShortcutManager_WithoutEachActionKey()
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.Control);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.A);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W);

            mock.Mock<IShortcutManager>()
                .Verify(x => x.GetShortcut(VirtualKeys.A, Ctrl));

            mock.Mock<IShortcutManager>()
                .Verify(x => x.GetShortcut(VirtualKeys.W, Ctrl));
        }

        [Fact]
        public void PreFilterMessage_DelegatesKeysToShortcutManagerTwice_WithSameKeyBackToBack()
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.Control);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W);
            SendKeyboardMessage(testObject, WindowsMessage.KEYLAST, VirtualKeys.W);
            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W);

            mock.Mock<IShortcutManager>()
                .Verify(x => x.GetShortcut(VirtualKeys.W, Ctrl), Times.Exactly(2));
        }

        [Fact]
        public void PreFilterMessage_SendsMessage_WhenCommandsAreReturned()
        {
            mock.Mock<IShortcutManager>().Setup(x => x.GetShortcut(It.IsAny<VirtualKeys>(), It.IsAny<KeyboardShortcutModifiers>()))
                .Returns(new ShortcutEntity(){Action = KeyboardShortcutCommand.FocusQuickSearch});
 
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W);

            mock.Mock<IMessenger>().Verify(x =>
                x.Send(It.Is<KeyboardShortcutMessage>(m => m.AppliesTo(KeyboardShortcutCommand.FocusQuickSearch)), It.IsAny<string>()));
        }

        [Fact]
        public void PreFilterMessage_ReturnsTrue_WhenKeyTriggersCommand()
        {
            mock.Mock<IShortcutManager>()
                .Setup(x => x.GetShortcut(It.IsAny<VirtualKeys>(), It.IsAny<KeyboardShortcutModifiers>()))
                .Returns(new ShortcutEntity() {Action = KeyboardShortcutCommand.ApplyWeight});

            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            Assert.True(SendKeyboardMessage(testObject, WindowsMessage.KEYFIRST, VirtualKeys.W));
            Assert.True(SendKeyboardMessage(testObject, WindowsMessage.CHAR, VirtualKeys.W));
        }

        [Fact]
        public void PreFilterMessage_ReturnsFalse_WhenKeyDoesNotTriggerCommand()
        {
            var testObject = mock.Create<KeyboardShortcutKeyFilter>();

            mock.Mock<IShortcutManager>().Setup(x => x.GetShortcut(VirtualKeys.W, None)).Returns((ShortcutEntity) null);
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
