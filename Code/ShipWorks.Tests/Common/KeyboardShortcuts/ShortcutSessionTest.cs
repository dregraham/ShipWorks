using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Common;
using ShipWorks.Common.IO.KeyboardShortcuts;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Common.KeyboardShortcuts
{
    public class ShortcutSessionTest : IDisposable
    {
        private readonly AutoMock mock;

        public ShortcutSessionTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void InitializeForCurrentSession_RemovesMessageFilter()
        {
            mock.Mock<IMainForm>().SetupGet(m => m.InvokeRequired).Returns(false);

            KeyboardShortcutKeyFilter keyboardShortcutKeyFilter = mock.Create<KeyboardShortcutKeyFilter>();
            
            var mockFuncKeyboardShortcutKeyFilter = mock.MockRepository.Create<Func<KeyboardShortcutKeyFilter>>();
            mockFuncKeyboardShortcutKeyFilter.Setup(func => func()).Returns(keyboardShortcutKeyFilter);
            mock.Provide(mockFuncKeyboardShortcutKeyFilter.Object);
            
            ShortcutSession shortcutSession = mock.Create<ShortcutSession>();
            shortcutSession.EndSession();
            
            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(r=>r.RemoveMessageFilter(keyboardShortcutKeyFilter), Times.Once);
        }
        
        [Fact]
        public void EndSession_AddsMessageFilter()
        {
            KeyboardShortcutKeyFilter keyboardShortcutKeyFilter = mock.Create<KeyboardShortcutKeyFilter>();
            
            var mockFuncKeyboardShortcutKeyFilter = mock.MockRepository.Create<Func<KeyboardShortcutKeyFilter>>();
            mockFuncKeyboardShortcutKeyFilter.Setup(func => func()).Returns(keyboardShortcutKeyFilter);
            mock.Provide(mockFuncKeyboardShortcutKeyFilter.Object);
            
            ShortcutSession shortcutSession = mock.Create<ShortcutSession>();
            shortcutSession.InitializeForCurrentSession();
            
            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(r=>r.AddMessageFilter(keyboardShortcutKeyFilter), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}