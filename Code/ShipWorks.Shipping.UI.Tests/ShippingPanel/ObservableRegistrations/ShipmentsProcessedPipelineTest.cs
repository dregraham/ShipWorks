using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class ShipmentsProcessedPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> subject;
        private readonly ShipmentEntity processedShipment;
        private readonly Mock<ShippingPanelViewModel> viewModelMock;

        public ShipmentsProcessedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            processedShipment = new ShipmentEntity { ShipmentID = 123 };
            viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v =>
            {
                v.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 123 });
            });

            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());

            subject = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);
        }

        [Fact]
        public void Register_DoesNotCallPopulate_WhenMessageTypeIsntExpected()
        {
            ShipmentsProcessedPipeline testObject = mock.Create<ShipmentsProcessedPipeline>();
            testObject.Register(viewModelMock.Object);

            subject.OnNext(mock.Create<IShipWorksMessage>());

            viewModelMock.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallPopulate_WhenProcessedShipmentsAreNotInViewModel()
        {
            ShipmentsProcessedPipeline testObject = mock.Create<ShipmentsProcessedPipeline>();
            testObject.Register(viewModelMock.Object);

            subject.OnNext(new ShipmentsProcessedMessage(this,
                new[] { new ProcessShipmentResult(new ShipmentEntity { ShipmentID = 456 }) }));

            viewModelMock.Verify(x => x.LoadShipment(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_CallsPopulate_WithCarrierCreatedFromShipment()
        {
            var shipmentAdapter = mock.Create<ICarrierShipmentAdapter>();

            mock.Mock<ICarrierShipmentAdapterFactory>()
                .Setup(x => x.Get(processedShipment))
                .Returns(shipmentAdapter);

            ShipmentsProcessedPipeline testObject = mock.Create<ShipmentsProcessedPipeline>();
            testObject.Register(viewModelMock.Object);

            subject.OnNext(new ShipmentsProcessedMessage(this,
                new[] { new ProcessShipmentResult(processedShipment) }));

            viewModelMock.Verify(x => x.LoadShipment(shipmentAdapter));
        }

        [Fact]
        public void Register_ExecutesOnDispatcherThread_WhenProcessingIsFinished()
        {
            var scheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(scheduler);

            ShipmentsProcessedPipeline testObject = mock.Create<ShipmentsProcessedPipeline>();
            testObject.Register(viewModelMock.Object);

            subject.OnNext(new ShipmentsProcessedMessage(this,
                new[] { new ProcessShipmentResult(processedShipment) }));

            scheduler.Dispatcher.AdvanceBy(1);

            mock.Mock<ICarrierShipmentAdapterFactory>()
                .Verify(x => x.Get(It.IsAny<ShipmentEntity>()));
        }

        public void Dispose()
        {
            subject.Dispose();
            mock.Dispose();
        }
    }
}
