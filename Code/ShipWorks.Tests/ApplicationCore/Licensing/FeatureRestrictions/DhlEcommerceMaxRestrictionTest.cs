using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.DhlEcommerce;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class DhlEcommerceMaxRestrictionTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DhlEcommerceMaxRestriction testObject;

        public DhlEcommerceMaxRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<DhlEcommerceMaxRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsDhlEcommerceMax()
        {
            Assert.Equal(EditionFeature.DhlEcommerceMax, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenDhlEcommerceMaxCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.DhlEcommerceMax)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenDhlEcommerceMaxCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.DhlEcommerceMax)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}