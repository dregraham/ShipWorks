using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores;
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

        [Fact]
        public void Enforce_ReturnsNotCompliant_WhenLicenseReturnsLicenseActivationStateMaxChannelsExceeded()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IStoreManager> storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(s => s.GetAllStores())
                    .Returns(new[]
                    {new StoreEntity {License = "foo", TypeCode = 9}, new StoreEntity {License = "bar", TypeCode = 8}});

                Mock<IActiveStore> activeStore = mock.Mock<IActiveStore>();
                activeStore.SetupGet(a => a.StoreLicenseKey).Returns("abcdefghijklmnopqrstuvwxyz");

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores()).Returns(new[] { activeStore.Object });
                license.Setup(l => l.Activate(It.IsAny<StoreEntity>()))
                    .Returns(new EnumResult<LicenseActivationState>(LicenseActivationState.MaxChannelsExceeded,
                        EnumHelper.GetDescription(LicenseActivationState.MaxChannelsExceeded)));

                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(l => l.GetLicenses()).Returns(new[] { license.Object });

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login);

                Assert.Equal(ComplianceLevel.NotCompliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_ReturnsMessage_WhenNotCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IStoreManager> storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(s => s.GetAllStores())
                    .Returns(new[]
                    {new StoreEntity {License = "foo", TypeCode = 9}, new StoreEntity {License = "bar", TypeCode = 8}});

                Mock<IActiveStore> activeStore = mock.Mock<IActiveStore>();
                activeStore.SetupGet(a => a.StoreLicenseKey).Returns("abcdefghijklmnopqrstuvwxyz");

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores()).Returns(new[] { activeStore.Object });
                license.Setup(l => l.Activate(It.IsAny<StoreEntity>()))
                    .Returns(new EnumResult<LicenseActivationState>(LicenseActivationState.MaxChannelsExceeded,
                        EnumHelper.GetDescription(LicenseActivationState.MaxChannelsExceeded)));

                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(l => l.GetLicenses()).Returns(new[] { license.Object });

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login);

                Assert.Equal("You have exceeded your channel limit. Please delete 2 channel(s).", result.Message);
            }
        }
    }
}