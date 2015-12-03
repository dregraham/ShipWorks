using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
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
        public void Register_DoesNotCallPopulate_WhenSenderIsViewModel()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new ShipmentChangedMessage(viewModel.Object, mock.Create<ICarrierShipmentAdapter>()));

            viewModel.Verify(x => x.Populate(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallPopulate_WhenMessageShipmentIsNull()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(new ShipmentEntity());

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            var adapter = mock.CreateMock<ICarrierShipmentAdapter>(a => a.Setup(x => x.Shipment).Returns((ShipmentEntity) null)).Object;
            subject.OnNext(new ShipmentChangedMessage(new object(), adapter));

            viewModel.Verify(x => x.Populate(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallPopulate_WhenViewModelShipmentIsNull()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(null);

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            var adapter = mock.CreateMock<ICarrierShipmentAdapter>(a => a.Setup(x => x.Shipment).Returns(new ShipmentEntity())).Object;
            subject.OnNext(new ShipmentChangedMessage(new object(), adapter));

            viewModel.Verify(x => x.Populate(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallPopulate_WhenShipmentIdsDoNotMatch()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(new ShipmentEntity { ShipmentID = 3 });

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            var adapter = mock.CreateMock<ICarrierShipmentAdapter>(a => a.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 5 })).Object;
            subject.OnNext(new ShipmentChangedMessage(new object(), adapter));

            viewModel.Verify(x => x.Populate(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_CallsPopulate_WhenShipmentIdsMatch()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(new ShipmentEntity { ShipmentID = 3 });

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            var adapter = mock.CreateMock<ICarrierShipmentAdapter>(a => a.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 3 })).Object;
            subject.OnNext(new ShipmentChangedMessage(new object(), adapter));

            viewModel.Verify(x => x.Populate(adapter));
        }

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
