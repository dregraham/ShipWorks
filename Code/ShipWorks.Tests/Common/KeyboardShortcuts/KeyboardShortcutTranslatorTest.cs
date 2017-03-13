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
        public void GetCommand_ReturnsCommands_ForDefaultShortcuts(
            KeyboardShortcutCommand expected, VirtualKeys actionKey, KeyboardShortcutModifiers modifiers)
        {
            var testObject = mock.Create<KeyboardShortcutTranslator>();
            var command = testObject.GetCommand(actionKey, modifiers);
            Assert.Equal(expected, command);
        }

        [Fact]
        public void GetCommand_ReturnsApplyWeightCommand_ForCtrlW()
        {
            var testObject = mock.Create<KeyboardShortcutTranslator>();
            var command = testObject.GetCommand(VirtualKeys.W, Ctrl);
            Assert.Equal(KeyboardShortcutCommand.ApplyWeight, command);
        }

        [Theory]
        [InlineData(Ctrl | Alt)]
        [InlineData(Ctrl | Alt | Shift)]
        [InlineData(Alt)]
        [InlineData(None)]
        public void GetCommand_ReturnsEmptyCollection_WhenModifierIsNotExact(KeyboardShortcutModifiers modifiers)
        {
            var testObject = mock.Create<KeyboardShortcutTranslator>();
            var command = testObject.GetCommand(VirtualKeys.W, modifiers);
            Assert.Null(command);
        }

        [Theory]
        [InlineData(KeyboardShortcutCommand.ApplyWeight, "Ctrl+W")]
        public void GetShortcut_ReturnsText_ForDefaultShortcuts(KeyboardShortcutCommand command, string expected)
        {
            var testObject = mock.Create<KeyboardShortcutTranslator>();
            var shortcut = testObject.GetShortcut(command);
            Assert.Equal(expected, shortcut);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
