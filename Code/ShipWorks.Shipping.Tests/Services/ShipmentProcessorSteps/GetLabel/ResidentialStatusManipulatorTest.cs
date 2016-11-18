using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.GetLabel;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps.GetLabel
{
    public class ResidentialStatusManipulatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ResidentialStatusManipulator testObject;

        public ResidentialStatusManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ResidentialStatusManipulator>();
        }

        [Fact]
        public void Manipulate_DelegatesToShipmentType()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            Mock<ShipmentType> shipmentType = mock.Mock<ShipmentType>();
            Mock<IShipmentTypeManager> manager = mock.Mock<IShipmentTypeManager>();
            manager.Setup(x => x.Get(shipment)).Returns(shipmentType);

            testObject.Manipulate(shipment);

            shipmentType.Verify(x => x.IsResidentialStatusRequired(shipment));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Manipulate_DoesNotChangeStatus_WhenResidentialStatusIsNotRequired(bool status)
        {
            //mock.WithShipmentTypeFromShipmentManager(s =>
            //    s.Setup(x => x.IsResidentialStatusRequired(It.IsAny<ShipmentEntity>())).Returns(false));

            var result = testObject.Manipulate(new ShipmentEntity { ResidentialResult = status });

            Assert.Equal(status, result.ResidentialResult);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Manipulate_SetsStatus_WhenResidentialStatusIsRequired(bool status)
        {
            ShipmentEntity shipment = new ShipmentEntity();

            mock.WithShipmentTypeFromShipmentManager(s =>
                s.Setup(x => x.IsResidentialStatusRequired(It.IsAny<ShipmentEntity>())).Returns(true));

            mock.Mock<IResidentialDeterminationService>().Setup(x => x.IsResidentialAddress(shipment))
                .Returns(status);

            var result = testObject.Manipulate(shipment);

            Assert.Equal(status, result.ResidentialResult);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
