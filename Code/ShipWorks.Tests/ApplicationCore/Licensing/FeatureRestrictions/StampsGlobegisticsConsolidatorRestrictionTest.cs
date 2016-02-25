using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsGlobegisticsConsolidatorRestrictionTest
    {
        private readonly AutoMock mock;
        private readonly StampsGlobegisticsConsolidatorRestriction testObject;

        public StampsGlobegisticsConsolidatorRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<StampsGlobegisticsConsolidatorRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsStampsGlobegisticsConsolidator()
        {
            Assert.Equal(EditionFeature.StampsGlobegisticsConsolidator, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenStampsGlobegisticsConsolidatorCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.StampsGlobegisticsConsolidator)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenStampsGlobegisticsConsolidatorCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.StampsGlobegisticsConsolidator)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}