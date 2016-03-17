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
    public class ApplyProfilePipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> messenger;

        public ApplyProfilePipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            messenger = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(messenger);
        }

        [Fact]
        public void Register_DoesNotDelegateToShipmentTypeManager_WhenShipmentDoesNotMatchViewModel()
        {
            ApplyProfilePipeline testObject = mock.Create<ApplyProfilePipeline>();
            testObject.Register(mock.Create<ShippingPanelViewModel>());

            messenger.OnNext(new ApplyProfileMessage(this, 1234, new ShippingProfileEntity()));

            mock.Mock<IShipmentTypeManager>()
                .Verify(x => x.Get(It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public void Register_DelegatesToApplyProfile_WhenShipmentMatchesViewModel()
        {
            Mock<ShipmentType> shipmentTypeMock = mock.CreateMock<ShipmentType>();
            ShipmentEntity shipment = new ShipmentEntity { ShipmentID = 12 };

            Mock<ShippingPanelViewModel> viewModel = mock.CreateMock<ShippingPanelViewModel>();
            viewModel.Setup(x => x.Shipment).Returns(shipment);

            ShippingProfileEntity profile = new ShippingProfileEntity();

            mock.Mock<IShipmentTypeManager>()
                .Setup(x => x.Get(shipment))
                .Returns(shipmentTypeMock.Object);

            ApplyProfilePipeline testObject = mock.Create<ApplyProfilePipeline>();
            testObject.Register(viewModel.Object);

            messenger.OnNext(new ApplyProfileMessage(this, 12, profile));

            shipmentTypeMock.Verify(x => x.ApplyProfile(shipment, profile));
        }

        [Fact]
        public void Register_CallsLoadShipment_WhenShipmentMatchesViewModel()
        {
            Mock<ShippingPanelViewModel> viewModel = mock.CreateMock<ShippingPanelViewModel>();
            viewModel.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 12 });

            ShippingProfileEntity profile = new ShippingProfileEntity();

            ApplyProfilePipeline testObject = mock.Create<ApplyProfilePipeline>();
            testObject.Register(viewModel.Object);

            messenger.OnNext(new ApplyProfileMessage(this, 12, new ShippingProfileEntity()));

            viewModel.Verify(x => x.LoadShipment(viewModel.Object.ShipmentAdapter));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
