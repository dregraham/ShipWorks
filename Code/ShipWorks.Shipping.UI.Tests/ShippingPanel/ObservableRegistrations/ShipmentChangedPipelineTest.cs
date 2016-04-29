using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Moq;
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
        public void Register_DoesNotCallLoadShipment_WhenSenderIsShippingPanelViewModel()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new ShipmentChangedMessage(viewModel.Object, mock.Create<ICarrierShipmentAdapter>()));

            viewModel.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallLoadShipment_WhenSenderIsShipmentViewModel()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(null)
                .WithShipmentViewModel();

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new ShipmentChangedMessage(viewModel.Object.ShipmentViewModel, mock.Create<ICarrierShipmentAdapter>()));

            viewModel.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallLoadShipment_WhenSenderIsInsuranceViewModel()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(null)
                .WithShipmentViewModel();

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new ShipmentChangedMessage(viewModel.Object.ShipmentViewModel.InsuranceViewModel, mock.Create<ICarrierShipmentAdapter>()));

            viewModel.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallLoadShipment_WhenSenderIsAddressViewModel()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(null)
                .WithShipmentViewModel();

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new ShipmentChangedMessage(viewModel.Object.Origin, mock.Create<ICarrierShipmentAdapter>()));
            subject.OnNext(new ShipmentChangedMessage(viewModel.Object.Destination, mock.Create<ICarrierShipmentAdapter>()));

            viewModel.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallLoadShipment_WhenMessageShipmentIsNull()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(new ShipmentEntity())
                .WithShipmentViewModel();

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            var adapter = mock.CreateMock<ICarrierShipmentAdapter>(a => a.Setup(x => x.Shipment).Returns((ShipmentEntity) null)).Object;
            subject.OnNext(new ShipmentChangedMessage(new object(), adapter));

            viewModel.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesCallLoadShipment_WhenViewModelShipmentIsNull()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(null)
                .WithShipmentViewModel();

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            var adapter = mock.CreateMock<ICarrierShipmentAdapter>(a => a.Setup(x => x.Shipment).Returns(new ShipmentEntity())).Object;
            subject.OnNext(new ShipmentChangedMessage(new object(), adapter));

            viewModel.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Once);
        }

        [Fact]
        public void Register_DoesNotCallLoadShipment_WhenShipmentIdsDoNotMatch()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(new ShipmentEntity { ShipmentID = 3 })
                .WithShipmentViewModel();

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            var adapter = mock.CreateMock<ICarrierShipmentAdapter>(a => a.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 5 })).Object;
            subject.OnNext(new ShipmentChangedMessage(new object(), adapter));

            viewModel.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_CallsLoadShipment_WhenShipmentIdsMatch()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(new ShipmentEntity { ShipmentID = 3 })
                .WithShipmentViewModel();

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            var adapter = mock.CreateMock<ICarrierShipmentAdapter>(a => a.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 3 })).Object;
            subject.OnNext(new ShipmentChangedMessage(new object(), adapter));

            viewModel.Verify(x => x.LoadShipment(adapter));
        }


        [Fact]
        public void Register_DoesNotCallLoadShipment_WhenMessageSenderIsNull()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(new ShipmentEntity { ShipmentID = 3 })
                .WithShipmentViewModel();

            var testObject = mock.Create<ShipmentChangedPipeline>();
            testObject.Register(viewModel.Object);

            var adapter = mock.CreateMock<ICarrierShipmentAdapter>(a => a.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 5 })).Object;
            subject.OnNext(new ShipmentChangedMessage(null, adapter));

            viewModel.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
