using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsRrDonnelleyConsolidatorRestrictionTest
    {
        private readonly AutoMock mock;
        private readonly StampsRrDonnelleyConsolidatorRestriction testObject;

        public StampsRrDonnelleyConsolidatorRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<StampsRrDonnelleyConsolidatorRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsStampsRrDonnelleyConsolidator()
        {
            Assert.Equal(EditionFeature.StampsRrDonnelleyConsolidator, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenStampsRrDonnelleyConsolidatorCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.StampsRrDonnelleyConsolidator)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenStampsRrDonnelleyConsolidatorCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.StampsRrDonnelleyConsolidator)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}