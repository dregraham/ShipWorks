using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class CreateLabelPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> messages;

        public CreateLabelPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            messages = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(messages);
        }

        [Fact]
        public void Register_CallsCreateLabel_WhenShipmentIDofMessageMatchesViewModel()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();
            viewModel.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 123 });
            var testObject = mock.Create<CreateLabelPipeline>();
            testObject.Register(viewModel.Object);

            messages.OnNext(new CreateLabelMessage(this, 123));

            viewModel.Verify(x => x.CreateLabel());
        }

        [Fact]
        public void Register_DoesNotCallCreateLabel_WhenShipmentIDofMessageDoesNotMatchViewModel()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();
            viewModel.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 456 });
            var testObject = mock.Create<CreateLabelPipeline>();
            testObject.Register(viewModel.Object);

            messages.OnNext(new CreateLabelMessage(this, 123));

            viewModel.Verify(x => x.CreateLabel(), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallCreateLabel_WhenNoShipmentIsLoaded()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();
            var testObject = mock.Create<CreateLabelPipeline>();
            testObject.Register(viewModel.Object);

            messages.OnNext(new CreateLabelMessage(this, 123));

            viewModel.Verify(x => x.CreateLabel(), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
