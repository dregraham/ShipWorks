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

        [Theory]
        [InlineData(KeyboardShortcutCommand.ApplyWeight, "Ctrl+W")]
        public void GetShortcuts_ReturnsText_ForDefaultShortcuts(KeyboardShortcutCommand command, string expected)
        {
            var testObject = mock.Create<KeyboardShortcutTranslator>();
            var shortcut = testObject.GetShortcuts(command).Shortcuts.Single();
            Assert.Equal(expected, shortcut);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
