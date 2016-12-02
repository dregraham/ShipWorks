using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps.LabelRetrieval
{
    public class AllowedShipmentTypeValidatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly AllowedShipmentTypeValidator testObject;

        public AllowedShipmentTypeValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<AllowedShipmentTypeValidator>();
        }

        [Fact]
        public void Validate_DelegatesToLicenseService()
        {
            testObject.Validate(new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.iParcel });

            mock.Mock<ILicenseService>().Verify(x => x.CheckRestriction(EditionFeature.ProcessShipment, ShipmentTypeCode.iParcel));
        }

        [Theory]
        [InlineData(EditionRestrictionLevel.Hidden)]
        [InlineData(EditionRestrictionLevel.None)]
        [InlineData(EditionRestrictionLevel.RequiresUpgrade)]
        public void Validate_ReturnsSuccess_WhenLicenseIsNotForbidden(EditionRestrictionLevel level)
        {
            mock.Mock<ILicenseService>()
                .Setup(x => x.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<ShipmentTypeCode>()))
                .Returns(level);

            var result = testObject.Validate(new ShipmentEntity());

            Assert.True(result.Success);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenLicenseIsForbidden()
        {
            mock.Mock<ILicenseService>()
                .Setup(x => x.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<ShipmentTypeCode>()))
                .Returns(EditionRestrictionLevel.Forbidden);

            var result = testObject.Validate(new ShipmentEntity());

            Assert.True(result.Failure);
            Assert.Contains("no longer process", result.Message);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
