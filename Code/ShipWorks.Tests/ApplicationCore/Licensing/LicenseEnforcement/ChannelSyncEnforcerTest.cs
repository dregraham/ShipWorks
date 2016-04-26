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
        public void Priority_ReturnsHigh()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();

                Assert.Equal(EnforcementPriority.Medium, testObject.Priority);
            }
        }

        [Fact]
        public void EditionFeature_ReturnsChannelCount()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();

                Assert.Equal(EditionFeature.ClientChannelsAccountedFor, testObject.EditionFeature);
            }
        }

        [Fact]
        public void Enforce_GetsListOfActiveStoresFromLicense_WhenEnforcementContextIsLogin()
        {
            using (AutoMock mock = AutoMock.GetLoose())
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
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Mock<IStoreManager> storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(s => s.GetAllStores())
                    .Returns(new[]
                    {new StoreEntity {License = "foo", TypeCode = 9}, new StoreEntity {License = "bar", TypeCode = 8}});

                Mock<IActiveStore> activeStore = mock.Mock<IActiveStore>();
                activeStore.SetupGet(a => a.StoreLicenseKey).Returns("abcdefghijklmnopqrstuvwxyz");

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores()).Returns(new[] {activeStore.Object});
                license.Setup(l => l.Activate(It.IsAny<StoreEntity>()))
                    .Returns(new EnumResult<LicenseActivationState>(LicenseActivationState.MaxChannelsExceeded,
                        EnumHelper.GetDescription(LicenseActivationState.MaxChannelsExceeded)));

                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(l => l.GetLicenses()).Returns(new[] {license.Object});

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();
                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object,
                    EnforcementContext.Login);

                Assert.Equal(ComplianceLevel.NotCompliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_ReturnsCompliant_WhenUnknownStoreLicenseSuccessfullyAdded()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Mock<IStoreManager> storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(s => s.GetAllStores())
                    .Returns(new[]
                    {new StoreEntity {License = "foo", TypeCode = 9}, new StoreEntity {License = "bar", TypeCode = 8}});

                Mock<IActiveStore> activeStore = mock.Mock<IActiveStore>();
                activeStore.SetupGet(a => a.StoreLicenseKey).Returns("abcdefghijklmnopqrstuvwxyz");

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores()).Returns(new[] {activeStore.Object});
                license.Setup(l => l.Activate(It.IsAny<StoreEntity>()))
                    .Returns(new EnumResult<LicenseActivationState>(LicenseActivationState.Active));

                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(l => l.GetLicenses()).Returns(new[] {license.Object});

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();
                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object,
                    EnforcementContext.Login);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_SavesAddedStoreWithCorrectLicenseKey_WhenUnknownStoreLicenseSuccessfullyAdded()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Mock<IStoreManager> storeManager = mock.Mock<IStoreManager>();
                StoreEntity store1 = new StoreEntity {License = "foo", TypeCode = 9};
                StoreEntity store2 = new StoreEntity {License = "bar", TypeCode = 8};

                storeManager.Setup(s => s.GetAllStores())
                    .Returns(new[]
                    {store1, store2});

                Mock<IActiveStore> activeStore = mock.Mock<IActiveStore>();
                activeStore.SetupGet(a => a.StoreLicenseKey).Returns("abcdefghijklmnopqrstuvwxyz");

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores()).Returns(new[] {activeStore.Object});
                license.Setup(l => l.Activate(It.IsAny<StoreEntity>()))
                    .Returns(
                        (StoreEntity store) => new EnumResult<LicenseActivationState>(LicenseActivationState.Active));


                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(l => l.GetLicenses()).Returns(new[] {license.Object});

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();
                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login);

                storeManager.Verify(m => m.SaveStore(store1), Times.Once);
                storeManager.Verify(m => m.SaveStore(store2), Times.Once);
            }
        }

        [Fact]
        public void Enforce_ReturnsMessage_WhenNotCompliant()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Mock<IStoreManager> storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(s => s.GetAllStores())
                    .Returns(new[]
                    {new StoreEntity {License = "foo", TypeCode = 9}, new StoreEntity {License = "bar", TypeCode = 8}});

                Mock<IActiveStore> activeStore = mock.Mock<IActiveStore>();
                activeStore.SetupGet(a => a.StoreLicenseKey).Returns("abcdefghijklmnopqrstuvwxyz");

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores()).Returns(new[] {activeStore.Object});
                license.Setup(l => l.Activate(It.IsAny<StoreEntity>()))
                    .Returns(new EnumResult<LicenseActivationState>(LicenseActivationState.MaxChannelsExceeded,
                        EnumHelper.GetDescription(LicenseActivationState.MaxChannelsExceeded)));

                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(l => l.GetLicenses()).Returns(new[] {license.Object});

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                ChannelSyncEnforcer testObject = mock.Create<ChannelSyncEnforcer>();

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object,
                    EnforcementContext.Login);

                Assert.Equal(EnumHelper.GetDescription(EditionFeature.ClientChannelsAccountedFor), result.Message);
            }
        }
    }
}