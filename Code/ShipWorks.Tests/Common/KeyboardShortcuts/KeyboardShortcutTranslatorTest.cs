using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Win32.Native;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shared.IO.KeyboardShortcuts;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;
using static ShipWorks.Common.IO.KeyboardShortcuts.KeyboardShortcutModifiers;

namespace ShipWorks.Tests.Common.KeyboardShortcuts
{
    public class KeyboardShortcutTranslatorTest : IDisposable
    {
        readonly AutoMock mock;

        public KeyboardShortcutTranslatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(KeyboardShortcutCommand.ApplyWeight, VirtualKeys.W, Ctrl)]
        public void GetCommands_ReturnsCommands_ForDefaultShortcuts(
            KeyboardShortcutCommand expected, VirtualKeys actionKey, KeyboardShortcutModifiers modifiers)
        {
            var testObject = mock.Create<KeyboardShortcutTranslator>();
            var commands = testObject.GetCommands(actionKey, modifiers);
            var message = commands.Single();
            Assert.Equal(expected, message);
        }

        [Fact]
        public void GetCommands_ReturnsApplyWeightMessage_ForCtrlW()
        {
            var testObject = mock.Create<KeyboardShortcutTranslator>();
            var commands = testObject.GetCommands(VirtualKeys.W, Ctrl);
            var message = commands.Single();
            Assert.Equal(KeyboardShortcutCommand.ApplyWeight, message);
        }

        [Theory]
        [InlineData(Ctrl | Alt)]
        [InlineData(Ctrl | Alt | Shift)]
        [InlineData(Alt)]
        [InlineData(None)]
        public void GetCommands_ReturnsEmptyCollection_WhenModifierIsNotExact(KeyboardShortcutModifiers modifiers)
        {
            var testObject = mock.Create<KeyboardShortcutTranslator>();
            var commands = testObject.GetCommands(VirtualKeys.W, modifiers);
            Assert.Empty(commands);
        }

        [Fact]
        public void GetCommands_ReturnsApplyWeightMessage_WithOverriddenShortcut()
        {
            var user = new UserEntity();
            var shortcut = user.ShortcutOverrides.AddNew();
            shortcut.Alt = true;
            shortcut.Shift = true;
            shortcut.KeyValue = "J";
            shortcut.CommandType = KeyboardShortcutCommand.ApplyWeight;
            mock.Mock<IUserSession>().Setup(x => x.User).Returns(user);

            var testObject = mock.Create<KeyboardShortcutTranslator>();
            testObject.InitializeForCurrentSession();

            var commands = testObject.GetCommands(VirtualKeys.J, Alt | Shift);
            var message = commands.Single();
            Assert.Equal(KeyboardShortcutCommand.ApplyWeight, message);
        }

        [Fact]
        public void GetCommands_ReturnsEmptyCollection_WithOriginalShortcut()
        {
            var user = new UserEntity();
            var shortcut = user.ShortcutOverrides.AddNew();
            shortcut.Alt = true;
            shortcut.Shift = true;
            shortcut.KeyValue = "J";
            shortcut.CommandType = KeyboardShortcutCommand.ApplyWeight;
            mock.Mock<IUserSession>().Setup(x => x.User).Returns(user);

            var testObject = mock.Create<KeyboardShortcutTranslator>();
            testObject.InitializeForCurrentSession();

            var commands = testObject.GetCommands(VirtualKeys.W, Ctrl);
            Assert.Empty(commands);
        }

        [Fact]
        public void GetCommands_ReturnsMultipleCommands_WhenShortcutHasMultipleCommands()
        {
            var user = new UserEntity();

            var shortcut = user.ShortcutOverrides.AddNew();
            shortcut.KeyValue = "J";
            shortcut.CommandType = KeyboardShortcutCommand.ApplyWeight;

            var shortcut2 = user.ShortcutOverrides.AddNew();
            shortcut2.KeyValue = "J";
            shortcut2.CommandType = KeyboardShortcutCommand.FocusQuickSearch;

            mock.Mock<IUserSession>().Setup(x => x.User).Returns(user);

            var testObject = mock.Create<KeyboardShortcutTranslator>();
            testObject.InitializeForCurrentSession();

            var commands = testObject.GetCommands(VirtualKeys.J, None);
            Assert.Contains(commands, x => x == KeyboardShortcutCommand.ApplyWeight);
            Assert.Contains(commands, x => x == KeyboardShortcutCommand.FocusQuickSearch);
        }

        [Theory]
        [InlineData(KeyboardShortcutCommand.ApplyWeight, "Ctrl+W")]
        public void GetShortcuts_ReturnsText_ForDefaultShortcuts(KeyboardShortcutCommand command, string expected)
        {
            var testObject = mock.Create<KeyboardShortcutTranslator>();
            var shortcut = testObject.GetShortcuts(command).Shortcuts.Single();
            Assert.Equal(expected, shortcut);
        }

        [Fact]
        public void GetShortcuts_ReturnsMultipleShortcuts_WhenMultipleShortcutsExistForCommand()
        {
            var user = new UserEntity();

            var shortcut = user.ShortcutOverrides.AddNew();
            shortcut.KeyValue = "J";
            shortcut.CommandType = KeyboardShortcutCommand.ApplyWeight;

            var shortcut2 = user.ShortcutOverrides.AddNew();
            shortcut2.KeyValue = "H";
            shortcut2.CommandType = KeyboardShortcutCommand.ApplyWeight;

            mock.Mock<IUserSession>().Setup(x => x.User).Returns(user);

            var testObject = mock.Create<KeyboardShortcutTranslator>();
            testObject.InitializeForCurrentSession();

            var shortcuts = testObject.GetShortcuts(KeyboardShortcutCommand.ApplyWeight);
            Assert.Contains(shortcuts.Shortcuts, x => x == "J");
            Assert.Contains(shortcuts.Shortcuts, x => x == "H");
        }

        [Theory]
        [InlineData("K", None, "K")]
        [InlineData("K", Alt, "Alt+K")]
        [InlineData("K", Ctrl, "Ctrl+K")]
        [InlineData("K", Shift, "Shift+K")]
        [InlineData("K", Alt | Ctrl, "Ctrl+Alt+K")]
        [InlineData("K", Alt | Shift, "Alt+Shift+K")]
        [InlineData("K", Ctrl | Shift, "Ctrl+Shift+K")]
        [InlineData("K", Alt | Ctrl | Shift, "Ctrl+Alt+Shift+K")]
        [InlineData("F2", Ctrl, "Ctrl+F2")]
        public void GetShortcuts_DisplaysShortcutAsExptected_ForVariousCombinations(string actionKey,
            KeyboardShortcutModifiers modifiers, string expected)
        {
            var user = new UserEntity();

            var shortcut = user.ShortcutOverrides.AddNew();
            shortcut.Alt = modifiers.HasFlag(Alt);
            shortcut.Ctrl = modifiers.HasFlag(Ctrl);
            shortcut.Shift = modifiers.HasFlag(Shift);
            shortcut.KeyValue = actionKey;
            shortcut.CommandType = KeyboardShortcutCommand.ApplyWeight;

            mock.Mock<IUserSession>().Setup(x => x.User).Returns(user);

            var testObject = mock.Create<KeyboardShortcutTranslator>();
            testObject.InitializeForCurrentSession();

            var shortcutText = testObject.GetShortcuts(KeyboardShortcutCommand.ApplyWeight).Shortcuts.Single();
            Assert.Equal(expected, shortcutText);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
