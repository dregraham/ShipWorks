using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Endicia;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class EndiciaDhlRestrictionTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly EndiciaDhlRestriction testObject;

        public EndiciaDhlRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<EndiciaDhlRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsEndiciaDhl()
        {
            Assert.Equal(EditionFeature.EndiciaDhl, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenEndiciaDhlCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.EndiciaDhl)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenEndiciaDhlCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.EndiciaDhl)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}