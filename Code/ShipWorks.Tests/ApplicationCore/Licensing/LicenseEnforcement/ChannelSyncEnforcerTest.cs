using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.LicenseEnforcement
{
    public class ChannelSyncEnforcerTest
    {
        [Fact]
        public void Priority_ReturnsHeigh()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();

                Assert.Equal(EnforcementPriority.High, testObject.Priority);
            }
        }

        [Fact]
        public void EditionFeature_ReturnsChannelCount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();

                Assert.Equal(EditionFeature.ChannelCount, testObject.EditionFeature);
            }
        }

        [Fact]
        public void Enforce_GetsListOfActiveStoresFromLicense_WhenEnforcementContextIsLogin()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IActiveStore> activeStore = mock.Mock<IActiveStore>();

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores()).Returns(new[] {activeStore.Object});

                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(l => l.GetLicenses()).Returns(new[] {license.Object});
                
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();
                

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login);

                license.Verify(l => l.GetActiveStores(), Times.AtLeastOnce);
            }
        }
    }
}