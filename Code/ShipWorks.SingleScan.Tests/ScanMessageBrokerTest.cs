using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class ScanMessageBrokerTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly TestScheduler scheduler;
        private readonly Mock<IShortcutManager> shortcutManager;
        private readonly ScanMessageBroker testObject;

        public ScanMessageBrokerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);

            shortcutManager = mock.Mock<IShortcutManager>();

            testObject = mock.Create<ScanMessageBroker>();
        }

        [Fact]
        public void HandleScanMessage_DelegatesToShortcutManagerForShortcut()
        {
            testObject.InitializeForCurrentSession();
            testMessenger.Send(new ScanMessage(this, "blah", IntPtr.Zero));
            scheduler.Start();

            shortcutManager.VerifyGet(m => m.Shortcuts);
        }

        [Fact]
        public void HandleScanMessage_SendsShortcutMessage_WhenScanMessageTextMatchesShortcutBarcode()
        {
            shortcutManager.SetupGet(m => m.Shortcuts).Returns(new[] { new ShortcutEntity(), new ShortcutEntity() { Barcode = "blah"} });
            testObject.InitializeForCurrentSession();
            testMessenger.Send(new ScanMessage(this, "blah", IntPtr.Zero));
            scheduler.Start();

            ShortcutMessage sentMessage = (ShortcutMessage) testMessenger.SentMessages.Single(m => m.GetType() == typeof(ShortcutMessage));

            Assert.Equal("blah", sentMessage.Shortcut.Barcode);
        }

        [Fact]
        public void HandleScanMessage_SendsSingleScanMessage_WhenScanMessageTextDoesNotMatchShortcutBarcode()
        {
            shortcutManager.SetupGet(m => m.Shortcuts).Returns(new[] { new ShortcutEntity(), new ShortcutEntity() { Barcode = "nope" } });
            testObject.InitializeForCurrentSession();
            testMessenger.Send(new ScanMessage(this, "blah", IntPtr.Zero));
            scheduler.Start();

            SingleScanMessage sentMessage = (SingleScanMessage) testMessenger.SentMessages.Single(m => m.GetType() == typeof(SingleScanMessage));

            Assert.Equal("blah", sentMessage.ScannedText);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
