using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Ups;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions.Ups
{
    public class UpsAccountNumberRestrictionTest
    {
        [Fact]
        public void EditionFeature_ReturnsUpsAccountNumbers()
        {
            UpsAccountNumberRestriction testObject= new UpsAccountNumberRestriction();

            Assert.Equal(EditionFeature.UpsAccountNumbers, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsEditionRestrictionLevelNone_WhenDataIsEmptyString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                UpsAccountNumberRestriction testObject = new UpsAccountNumberRestriction();

                Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, string.Empty));
            }
        }

        [Fact]
        public void Check_ReturnsEditionRestrictionLevelNone_WhenDataIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                UpsAccountNumberRestriction testObject = new UpsAccountNumberRestriction();

                Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
            }
        }

        [Fact]
        public void Check_ReturnsEditionRestrictionLevelNone_WhenDataIsWhitespace()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                UpsAccountNumberRestriction testObject = new UpsAccountNumberRestriction();

                Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, "     "));
            }
        }

        [Fact]
        public void Check_ReturnsEditionRestrictionLevelNone_WhenLicenseCapabilitiesUpsAccountNumbersIsEmpty()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(l => l.UpsAccountNumbers).Returns(new string[] {});
                UpsAccountNumberRestriction testObject = new UpsAccountNumberRestriction();

                Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, "abcdefg"));
            }
        }

        [Fact]
        public void Check_ReturnsEditionRestrictionLevelNone_WhenLicenseCapabilitiesContainsUpsAccount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(l => l.UpsAccountNumbers).Returns(new[] {"abcdefg"});
                UpsAccountNumberRestriction testObject = new UpsAccountNumberRestriction();

                Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, "abcdefg"));
            }
        }

        [Fact]
        public void Check_ReturnsEditionRestrictionLevelForbidden_WhenLicenseCapabilitiesDoesNotContainsUpsAccount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(l => l.UpsAccountNumbers).Returns(new[] { "123456" });
                UpsAccountNumberRestriction testObject = new UpsAccountNumberRestriction();

                Assert.Equal(EditionRestrictionLevel.Forbidden, testObject.Check(licenseCapabilities.Object, "abcdefg"));
            }
        }
    }
}