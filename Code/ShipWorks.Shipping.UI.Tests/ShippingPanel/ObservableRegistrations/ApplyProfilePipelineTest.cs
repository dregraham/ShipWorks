using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
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
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
        }

        [Fact]
        public void Register_DoesNotDelegateToShipmentTypeManager_WhenShipmentDoesNotMatchViewModel()
        {
            ApplyProfilePipeline testObject = mock.Create<ApplyProfilePipeline>();
            testObject.Register(mock.Create<ShippingPanelViewModel>());

            messenger.OnNext(new ApplyProfileMessage(this, 1234, 0));

            mock.Mock<IShippingProfileService>()
                .Verify(x => x.Get(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Register_DelegatesToApplyProfile_WhenShipmentMatchesViewModel()
        {
            Mock<IShippingProfile> shippingProfile = mock.CreateMock<IShippingProfile>();
            ShipmentEntity shipment = new ShipmentEntity { ShipmentID = 12 };

            Mock<ShippingPanelViewModel> viewModel = mock.CreateMock<ShippingPanelViewModel>();
            viewModel.Setup(x => x.Shipment).Returns(shipment);

            ShippingProfileEntity profile = new ShippingProfileEntity() { ShippingProfileID = 123 };

            mock.Mock<IShippingProfileService>()
                .Setup(x => x.Get(123))
                .Returns(shippingProfile);

            ApplyProfilePipeline testObject = mock.Create<ApplyProfilePipeline>();
            testObject.Register(viewModel.Object);

            messenger.OnNext(new ApplyProfileMessage(this, 12, profile.ShippingProfileID));

            shippingProfile.Verify(x => x.Apply(shipment));
        }

        [Fact]
        public void Register_CallsLoadShipment_WhenShipmentMatchesViewModel()
        {
            Mock<ShippingPanelViewModel> viewModel = mock.CreateMock<ShippingPanelViewModel>();
            viewModel.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 12 });

            var adapter = mock.Mock<ICarrierShipmentAdapter>();

            var profile = mock.Mock<IShippingProfile>();
            profile.Setup(p => p.Apply(It.IsAny<ShipmentEntity>())).Returns(adapter);

            mock.Mock<IShippingProfileService>().Setup(s => s.Get(It.IsAny<long>())).Returns(profile);

            ApplyProfilePipeline testObject = mock.Create<ApplyProfilePipeline>();
            testObject.Register(viewModel.Object);

            messenger.OnNext(new ApplyProfileMessage(this, 12, 0));

            viewModel.Verify(x => x.LoadShipment(adapter.Object));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
