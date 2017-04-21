using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.BigCommerce.AccountSettings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce.AccountSettings
{
    public class BigCommerceOauthAuthenticationPersistenceStrategyTest : IDisposable
    {
        readonly AutoMock mock;

        public BigCommerceOauthAuthenticationPersistenceStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void LoadStoreIntoViewModel_Throws_WhenStoreIsNull()
        {
            var viewModel = mock.Create<IBigCommerceAccountSettingsViewModel>();
            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.LoadStoreIntoViewModel(null, viewModel));
        }

        [Fact]
        public void LoadStoreIntoViewModel_Throws_WhenViewModelIsNull()
        {
            var store = mock.Create<IBigCommerceStoreEntity>();
            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.LoadStoreIntoViewModel(store, null));
        }

        [Fact]
        public void LoadStoreIntoViewModel_CopiesOauthAuthenticationInformation_IntoViewModel()
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            var store = mock.CreateMock<IBigCommerceStoreEntity>();
            store.SetupGet(x => x.OauthClientId).Returns("foo");
            store.SetupGet(x => x.OauthToken).Returns("bar");

            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();
            testObject.LoadStoreIntoViewModel(store.Object, viewModel.Object);

            viewModel.VerifySet(x => x.OauthClientID = "foo");
            viewModel.VerifySet(x => x.OauthToken = "bar");
        }

        [Fact]
        public void LoadStoreIntoViewModel_ClearsBasicData_FromViewModel()
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            var store = mock.CreateMock<IBigCommerceStoreEntity>();

            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();
            testObject.LoadStoreIntoViewModel(store.Object, viewModel.Object);

            viewModel.VerifySet(x => x.BasicUsername = string.Empty);
            viewModel.VerifySet(x => x.BasicToken = string.Empty);
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_Throws_WhenStoreIsNull()
        {
            var viewModel = mock.Create<IBigCommerceAccountSettingsViewModel>();
            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.SaveDataToStoreFromViewModel(null, viewModel));
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_Throws_WhenViewModelIsNull()
        {
            var store = new BigCommerceStoreEntity();
            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.SaveDataToStoreFromViewModel(store, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveDataToStoreFromViewModel_WithInvalidOauthClientID_CausesError(string clientID)
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.OauthClientID).Returns(clientID);
            viewModel.SetupGet(x => x.OauthToken).Returns("foo");

            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new BigCommerceStoreEntity(), viewModel.Object);

            Assert.False(result.Success);
            Assert.Equal("Please enter the Client ID for your BigCommerce store.", result.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveDataToStoreFromViewModel_WithInvalidOauthToken_CausesError(string oauthToken)
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.OauthClientID).Returns("foo");
            viewModel.SetupGet(x => x.OauthToken).Returns(oauthToken);

            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new BigCommerceStoreEntity(), viewModel.Object);

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
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.OauthClientID).Returns(oauthClientID);
            viewModel.SetupGet(x => x.OauthToken).Returns("foo");

            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new BigCommerceStoreEntity(), viewModel.Object);

            Assert.True(result.Success);
            Assert.Equal("bar", result.Value.OauthClientId);
        }

        [Theory]
        [InlineData("bar")]
        [InlineData(" bar")]
        [InlineData("bar ")]
        [InlineData(" bar ")]
        public void SaveDataToStoreFromViewModel_WithValidOauthToken_SetsTokenOnStore(string oauthToken)
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.OauthClientID).Returns("foo");
            viewModel.SetupGet(x => x.OauthToken).Returns(oauthToken);

            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new BigCommerceStoreEntity(), viewModel.Object);

            Assert.True(result.Success);
            Assert.Equal("bar", result.Value.OauthToken);
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_WithValidOauthData_ClearsBasicData()
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.OauthClientID).Returns("foo");
            viewModel.SetupGet(x => x.OauthToken).Returns("bar");

            var store = new BigCommerceStoreEntity
            {
                ApiUserName = "baz",
                ApiToken = "quux"
            };

            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(store, viewModel.Object);

            Assert.Equal(string.Empty, result.Value.ApiUserName);
            Assert.Equal(string.Empty, result.Value.ApiToken);
        }

        [Fact]
        public void ConnectionVerificationNeeded_ReturnsFalse_WhenBasicDataHasNotChanged()
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            var store = CreateBigCommerceStore(clientID: "foo", token: "bar");

            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.ConnectionVerificationNeeded(store);

            Assert.False(result);
        }

        [Theory]
        [InlineData(BigCommerceStoreFieldIndex.OauthClientId)]
        [InlineData(BigCommerceStoreFieldIndex.OauthToken)]
        public void ConnectionVerificationNeeded_ReturnsTrue_WhenBasicDataHasChanged(BigCommerceStoreFieldIndex changedField)
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            var store = CreateBigCommerceStore(clientID: "foo", token: "bar");
            store.Fields[(int) changedField].IsChanged = true;

            var testObject = mock.Create<BigCommerceOauthAuthenticationPersisitenceStrategy>();

            var result = testObject.ConnectionVerificationNeeded(store);

            Assert.True(result);
        }

        /// <summary>
        /// Create a BigCommerce store that doesn't look modified
        /// </summary>
        private BigCommerceStoreEntity CreateBigCommerceStore(string clientID, string token)
        {
            var store = new BigCommerceStoreEntity()
            {
                OauthClientId = clientID,
                OauthToken = token
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
