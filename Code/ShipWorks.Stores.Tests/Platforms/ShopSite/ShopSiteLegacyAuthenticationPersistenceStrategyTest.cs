using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Stores.Platforms.ShopSite.AccountSettings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ShopSite.AccountSettings
{
    public class ShopSiteLegacyAuthenticationPersistenceStrategyTest : IDisposable
    {
        readonly AutoMock mock;

        public ShopSiteLegacyAuthenticationPersistenceStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void LoadStoreIntoViewModel_Throws_WhenStoreIsNull()
        {
            var viewModel = mock.Create<IShopSiteAccountSettingsViewModel>();
            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.LoadStoreIntoViewModel(null, viewModel));
        }

        [Fact]
        public void LoadStoreIntoViewModel_Throws_WhenViewModelIsNull()
        {
            var store = mock.Create<IShopSiteStoreEntity>();
            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.LoadStoreIntoViewModel(store, null));
        }

        [Fact]
        public void LoadStoreIntoViewModel_DelegatesToEncryptionProvider_ToDecryptPassword()
        {
            mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateSecureTextEncryptionProvider("foo"))
                .Setup(x => x.Decrypt("bar"))
                .Verifiable();

            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            var store = mock.CreateMock<IShopSiteStoreEntity>();
            store.SetupGet(x => x.Username).Returns("foo");
            store.SetupGet(x => x.Password).Returns("bar");

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();
            testObject.LoadStoreIntoViewModel(store.Object, viewModel.Object);

            mock.VerifyAll = true;
        }

        [Fact]
        public void LoadStoreIntoViewModel_CopiesBasicAuthenticationInformation_IntoViewModel()
        {
            mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateSecureTextEncryptionProvider(It.IsAny<string>()))
                .Setup(x => x.Decrypt(It.IsAny<string>()))
                .Returns("bar");

            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            var store = mock.CreateMock<IShopSiteStoreEntity>();
            store.SetupGet(x => x.Username).Returns("foo");
            store.SetupGet(x => x.RequireSSL).Returns(false);

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();
            testObject.LoadStoreIntoViewModel(store.Object, viewModel.Object);

            viewModel.VerifySet(x => x.LegacyMerchantID = "foo");
            viewModel.VerifySet(x => x.LegacyPassword = "bar");
            viewModel.VerifySet(x => x.LegacyUseUnsecureHttp = true);
        }

        [Fact]
        public void LoadStoreIntoViewModel_ClearsOAuthData_FromViewModel()
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            var store = mock.CreateMock<IShopSiteStoreEntity>();

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();
            testObject.LoadStoreIntoViewModel(store.Object, viewModel.Object);

            viewModel.VerifySet(x => x.OAuthClientID = string.Empty);
            viewModel.VerifySet(x => x.OAuthSecretKey = string.Empty);
            viewModel.VerifySet(x => x.OAuthAuthorizationCode = string.Empty);
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_Throws_WhenStoreIsNull()
        {
            var viewModel = mock.Create<IShopSiteAccountSettingsViewModel>();
            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.SaveDataToStoreFromViewModel(null, viewModel));
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_Throws_WhenViewModelIsNull()
        {
            var store = new ShopSiteStoreEntity();
            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.SaveDataToStoreFromViewModel(store, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveDataToStoreFromViewModel_WithInvalidApiUserName_CausesError(string apiUsername)
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.LegacyMerchantID).Returns(apiUsername);
            viewModel.SetupGet(x => x.LegacyPassword).Returns("foo");

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new ShopSiteStoreEntity(), viewModel.Object);

            Assert.False(result.Success);
            Assert.Equal("Please enter a username for your ShopSite Store.", result.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveDataToStoreFromViewModel_WithInvalidApiToken_CausesError(string apiToken)
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.LegacyMerchantID).Returns("foo");
            viewModel.SetupGet(x => x.LegacyPassword).Returns(apiToken);

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new ShopSiteStoreEntity(), viewModel.Object);

            Assert.False(result.Success);
            Assert.Equal("Please enter a password for your ShopSite Store.", result.Message);
        }

        [Theory]
        [InlineData("bar")]
        [InlineData(" bar")]
        [InlineData("bar ")]
        [InlineData(" bar ")]
        public void SaveDataToStoreFromViewModel_WithValidApiUsername_SetsTokenOnStore(string apiUsername)
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.LegacyMerchantID).Returns(apiUsername);
            viewModel.SetupGet(x => x.LegacyPassword).Returns("foo");

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new ShopSiteStoreEntity(), viewModel.Object);

            Assert.True(result.Success);
            Assert.Equal("bar", result.Value.Username);
        }

        [Theory]
        [InlineData("bar")]
        [InlineData(" bar")]
        [InlineData("bar ")]
        [InlineData(" bar ")]
        public void SaveDataToStoreFromViewModel_DelegatesToEncryptionProvider_ToEncryptPassword(string apiToken)
        {
            mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateSecureTextEncryptionProvider("foo"))
                .Setup(x => x.Encrypt("bar"))
                .Verifiable();

            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.LegacyMerchantID).Returns("foo");
            viewModel.SetupGet(x => x.LegacyPassword).Returns(apiToken);

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            testObject.SaveDataToStoreFromViewModel(new ShopSiteStoreEntity(), viewModel.Object);

            mock.VerifyAll = true;
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_WithValidApiToken_SetsTokenOnStore()
        {
            mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateSecureTextEncryptionProvider(It.IsAny<string>()))
                .Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns("bar");

            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.LegacyMerchantID).Returns("foo");
            viewModel.SetupGet(x => x.LegacyPassword).Returns("baz");

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new ShopSiteStoreEntity(), viewModel.Object);

            Assert.True(result.Success);
            Assert.Equal("bar", result.Value.Password);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void SaveDataToStoreFromViewModel_SetsRequireSSL(bool useUnsecureConnection, bool requireSSL)
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.LegacyMerchantID).Returns("bar");
            viewModel.SetupGet(x => x.LegacyPassword).Returns("foo");
            viewModel.SetupGet(x => x.LegacyUseUnsecureHttp).Returns(useUnsecureConnection);

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new ShopSiteStoreEntity(), viewModel.Object);

            Assert.True(result.Success);
            Assert.Equal(requireSSL, result.Value.RequireSSL);
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_WithValidBasicData_ClearsOauthData()
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.LegacyMerchantID).Returns("foo");
            viewModel.SetupGet(x => x.LegacyPassword).Returns("bar");

            var store = new ShopSiteStoreEntity
            {
                OauthClientID = "baz",
                OauthSecretKey = "quux"
            };

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(store, viewModel.Object);

            Assert.Equal(string.Empty, result.Value.OauthClientID);
            Assert.Equal(string.Empty, result.Value.OauthSecretKey);
            Assert.Equal(string.Empty, result.Value.OauthAuthorizationCode);
        }

        [Fact]
        public void ConnectionVerificationNeeded_ReturnsFalse_WhenBasicDataHasNotChanged()
        {
            var store = CreateShopSiteStore(username: "foo", token: "bar");

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            var result = testObject.ConnectionVerificationNeeded(store);

            Assert.False(result);
        }

        [Theory]
        [InlineData(ShopSiteStoreFieldIndex.Username)]
        [InlineData(ShopSiteStoreFieldIndex.Password)]
        public void ConnectionVerificationNeeded_ReturnsTrue_WhenBasicDataHasChanged(ShopSiteStoreFieldIndex changedField)
        {
            var store = CreateShopSiteStore(username: "foo", token: "bar");
            store.Fields[(int) changedField].IsChanged = true;

            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            var result = testObject.ConnectionVerificationNeeded(store);

            Assert.True(result);
        }

        [Fact]
        public void ValidateApiUrl_ReturnsFailure_WhenUrlDoesNotEndWithCorrectFile()
        {
            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            var result = testObject.ValidateApiUrl("http://www.google.com/");

            Assert.True(result.Failure);
        }

        [Fact]
        public void ValidateApiUrl_ReturnsSuccess_WhenUrlEndsWithCorrectFile()
        {
            var testObject = mock.Create<ShopSiteLegacyAuthenticationPersistenceStrategy>();

            var result = testObject.ValidateApiUrl("http://www.google.com/db_xml.cgi");

            Assert.True(result.Success);
        }

        /// <summary>
        /// Create a ShopSite store that doesn't look modified
        /// </summary>
        private ShopSiteStoreEntity CreateShopSiteStore(string username, string token)
        {
            var store = new ShopSiteStoreEntity()
            {
                Username = username,
                Password = token
            };

            store.IsDirty = false;
            foreach (var field in store.Fields.OfType<IEntityField2>())
            {
                field.IsChanged = false;
            }

            return store;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
