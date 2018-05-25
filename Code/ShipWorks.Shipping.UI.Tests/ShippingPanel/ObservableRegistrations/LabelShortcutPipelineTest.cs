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
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class LabelShortcutPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly TestScheduler scheduler;
        private readonly LabelShortcutPipeline testObject;
        private readonly Mock<IMainForm> mainForm;
        private readonly Mock<ShippingPanelViewModel> viewModel;

        public LabelShortcutPipelineTest()
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

            testObject = mock.Create<LabelShortcutPipeline>();
            testObject.Register(viewModel.Object);
        }

        [Fact]
        public void Register_SendsCreateLabelMessage_WhenShortcutMessageReceived_AndAppliesToCreateLabel()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.CreateLabel },
                ShortcutTriggerType.Barcode, "-PL-");
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456));
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            mainForm.Setup(m => m.IsShippingPanelOpen()).Returns(true);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Equal(1, testMessenger.SentMessages.OfType<CreateLabelMessage>()
                .Count(m => m.ShipmentID == 456 && m.Sender == testObject));
        }
        
        
        [Fact]
        public void Register_DoesNotSendMessage_WhenShippingPanelIsClosed()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.CreateLabel },
                ShortcutTriggerType.Barcode, "-PL-");
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456));
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            mainForm.Setup(m => m.IsShippingPanelOpen()).Returns(false);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<CreateLabelMessage>());
        }
        
        [Fact]
        public void Register_CallsMainFormFocus_WhenShortcutMessageReceived_AndAppliesToCreateLabel()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.CreateLabel },
                ShortcutTriggerType.Barcode, "-PL-");
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456));
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            mainForm.Setup(m => m.IsShippingPanelOpen()).Returns(true);

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
            mainForm.Setup(m => m.IsShippingPanelOpen()).Returns(true);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<CreateLabelMessage>());
        }
        
        [Fact]
        public void Register_DoesNotSendMessage_ViewModelDoesNotHaveShipment()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyWeight, RelatedObjectID = 789 },
                ShortcutTriggerType.Barcode, "123");

            viewModel.SetupGet(v => v.Shipment).Returns((ShipmentEntity) null);

            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            mainForm.Setup(m => m.IsShippingPanelOpen()).Returns(true);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<CreateLabelMessage>());
        }

        [Fact]
        public void Register_DoesNotSendMessage_WhenAdditionalFormsAreOpen()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyWeight, RelatedObjectID = 789 },
                ShortcutTriggerType.Barcode, "123");
            
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456));
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(true);
            mainForm.Setup(m => m.IsShippingPanelOpen()).Returns(true);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<CreateLabelMessage>());
        }
        
        [Fact]
        public void Register_DoesNotSendMessage_WhenShipmentIsProcessed()
        {
            ShortcutMessage message = new ShortcutMessage(this,
                new ShortcutEntity { Action = KeyboardShortcutCommand.ApplyWeight, RelatedObjectID = 789 },
                ShortcutTriggerType.Barcode, "123");
            
            viewModel.SetupGet(v => v.Shipment).Returns(new ShipmentEntity(456) {Processed = true});
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            mainForm.Setup(m => m.IsShippingPanelOpen()).Returns(true);

            testMessenger.Send(message);
            scheduler.Start();

            Assert.Empty(testMessenger.SentMessages.OfType<CreateLabelMessage>());
        }
        
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}