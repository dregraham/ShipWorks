using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using System;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class ChangeShipmentTypePipelineTest : IDisposable
    {
        readonly AutoMock mock;

        public ChangeShipmentTypePipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
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
            var viewModel = CreateViewModel(shipmentEntity);

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

            var viewModel = CreateViewModel();

            var testObject = mock.Create<ChangeShipmentTypePipeline>();
            testObject.Register(viewModel);

            viewModel.ShipmentType = ShipmentTypeCode.Usps;

            Assert.Equal(newAdapter, viewModel.ShipmentAdapter);
        }

        [Fact]
        public void Register_DoesNotChangeShipmentType_WhenShipmentTypeChangesButShipmentIsProcessed()
        {
            var viewModel = CreateViewModel(new ShipmentEntity { Processed = true });

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
            ShippingPanelViewModel viewModel = CreateViewModel();

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

            var viewModel = CreateViewModel();

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

            var viewModel = CreateViewModel();

            var testObject = mock.Create<ChangeShipmentTypePipeline>();
            testObject.Register(viewModel);

            viewModel.ShipmentType = ShipmentTypeCode.Usps;

            mock.Mock<ILog>()
                .Verify(x => x.Error(exception));
        }

        private ShippingPanelViewModel CreateViewModel() => CreateViewModel(new ShipmentEntity());

        private ShippingPanelViewModel CreateViewModel(ShipmentEntity shipment)
        {
            var adapter = mock.Mock<ICarrierShipmentAdapter>();
            adapter.Setup(x => x.Shipment).Returns(shipment);

            var viewModel = mock.Create<ShippingPanelViewModel>();
            viewModel.Populate(adapter.Object);

            return viewModel;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
