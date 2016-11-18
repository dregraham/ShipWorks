using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.GetLabel;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps.GetLabel
{
    public class POBoxLicenseValidatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly POBoxLicenseValidator testObject;

        public POBoxLicenseValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<POBoxLicenseValidator>();
        }

        [Fact]
        public void Validate_DelegatesToLicenseService()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.iParcel };
            testObject.Validate(shipment);

            mock.Mock<ILicenseService>().Verify(x => x.CheckRestriction(EditionFeature.PostalApoFpoPoboxOnly, shipment));
        }

        [Fact]
        public void Validate_ReturnsSuccess_WhenLicenseIsNone()
        {
            mock.Mock<ILicenseService>()
                .Setup(x => x.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<ShipmentEntity>()))
                .Returns(EditionRestrictionLevel.None);

            var result = testObject.Validate(new ShipmentEntity());

            Assert.True(result.Success);
        }

        [Theory]
        [InlineData(EditionRestrictionLevel.Hidden)]
        [InlineData(EditionRestrictionLevel.Forbidden)]
        [InlineData(EditionRestrictionLevel.RequiresUpgrade)]
        public void Validate_ReturnsFailure_WhenLicenseIsForbidden(EditionRestrictionLevel level)
        {
            mock.Mock<ILicenseService>()
                .Setup(x => x.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<ShipmentEntity>()))
                .Returns(level);

            var result = testObject.Validate(new ShipmentEntity());

            Assert.True(result.Failure);
            Assert.Contains("using APO, FPO, and P.O.", result.Message);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
