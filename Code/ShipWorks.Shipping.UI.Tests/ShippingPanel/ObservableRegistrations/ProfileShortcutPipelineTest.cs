using System;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class ProfileShortcutPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly TestScheduler scheduler;
        private readonly ProfileShortcutPipeline testObject;
        private readonly Mock<IMainForm> mainForm;
        private readonly Mock<ShippingPanelViewModel> viewModel;

        public ProfileShortcutPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);

            viewModel = mock.CreateMock<ShippingPanelViewModel>();
            mainForm = mock.Mock<IMainForm>();

            testObject = mock.Create<ProfileShortcutPipeline>();
            testObject.Register(viewModel.Object);
        }

        [Fact]
        public void Register_SendsApplyProfileMessage_WhenShortcutMessageReceived_AndAppliesToProfile()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyProfile, RelatedObjectID = 789 },
                ShortcutTriggerType.Barcode, "123");
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456));
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Equal(1, testMessenger.SentMessages.OfType<ApplyProfileMessage>()
                .Count(m => m.ShipmentID == 456 &&
                            m.Sender == testObject &&
                            m.ProfileID == 789));
        }

        [Fact]
        public void Register_CallsMainFormFocus_WhenShortcutMessageReceived_AndAppliesToProfile()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyProfile, RelatedObjectID = 789 },
                ShortcutTriggerType.Barcode, "123");
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456));
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);

            testMessenger.Send(message);
            scheduler.Start();
            
            mainForm.Verify(f=>f.Focus(), Times.Once);
        }

        [Fact]
        public void Register_DoesNotSendMessage_WhenShortcutActionDoesNotApply()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyWeight, RelatedObjectID = 789 },
                ShortcutTriggerType.Barcode, "123");
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456));
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<ApplyProfileMessage>());
        }

        [Fact]
        public void Register_DoesNotSendApplyProfileMessage_ViewModelDoesNotHaveShipment()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyProfile, RelatedObjectID = 789 },
                ShortcutTriggerType.Barcode, "123");

            viewModel.SetupGet(v => v.Shipment).Returns((ShipmentEntity) null);

            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<ApplyProfileMessage>());
        }

        [Fact]
        public void Register_DoesNotSendApplyProfileMessage_WhenAdditionalFormsAreOpen()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyProfile, RelatedObjectID = 789 },
                ShortcutTriggerType.Barcode, "123");
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456));
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(true);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<ApplyProfileMessage>());
        }
        
        [Fact]
        public void Register_DoesNotSendApplyProfileMessage_WhenShipmentIsProcessed()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                                                          new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyProfile, RelatedObjectID = 789 },
                                                          ShortcutTriggerType.Barcode, "123");
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456) {Processed = true});
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<ApplyProfileMessage>());
        }
        
        [Fact]
        public void Register_DoesNotSendApplyProfileMessage_WhenShipmentIsProcessed()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                                                          new ShortcutEntity() { Action = KeyboardShortcutCommand.ApplyProfile, RelatedObjectID = 789 },
                                                          ShortcutTriggerType.Barcode, "123");
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456) {Processed = true});
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<ApplyProfileMessage>());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}