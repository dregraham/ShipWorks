using System;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class ChangeShipmentTypePipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Mock<ShippingPanelViewModel> viewModelMock;
        readonly ShippingPanelViewModel viewModel;

        public ChangeShipmentTypePipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v =>
            {
                v.Setup(x => x.IsProcessed).Returns(false);
                v.Setup(x => x.Populate(It.IsAny<ICarrierShipmentAdapter>()));
                v.CallBase = true;
            });
            viewModel = viewModelMock.Object;
        }

        [Fact]
        public void Constructor_CreatesLogger_WithCorrectType()
        {
            Type calledType = null;
            Func<Type, ILog> createLogger = t =>
            {
                calledType = t;
                return null;
            };
            mock.Provide(createLogger);

            mock.Create<ChangeShipmentTypePipeline>();

            Assert.Equal(typeof(ChangeShipmentTypePipeline), calledType);
        }

        [Fact]
        public void Register_ChangesShipmentType_WhenShipmentTypeChangesAndShipmentIsNotProcessed()
        {
            var shipmentEntity = new ShipmentEntity();
            viewModelMock.Setup(x => x.Shipment).Returns(shipmentEntity);

            var testObject = mock.Create<ChangeShipmentTypePipeline>();
            testObject.Register(viewModel);

            viewModel.ShipmentType = ShipmentTypeCode.Usps;

            mock.Mock<IShippingManager>()
                .Verify(x => x.ChangeShipmentType(ShipmentTypeCode.Usps, shipmentEntity));
        }

        [Fact]
        public void Register_ChangesShipmentAdapter_WhenShipmentTypeChangesAndShipmentIsNotProcessed()
        {
            var newAdapter = mock.Create<ICarrierShipmentAdapter>();
            mock.Mock<IShippingManager>()
                .Setup(x => x.ChangeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentEntity>()))
                .Returns(newAdapter);

            var testObject = mock.Create<ChangeShipmentTypePipeline>();
            testObject.Register(viewModel);

            viewModel.ShipmentType = ShipmentTypeCode.Usps;

            viewModelMock.Verify(x => x.Populate(newAdapter));
        }

        [Fact]
        public void Register_DoesNotChangeShipmentType_WhenShipmentTypeChangesButShipmentIsProcessed()
        {
            viewModelMock.Setup(x => x.IsProcessed).Returns(true);

            var testObject = mock.Create<ChangeShipmentTypePipeline>();
            testObject.Register(viewModel);

            viewModel.ShipmentType = ShipmentTypeCode.Usps;

            mock.Mock<IShippingManager>()
                .Verify(x => x.ChangeShipmentType(
                    It.IsAny<ShipmentTypeCode>(),
                    It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotChangeShipmentType_WhenPropertyIsNotShipmentType()
        {
            var testObject = mock.Create<ChangeShipmentTypePipeline>();
            testObject.Register(viewModel);

            viewModel.SupportsMultiplePackages = true;

            mock.Mock<IShippingManager>()
                .Verify(x => x.ChangeShipmentType(
                    It.IsAny<ShipmentTypeCode>(),
                    It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public void Register_ContinuesHandlingChanges_WhenChangingShipmentTypeThrowsException()
        {
            mock.Mock<IShippingManager>()
                .SetupSequence(x => x.ChangeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentEntity>()))
                .Throws<InvalidOperationException>();

            var testObject = mock.Create<ChangeShipmentTypePipeline>();
            testObject.Register(viewModel);

            viewModel.ShipmentType = ShipmentTypeCode.Usps;

            viewModel.ShipmentType = ShipmentTypeCode.OnTrac;

            mock.Mock<IShippingManager>()
                .Verify(x => x.ChangeShipmentType(
                    It.IsAny<ShipmentTypeCode>(),
                    It.IsAny<ShipmentEntity>()), Times.Exactly(2));
        }

        [Fact]
        public void Register_LogsAnError_WhenChangingShipmentTypeThrowsException()
        {
            Exception exception = new Exception("Foo");
            mock.Mock<IShippingManager>()
                .SetupSequence(x => x.ChangeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentEntity>()))
                .Throws(exception);

            var testObject = mock.Create<ChangeShipmentTypePipeline>();
            testObject.Register(viewModel);

            viewModel.ShipmentType = ShipmentTypeCode.Usps;

            mock.Mock<ILog>()
                .Verify(x => x.Error(exception));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
