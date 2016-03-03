using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Stamps;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsDhlConsolidatorRestrictionTest
    {
        private readonly AutoMock mock;
        private readonly StampsDhlConsolidatorRestriction testObject;

        public StampsDhlConsolidatorRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<StampsDhlConsolidatorRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsStampsDhlConsolidator()
        {
            Assert.Equal(EditionFeature.StampsDhlConsolidator, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenStampsDhlConsolidatorCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.StampsDhlConsolidator)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenStampsDhlConsolidatorCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.StampsDhlConsolidator)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}