using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
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
    public class CarrierConfiguredPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly TestSchedulerProvider schedulerProvider;
        readonly Mock<ShippingPanelViewModel> viewModel;
        readonly Subject<IShipWorksMessage> messages;

        public CarrierConfiguredPipelineTest()
        {
            schedulerProvider = new TestSchedulerProvider();
            messages = new Subject<IShipWorksMessage>();

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Provide<ISchedulerProvider>(schedulerProvider);
            mock.Provide<IObservable<IShipWorksMessage>>(messages);

            viewModel = mock.CreateMock<ShippingPanelViewModel>();
        }

        [Fact]
        public void Register_DoesNotUpdateAllowEditing_WhenChangedCarrierDoesNotMatchViewModel()
        {
            viewModel.Setup(x => x.ShipmentType).Returns(ShipmentTypeCode.Usps);
            var testObject = mock.Create<CarrierConfiguredPipeline>();
            testObject.Register(viewModel.Object);

            messages.OnNext(new CarrierConfiguredMessage(this, ShipmentTypeCode.FedEx));

            viewModel.VerifySet(x => x.AllowEditing = false, Times.Never);
        }

        [Fact]
        public void Register_DoesNotDelegateToShippingManager_WhenSchedulerHasNotAdvanced()
        {
            var testObject = mock.Create<CarrierConfiguredPipeline>();
            testObject.Register(viewModel.Object);

            messages.OnNext(new CarrierConfiguredMessage(this, viewModel.Object.ShipmentType));

            mock.Mock<IShippingManager>().Verify(x => x.GetShipment(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Register_DelegatesToShippingManager_OnTaskPool()
        {
            viewModel.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 123 });
            var testObject = mock.Create<CarrierConfiguredPipeline>();
            testObject.Register(viewModel.Object);

            messages.OnNext(new CarrierConfiguredMessage(this, viewModel.Object.ShipmentType));
            schedulerProvider.TaskPool.Start();

            mock.Mock<IShippingManager>().Verify(x => x.GetShipment(123));
        }

        [Fact]
        public void Register_DoesNotLoadShipment_WhenViewModelNoLongerShowsPreviousShipment()
        {
            var shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 123 });

            mock.Mock<IShippingManager>()
                .Setup(x => x.GetShipment(It.IsAny<long>()))
                .Returns(shipmentAdapter.Object);
            var testObject = mock.Create<CarrierConfiguredPipeline>();
            testObject.Register(viewModel.Object);

            messages.OnNext(new CarrierConfiguredMessage(this, viewModel.Object.ShipmentType));
            schedulerProvider.TaskPool.Start();
            viewModel.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 999 });
            schedulerProvider.Dispatcher.Start();

            viewModel.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotLoadShipment_WhenDispatcherSchedulerHasNotElapsed()
        {
            var shipment = new ShipmentEntity { ShipmentID = 123 };
            var shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.Shipment).Returns(shipment);

            mock.Mock<IShippingManager>()
                .Setup(x => x.GetShipment(It.IsAny<long>()))
                .Returns(shipmentAdapter.Object);
            var testObject = mock.Create<CarrierConfiguredPipeline>();
            testObject.Register(viewModel.Object);

            messages.OnNext(new CarrierConfiguredMessage(this, viewModel.Object.ShipmentType));
            schedulerProvider.TaskPool.Start();
            viewModel.Setup(x => x.Shipment).Returns(shipment);

            viewModel.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_LoadsShipment_WhenDispatcherSchedulerHasElapsed()
        {
            var shipment = new ShipmentEntity { ShipmentID = 123 };
            var shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.Shipment).Returns(shipment);

            mock.Mock<IShippingManager>()
                .Setup(x => x.GetShipment(It.IsAny<long>()))
                .Returns(shipmentAdapter.Object);
            var testObject = mock.Create<CarrierConfiguredPipeline>();
            testObject.Register(viewModel.Object);

            messages.OnNext(new CarrierConfiguredMessage(this, viewModel.Object.ShipmentType));
            schedulerProvider.TaskPool.Start();
            viewModel.Setup(x => x.Shipment).Returns(shipment);
            schedulerProvider.Dispatcher.Start();

            viewModel.Verify(x => x.LoadShipment(shipmentAdapter.Object));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
