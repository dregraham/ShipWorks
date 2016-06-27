using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Moq;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.RatingPanel;
using ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.RatingPanel.ObservableRegistrations
{
    public class ShipmentChangedPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> subject;

        public ShipmentChangedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            subject = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);
        }

        [Fact]
        public void Register_DoesNotCallSelectRate_WhenPropertyIsNotSet()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new ShipmentChangedMessage(this, mock.Create<ICarrierShipmentAdapter>()));

            viewModel.Verify(x => x.SelectRate(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallSelectRate_WhenPropertyIsNotServiceType()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new ShipmentChangedMessage(this, mock.Create<ICarrierShipmentAdapter>(), "Foo"));

            viewModel.Verify(x => x.SelectRate(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_CallsSelectRate_WhenServiceTypeChanged()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);
            var adapter = mock.Create<ICarrierShipmentAdapter>();

            subject.OnNext(new ShipmentChangedMessage(this, adapter, "ServiceType"));

            viewModel.Verify(x => x.SelectRate(adapter));
        }

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
