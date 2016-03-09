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
    public class VoidLabelPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> messages;

        public VoidLabelPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            messages = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(messages);
        }

        [Fact]
        public void Register_CallsVoidLabel_WhenShipmentIDofMessageMatchesViewModel()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();
            viewModel.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 123 });
            var testObject = mock.Create<VoidLabelPipeline>();
            testObject.Register(viewModel.Object);

            VoidLabelMessage voidLabelMessage = new VoidLabelMessage(this, 123);
            messages.OnNext(voidLabelMessage);

            viewModel.Verify(x => x.VoidLabel(voidLabelMessage));
        }

        [Fact]
        public void Register_DoesNotCallVoidLabel_WhenShipmentIDofMessageDoesNotMatchViewModel()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();
            viewModel.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 456 });
            var testObject = mock.Create<VoidLabelPipeline>();
            testObject.Register(viewModel.Object);

            VoidLabelMessage voidLabelMessage = new VoidLabelMessage(this, 123);
            messages.OnNext(voidLabelMessage);

            viewModel.Verify(x => x.VoidLabel(voidLabelMessage), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallVoidLabel_WhenNoShipmentIsLoaded()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();
            var testObject = mock.Create<VoidLabelPipeline>();
            testObject.Register(viewModel.Object);

            VoidLabelMessage voidLabelMessage = new VoidLabelMessage(this, 123);
            messages.OnNext(voidLabelMessage);

            viewModel.Verify(x => x.VoidLabel(voidLabelMessage), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
