using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.GetLabel;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps.GetLabel
{
    public class SupportsReturnsManipulatorTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly SupportsReturnsManipulator testObject;

        public SupportsReturnsManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<SupportsReturnsManipulator>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Manipulate_SetsReturnToFalse_WhenReturnIsNotAllowedByShipmentType(bool returnInput)
        {
            mock.WithShipmentTypeFromShipmentManager(s => s.SetupGet(x => x.SupportsReturns).Returns(false));
            var result = testObject.Manipulate(new ShipmentEntity { ReturnShipment = returnInput });
            Assert.False(result.ReturnShipment);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Manipulate_DoesNotModifyReturn_WhenReturnIsAllowedByShipmentType(bool returnInput)
        {
            mock.WithShipmentTypeFromShipmentManager(s => s.SetupGet(x => x.SupportsReturns).Returns(true));
            var result = testObject.Manipulate(new ShipmentEntity { ReturnShipment = returnInput });
            Assert.Equal(returnInput, result.ReturnShipment);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
