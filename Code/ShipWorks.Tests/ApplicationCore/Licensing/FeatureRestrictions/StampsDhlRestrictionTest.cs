using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsDhlRestrictionTest
    {
        private readonly AutoMock mock;
        private readonly StampsDhlRestriction testObject;

        public StampsDhlRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<StampsDhlRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsStampsDhl()
        {
            Assert.Equal(EditionFeature.StampsDhl, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenStampsDhlCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.StampsDhl)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenStampsDhlCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.StampsDhl)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}