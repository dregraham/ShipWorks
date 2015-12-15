using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
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
        AutoMock mock;
        Subject<IShipWorksMessage> subject;

        public ShipmentsProcessedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            subject = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);
        }

        [Fact]
        public void Register_DoesNotCallPopulate_WhenMessageTypeIsntExpected()
        {
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v =>
            {
                v.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 123 });
            });

            ShipmentsProcessedPipeline testObject = mock.Create<ShipmentsProcessedPipeline>();
            testObject.Register(viewModelMock.Object);

            subject.OnNext(mock.Create<IShipWorksMessage>());

            viewModelMock.Verify(x => x.Populate(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallPopulate_WhenProcessedShipmentsAreNotInViewModel()
        {
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v =>
            {
                v.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 123 });
            });

            ShipmentsProcessedPipeline testObject = mock.Create<ShipmentsProcessedPipeline>();
            testObject.Register(viewModelMock.Object);

            subject.OnNext(new ShipmentsProcessedMessage(this,
                new[] { new ProcessShipmentResult(new ShipmentEntity { ShipmentID = 456 }) }));

            viewModelMock.Verify(x => x.Populate(It.IsAny<ICarrierShipmentAdapter>()), Times.Never);
        }

        [Fact]
        public void Register_CallsPopulate_WithCarrierCreatedFromShipment()
        {
            var processedShipment = new ShipmentEntity { ShipmentID = 123 };
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v =>
            {
                v.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 123 });
            });

            var shipmentAdapter = mock.Create<ICarrierShipmentAdapter>();

            mock.Mock<ICarrierShipmentAdapterFactory>()
                .Setup(x => x.Get(processedShipment))
                .Returns(shipmentAdapter);

            ShipmentsProcessedPipeline testObject = mock.Create<ShipmentsProcessedPipeline>();
            testObject.Register(viewModelMock.Object);

            subject.OnNext(new ShipmentsProcessedMessage(this,
                new[] { new ProcessShipmentResult(processedShipment) }));

            viewModelMock.Verify(x => x.Populate(shipmentAdapter));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
