using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsAscendiaConsolidatorRestrictionTest
    {
        private readonly AutoMock mock;
        private readonly StampsAscendiaConsolidatorRestriction testObject;

        public StampsAscendiaConsolidatorRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<StampsAscendiaConsolidatorRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsStampsAscendiaConsolidator()
        {
            Assert.Equal(EditionFeature.StampsAscendiaConsolidator, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenStampsAscendiaConsolidatorCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.StampsAscendiaConsolidator)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenStampsAscendiaConsolidatorCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.StampsAscendiaConsolidator)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}