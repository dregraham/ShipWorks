using System;
using System.Linq;
using Autofac.Extras.Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite.AccountSettings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ShopSite
{
    public class ShopSiteOauthAuthenticationPersistenceStrategyTest : IDisposable
    {
        readonly AutoMock mock;

        public ShopSiteOauthAuthenticationPersistenceStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void LoadStoreIntoViewModel_Throws_WhenStoreIsNull()
        {
            var viewModel = mock.Create<IShopSiteAccountSettingsViewModel>();
            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.LoadStoreIntoViewModel(null, viewModel));
        }

        [Fact]
        public void LoadStoreIntoViewModel_Throws_WhenViewModelIsNull()
        {
            var store = mock.Create<IShopSiteStoreEntity>();
            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.LoadStoreIntoViewModel(store, null));
        }

        [Fact]
        public void LoadStoreIntoViewModel_LoadOauthAuthenticationInformation_IntoViewModel()
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            var store = mock.CreateMock<IShopSiteStoreEntity>();
            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();

            store.SetupGet(x => x.OauthClientID).Returns("Username");
            store.SetupGet(x => x.OauthSecretKey).Returns("Password");

            testObject.LoadStoreIntoViewModel(store.Object, viewModel.Object);

            viewModel.VerifySet(x => x.OAuthClientID = "Username");
            viewModel.VerifySet(x => x.OAuthSecretKey = "Password");
        }

        [Fact]
        public void LoadStoreIntoViewModel_ClearsBasicData_FromViewModel()
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            var store = mock.CreateMock<IShopSiteStoreEntity>();
            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();

            testObject.LoadStoreIntoViewModel(store.Object, viewModel.Object);
            viewModel.VerifySet(x => x.LegacyMerchantID = string.Empty);
            viewModel.VerifySet(x => x.LegacyPassword = string.Empty);
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_Throws_WhenStoreIsNull()
        {
            var viewModel = mock.Create<IShopSiteAccountSettingsViewModel>();
            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();

            Assert.Throws<ArgumentNullException>(() => testObject.SaveDataToStoreFromViewModel(null, viewModel));
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_Throws_WhenViewModelIsNull()
        {
            var store = new ShopSiteStoreEntity();
            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();

            Assert.Throws<ArgumentNullException>(() => testObject.SaveDataToStoreFromViewModel(store, null));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
