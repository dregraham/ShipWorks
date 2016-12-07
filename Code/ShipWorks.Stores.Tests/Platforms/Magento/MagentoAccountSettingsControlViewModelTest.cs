using System;
using Autofac.Extras.Moq;
using ShipWorks.Tests.Shared;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.Enums;
using Autofac.Features.Indexed;
using Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.UI.Platforms.Magento;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Tests.Platforms.Magento
{
    public class MagentoAccountSettingsControlViewModelTest : IDisposable
    {
        readonly AutoMock mock;
        readonly MagentoStoreEntity store;
        readonly MagentoAccountSettingsControlViewModel testObject;
        readonly Mock<IIndex<MagentoVersion, IMagentoProbe>> probeIIndex;
        readonly Mock<IEncryptionProvider> encryptionProvider;
        readonly string decryptedPassword = "descryptedpassword";

        const string defaultUrl = "http://www.shipworks.com";
        const string defaultPassword = "defaultPassword";
        const string defaultUsername = "defaultUsername";
        
        public MagentoAccountSettingsControlViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            store = new MagentoStoreEntity {StoreTypeCode = StoreTypeCode.Magento};

            probeIIndex = mock.CreateMock<IIndex<MagentoVersion, IMagentoProbe>>();
            mock.Provide(probeIIndex.Object);

            encryptionProvider = mock.MockRepository.Create<IEncryptionProvider>();
            encryptionProvider.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(decryptedPassword);
            mock.Mock<IEncryptionProviderFactory>()
                .Setup(f => f.CreateSecureTextEncryptionProvider(It.IsAny<string>()))
                .Returns(encryptionProvider.Object);

            testObject = mock.Create<MagentoAccountSettingsControlViewModel>();
            testObject.Username = defaultUsername;
            testObject.StoreUrl = defaultUrl;
            testObject.Password = defaultPassword.ToSecureString();
        }

        [Fact]
        public void Load_SetsCorrectMagentoVersion()
        {
            var version = MagentoVersion.MagentoTwoREST;
            store.MagentoVersion = (int) version;
            testObject.Load(store);

            Assert.Equal(version, testObject.MagentoVersion);
        }

        [Fact]
        public void Load_SetsCorrectUsername()
        {
            string username = "my username";
            store.ModuleUsername = username;
            testObject.Load(store);

            Assert.Equal(username, testObject.Username);
        }

        [Fact]
        public void Load_InvokesCreateSecureTextEncryptionProvider_WithUsername()
        {
            string username = "my username";
            store.ModuleUsername = username;
            testObject.Load(store);

            mock.Mock<IEncryptionProviderFactory>()
                .Verify(f => f.CreateSecureTextEncryptionProvider(username), Times.Once);
        }

        [Fact]
        public void Load_InvokesDecrypt_WithStorePassword()
        {
            string encryptedPassword = "my encrypted password";
            store.ModulePassword = encryptedPassword;
            testObject.Load(store);

            encryptionProvider.Verify(p => p.Decrypt(encryptedPassword), Times.Once);
        }

        [Fact]
        public void Load_PasswordSetCorrectly_FromDecryptedString()
        {
            testObject.Load(store);

            Assert.Equal(decryptedPassword, testObject.Password.ToInsecureString());
        }

        [Fact]
        public void Load_StoreUrlSetCorrectly()
        {
            string url = "http://www.bloop.com";
            store.ModuleUrl = url;

            testObject.Load(store);

            Assert.Equal(url, testObject.StoreUrl);
        }

        [Fact]
        public void Load_StoreCodeSetCorrectly()
        {
            string storeCode = "A";
            store.ModuleOnlineStoreCode = storeCode;

            testObject.Load(store);

            Assert.Equal(storeCode, testObject.StoreCode);
        }

        [Fact]
        public void Save_SetsStoreMagentoVersion()
        {
            var magentoVersion = MagentoVersion.MagentoTwoREST;
            SetupMagentoVersion(magentoVersion, true);

            testObject.Save(store);

            Assert.Equal(magentoVersion, (MagentoVersion) store.MagentoVersion);
        }

        [Fact]
        public void Save_SetsStoreUsername()
        {
            string userName = "random name";
            SetupMagentoVersion(MagentoVersion.MagentoTwoREST, true);
            testObject.Username = userName;

            testObject.Save(store);
            Assert.Equal(userName, store.ModuleUsername);
        }

        [Fact]
        public void Save_InvokesCreateSecureTextEncryptionProviderWithUsername()
        {
            string username = "my username";
            SetupMagentoVersion(MagentoVersion.MagentoTwoREST, true);

            testObject.Username = username;
            testObject.Save(store);

            mock.Mock<IEncryptionProviderFactory>()
                .Verify(f => f.CreateSecureTextEncryptionProvider(username), Times.Once);
        }

        [Fact]
        public void Save_InvokesEncrypt_WithStorePassword()
        {
            string plainTextPassword = "my encrypted password";
            SetupMagentoVersion(MagentoVersion.MagentoTwoREST, true);

            testObject.Password = plainTextPassword.ToSecureString();
            testObject.Save(store);

            encryptionProvider.Verify(p => p.Encrypt(plainTextPassword), Times.Once);
        }

        [Fact]
        public void Save_StorePasswordSetToEncryptedString()
        {
            string plainTextPassword = "my encrypted password";
            SetupMagentoVersion(MagentoVersion.MagentoTwoREST, true);

            encryptionProvider.Setup(p => p.Encrypt(It.IsAny<string>())).Returns(plainTextPassword);
            testObject.Save(store);

            Assert.Equal(plainTextPassword, store.ModulePassword);
        }

        [Fact]
        public void Save_StoreModuleUrlSet()
        {
            string url = "http://www.google.com/";

            testObject.StoreUrl = url;
            SetupMagentoVersion(MagentoVersion.MagentoTwoREST, true, url);

            testObject.Save(store);

            Assert.Equal(url, store.ModuleUrl);
        }
        
        [Fact]
        public void Save_StoreModuleOnlineStoreCodeSet()
        {
            string storeCode = "f";

            testObject.StoreCode = storeCode;
            SetupMagentoVersion(MagentoVersion.MagentoTwoREST, true);

            testObject.Save(store);

            Assert.Equal(storeCode, store.ModuleOnlineStoreCode);
        }

        [Fact]
        public void Save_ReturnsInvalidUrl_WhenUrlNotInValidFormat()
        {
            SetupMagentoVersion(MagentoVersion.MagentoTwoREST, true);
            testObject.StoreUrl = "badurl";

            GenericResult<MagentoStoreEntity> genericResult = testObject.Save(store);
            Assert.False(genericResult.Success);
            Assert.Equal(MagentoAccountSettingsControlViewModel.UrlNotInValidFormat, genericResult.Message);            
        }

        [Fact]
        public void Save_ReturnsFailure_WhenProbeUnsuccessful()
        {
            SetupMagentoVersion(MagentoVersion.MagentoTwoREST, false);

            GenericResult<MagentoStoreEntity> genericResult = testObject.Save(store);
            Assert.False(genericResult.Success);
            Assert.Equal(MagentoAccountSettingsControlViewModel.CouldNotConnect, genericResult.Message);
        }

        [Fact]
        public void Save_ReturnsFailure_WhenUrlDoesNotMatchDiscoveredUrl()
        {
            SetupMagentoVersion(MagentoVersion.MagentoTwoREST, true, defaultUrl);
            testObject.StoreUrl = "http://www.yahoo.com";

            GenericResult<MagentoStoreEntity> genericResult = testObject.Save(store);

            Assert.False(genericResult.Success);
            Assert.StartsWith(MagentoAccountSettingsControlViewModel.UrlDoesntMatchProbe, genericResult.Message);
            Assert.Contains(defaultUrl, genericResult.Message);
        }

        private void SetupMagentoVersion(MagentoVersion magentoVersion, bool success, string url = defaultUrl)
        {
            testObject.MagentoVersion = magentoVersion;
            GenericResult<Uri> result = success ? GenericResult.FromSuccess(new Uri(url)) : GenericResult.FromError<Uri>("error");

            var myProbe = mock.MockRepository.Create<IMagentoProbe>();
            myProbe.Setup(p => p.FindCompatibleUrl(store)).Returns(result);

            probeIIndex.Setup(i => i[magentoVersion]).Returns(myProbe.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}

