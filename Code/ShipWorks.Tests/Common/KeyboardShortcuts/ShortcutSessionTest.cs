using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Common;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Common.KeyboardShortcuts
{
    public class ShortcutSessionTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly KeyboardShortcutKeyFilter keyboardShortcutKeyFilter;
        
        public ShortcutSessionTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            
            keyboardShortcutKeyFilter = mock.Create<KeyboardShortcutKeyFilter>();
            
            Mock<Func<KeyboardShortcutKeyFilter>> mockFuncKeyboardShortcutKeyFilter =
                mock.MockRepository.Create<Func<KeyboardShortcutKeyFilter>>();
            mockFuncKeyboardShortcutKeyFilter.Setup(func => func()).Returns(keyboardShortcutKeyFilter);
            mock.Provide(mockFuncKeyboardShortcutKeyFilter.Object);
        }

        [Fact]
        public void EndSession_AddsMessageFilter()
        {
            ShortcutSession shortcutSession = mock.Create<ShortcutSession>();
            shortcutSession.InitializeForCurrentSession();

            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(r => r.AddMessageFilter(keyboardShortcutKeyFilter), Times.Once);
        }

        [Fact]
        public void InitializeForCurrentSession_RemovesMessageFilter()
        {
            mock.Mock<IMainForm>().SetupGet(m => m.InvokeRequired).Returns(false);

            ShortcutSession shortcutSession = mock.Create<ShortcutSession>();
            shortcutSession.EndSession();

            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(r => r.RemoveMessageFilter(keyboardShortcutKeyFilter), Times.Once);
        }
        
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}