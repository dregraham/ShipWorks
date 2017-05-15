﻿using System;
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveDataToStoreFromViewModel_WithInvalidOauthClientID_CausesError(string clientID)
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.OAuthClientID).Returns(clientID);
            viewModel.SetupGet(x => x.OAuthSecretKey).Returns("Password");

            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new ShopSiteStoreEntity(), viewModel.Object);

            Assert.False(result.Success);
            Assert.Equal("Please enter the Client ID for your ShopSite store.", result.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveDataToStoreFromViewModel_WithInvalidOauthToken_CausesError(string oauthToken)
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.OAuthClientID).Returns("Username");
            viewModel.SetupGet(x => x.OAuthSecretKey).Returns(oauthToken);

            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new ShopSiteStoreEntity(), viewModel.Object);

            Assert.False(result.Success);
            Assert.Equal("Please enter an OAuth Token.", result.Message);
        }


        [Theory]
        [InlineData("bar")]
        [InlineData(" bar")]
        [InlineData("bar ")]
        [InlineData(" bar ")]
        public void SaveDataToStoreFromViewModel_WithValidOauthClientID_SetsClientIDOnStore(string oauthClientID)
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.OAuthClientID).Returns(oauthClientID);
            viewModel.SetupGet(x => x.OAuthSecretKey).Returns("foo");

            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new ShopSiteStoreEntity(), viewModel.Object);

            Assert.True(result.Success);
            Assert.Equal("bar", result.Value.OauthClientID);
        }

        [Theory]
        [InlineData("bar")]
        [InlineData(" bar")]
        [InlineData("bar ")]
        [InlineData(" bar ")]
        public void SaveDataToStoreFromViewModel_WithValidOauthToken_SetsTokenOnStore(string oauthToken)
        {
            var viewModel = mock.CreateMock<IShopSiteAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.OAuthClientID).Returns("foo");
            viewModel.SetupGet(x => x.OAuthSecretKey).Returns(oauthToken);

            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new ShopSiteStoreEntity(), viewModel.Object);

            Assert.True(result.Success);
            Assert.Equal("bar", result.Value.OauthSecretKey);
        }

        [Fact]
        public void ConnectionVerificationNeeded_ReturnsFalse_WhenBasicDataHasNotChanged()
        {
            var store = CreateShopSiteStore(clientID: "foo", token: "bar");

            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.ConnectionVerificationNeeded(store);

            Assert.False(result);
        }

        [Theory]
        [InlineData(ShopSiteStoreFieldIndex.OauthClientID)]
        [InlineData(ShopSiteStoreFieldIndex.OauthSecretKey)]
        public void ConnectionVerificationNeeded_ReturnsTrue_WhenBasicDataHasChanged(ShopSiteStoreFieldIndex changedField)
        {
            var store = CreateShopSiteStore(clientID: "foo", token: "bar");
            store.Fields[(int)changedField].IsChanged = true;

            var testObject = mock.Create<ShopSiteOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.ConnectionVerificationNeeded(store);

            Assert.True(result);
        }

        /// <summary>
        /// Create a ShopSite store that doesn't look modified
        /// </summary>
        private ShopSiteStoreEntity CreateShopSiteStore(string clientID, string token)
        {
            var store = new ShopSiteStoreEntity()
            {
                OauthClientID = clientID,
                OauthSecretKey = token
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
