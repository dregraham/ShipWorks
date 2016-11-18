using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.GetLabel;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps.GetLabel
{
    public class InsuranceValidatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly InsuranceValidator testObject;

        public InsuranceValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<InsuranceValidator>();
        }

        [Fact]
        public void Validate_Throws_WhenInsuranceUtilityThrows()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            mock.Mock<IInsuranceUtility>().Setup(x => x.ValidateShipment(shipment)).Throws<Exception>();

            Assert.Throws<Exception>(() => testObject.Validate(shipment));
        }

        [Fact]
        public void Validate_ReturnsSuccess_WhenInsuranceUtilityDoesNotThrow()
        {
            var result = testObject.Validate(new ShipmentEntity());
            Assert.True(result.Success);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
