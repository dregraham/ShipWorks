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
    public class EditionLicenseValidatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly EditionLicenseValidator testObject;

        public EditionLicenseValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<EditionLicenseValidator>();
        }

        [Fact]
        public void Validate_DelegatesToLicenseService()
        {
            testObject.Validate(new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.iParcel });

            mock.Mock<ILicenseService>().Verify(x => x.CheckRestriction(EditionFeature.ShipmentType, ShipmentTypeCode.iParcel));
        }

        [Fact]
        public void Validate_ReturnsSuccess_WhenLicenseIsNone()
        {
            mock.Mock<ILicenseService>()
                .Setup(x => x.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<ShipmentTypeCode>()))
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
                .Setup(x => x.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<ShipmentTypeCode>()))
                .Returns(level);

            var result = testObject.Validate(new ShipmentEntity());

            Assert.True(result.Failure);
            Assert.Contains("does not support shipping with", result.Message);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
