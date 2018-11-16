using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.OrderLookup.ShipmentModelPipelines;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class CreateLabelShortcutPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestScheduler scheduler;
        private readonly TestMessenger testMessenger;
        private readonly CreateLabelShortcutPipeline testObject;

        public CreateLabelShortcutPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();
            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            testObject = mock.Create<CreateLabelShortcutPipeline>();
            testObject.Register(mock.Mock<IOrderLookupShipmentModel>().Object);
        }

        [Fact]
        public void Register_DoesNotCallCreateLabel_WhenMessageIsNotShortcut()
        {
            testMessenger.Send(mock.Build<IShipWorksMessage>());

            scheduler.Start();

            mock.Mock<IOrderLookupShipmentModel>().Verify(x => x.CreateLabel(), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallCreateLabel_WhenShortcutIsNotCreateLabel()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyProfile },
                ShortcutTriggerType.Barcode, "-PL-");

            testMessenger.Send(message);

            scheduler.Start();

            mock.Mock<IOrderLookupShipmentModel>().Verify(x => x.CreateLabel(), Times.Never);
        }

        [Fact]
        public void Register_CallsCreateLabel_WhenShortcutIsCreateLabel()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.CreateLabel },
                ShortcutTriggerType.Barcode, "-PL-");

            testMessenger.Send(message);

            scheduler.Start();

            mock.Mock<IOrderLookupShipmentModel>().Verify(x => x.CreateLabel());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}