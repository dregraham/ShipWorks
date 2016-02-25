using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class EndiciaInsuranceRestrictionTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly EndiciaInsuranceRestriction testObject;

        public EndiciaInsuranceRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<EndiciaInsuranceRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsEndiciaInsurance()
        {
            Assert.Equal(EditionFeature.EndiciaInsurance, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenEndiciaInsuranceCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.EndiciaInsurance)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenEndiciaInsuranceCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.EndiciaInsurance)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}