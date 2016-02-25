using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class EndiciaConsolidatorRestrictionTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly EndiciaConsolidatorRestriction testObject;

        public EndiciaConsolidatorRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<EndiciaConsolidatorRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsEndiciaConsolidator()
        {
            Assert.Equal(EditionFeature.EndiciaConsolidator, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenEndiciaConsolidatorCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.EndiciaConsolidator)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenEndiciaConsolidatorCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.EndiciaConsolidator)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}