using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Endicia;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class EndiciaScanBasedReturnsRestrictionTest
    {
        private readonly AutoMock mock;
        private readonly EndiciaScanBasedReturnsRestriction testObject;

        public EndiciaScanBasedReturnsRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<EndiciaScanBasedReturnsRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsEndiciaScanBasedReturns()
        {
            Assert.Equal(EditionFeature.EndiciaScanBasedReturns, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenEndiciaScanBasedReturnsCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.EndiciaScanBasedReturns)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenEndiciaScanBasedReturnsCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.EndiciaScanBasedReturns)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}