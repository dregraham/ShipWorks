using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Ups;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions.Ups
{
    public class UpsSurePostRestrictionTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly UpsSurePostRestriction testObject;

        public UpsSurePostRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<UpsSurePostRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsUpsSurePost()
        {
            Assert.Equal(EditionFeature.UpsSurePost, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsHidden_WhenUpsSurePostCapabilityIsFalse()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.UpsSurePost)
                .Returns(false);

            Assert.Equal(EditionRestrictionLevel.Hidden, testObject.Check(licenseCapabilities.Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenUpsSurePostCapabilityIsTrue()
        {
            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.UpsSurePost)
                .Returns(true);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
        }
    }
}