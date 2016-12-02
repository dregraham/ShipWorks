using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps.LabelRetrieval
{
    public class EnsureLoadedManipulatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly EnsureLoadedManipulator testObject;

        public EnsureLoadedManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<EnsureLoadedManipulator>();
        }

        [Fact]
        public void Manipulate_DelegatesToShippingManager_ToEnsureShipmentLoaded()
        {
            var shipment = new ShipmentEntity();
            var result = testObject.Manipulate(shipment);
            mock.Mock<IShippingManager>().Verify(x => x.EnsureShipmentLoaded(shipment));
        }

        [Fact]
        public void Manipulate_DelegatesToShipmentType_ToLoadDynamicData()
        {
            var shipment = new ShipmentEntity();
            var shipmentType = mock.Mock<ShipmentType>();
            mock.Mock<IShipmentTypeManager>().Setup(x => x.Get(shipment)).Returns(shipmentType);

            testObject.Manipulate(shipment);

            shipmentType.Verify(x => x.UpdateDynamicShipmentData(shipment));
        }

        [Fact]
        public void Manipulate_ReturnsShipment()
        {
            var shipment = new ShipmentEntity();
            var result = testObject.Manipulate(shipment);
            Assert.Same(shipment, result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
