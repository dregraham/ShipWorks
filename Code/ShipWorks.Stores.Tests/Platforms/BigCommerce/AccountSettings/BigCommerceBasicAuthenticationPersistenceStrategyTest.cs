using System;
using System.Linq;
using Autofac.Extras.Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.BigCommerce.AccountSettings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce.AccountSettings
{
    public class BigCommerceBasicAuthenticationPersistenceStrategyTest : IDisposable
    {
        readonly AutoMock mock;

        public BigCommerceBasicAuthenticationPersistenceStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void LoadStoreIntoViewModel_Throws_WhenStoreIsNull()
        {
            var viewModel = mock.Create<IBigCommerceAccountSettingsViewModel>();
            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.LoadStoreIntoViewModel(null, viewModel));
        }

        [Fact]
        public void LoadStoreIntoViewModel_Throws_WhenViewModelIsNull()
        {
            var store = mock.Create<IBigCommerceStoreEntity>();
            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.LoadStoreIntoViewModel(store, null));
        }

        [Fact]
        public void LoadStoreIntoViewModel_CopiesBasicAuthenticationInformation_IntoViewModel()
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            var store = mock.CreateMock<IBigCommerceStoreEntity>();
            store.SetupGet(x => x.ApiUserName).Returns("foo");
            store.SetupGet(x => x.ApiToken).Returns("bar");

            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();
            testObject.LoadStoreIntoViewModel(store.Object, viewModel.Object);

            viewModel.VerifySet(x => x.BasicUsername = "foo");
            viewModel.VerifySet(x => x.BasicToken = "bar");
        }

        [Fact]
        public void LoadStoreIntoViewModel_ClearsOAuthData_FromViewModel()
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            var store = mock.CreateMock<IBigCommerceStoreEntity>();

            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();
            testObject.LoadStoreIntoViewModel(store.Object, viewModel.Object);

            viewModel.VerifySet(x => x.OauthClientID = string.Empty);
            viewModel.VerifySet(x => x.OauthToken = string.Empty);
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_Throws_WhenStoreIsNull()
        {
            var viewModel = mock.Create<IBigCommerceAccountSettingsViewModel>();
            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.SaveDataToStoreFromViewModel(null, viewModel));
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_Throws_WhenViewModelIsNull()
        {
            var store = new BigCommerceStoreEntity();
            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();
            Assert.Throws<ArgumentNullException>(() => testObject.SaveDataToStoreFromViewModel(store, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveDataToStoreFromViewModel_WithInvalidApiUserName_CausesError(string apiUsername)
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.BasicUsername).Returns(apiUsername);
            viewModel.SetupGet(x => x.BasicToken).Returns("foo");

            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new BigCommerceStoreEntity(), viewModel.Object);

            Assert.False(result.Success);
            Assert.Equal("Please enter the API Username for your BigCommerce store.", result.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveDataToStoreFromViewModel_WithInvalidApiToken_CausesError(string apiToken)
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.BasicUsername).Returns("foo");
            viewModel.SetupGet(x => x.BasicToken).Returns(apiToken);

            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new BigCommerceStoreEntity(), viewModel.Object);

            Assert.False(result.Success);
            Assert.Equal("Please enter an API Token.", result.Message);
        }

        [Theory]
        [InlineData("bar")]
        [InlineData(" bar")]
        [InlineData("bar ")]
        [InlineData(" bar ")]
        public void SaveDataToStoreFromViewModel_WithValidApiUsername_SetsTokenOnStore(string apiUsername)
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.BasicUsername).Returns(apiUsername);
            viewModel.SetupGet(x => x.BasicToken).Returns("foo");

            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new BigCommerceStoreEntity(), viewModel.Object);

            Assert.True(result.Success);
            Assert.Equal("bar", result.Value.ApiUserName);
        }

        [Theory]
        [InlineData("bar")]
        [InlineData(" bar")]
        [InlineData("bar ")]
        [InlineData(" bar ")]
        public void SaveDataToStoreFromViewModel_WithValidApiToken_SetsTokenOnStore(string apiToken)
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.BasicUsername).Returns("foo");
            viewModel.SetupGet(x => x.BasicToken).Returns(apiToken);

            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(new BigCommerceStoreEntity(), viewModel.Object);

            Assert.True(result.Success);
            Assert.Equal("bar", result.Value.ApiToken);
        }

        [Fact]
        public void SaveDataToStoreFromViewModel_WithValidBasicData_ClearsOauthData()
        {
            var viewModel = mock.CreateMock<IBigCommerceAccountSettingsViewModel>();
            viewModel.SetupGet(x => x.BasicUsername).Returns("foo");
            viewModel.SetupGet(x => x.BasicToken).Returns("bar");

            var store = new BigCommerceStoreEntity
            {
                OauthClientId = "baz",
                OauthToken = "quux"
            };

            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();

            var result = testObject.SaveDataToStoreFromViewModel(store, viewModel.Object);

            Assert.Equal(string.Empty, result.Value.OauthClientId);
            Assert.Equal(string.Empty, result.Value.OauthToken);
        }

        [Fact]
        public void ConnectionVerificationNeeded_ReturnsFalse_WhenBasicDataHasNotChanged()
        {
            var store = CreateBigCommerceStore(username: "foo", token: "bar");

            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();

            var result = testObject.ConnectionVerificationNeeded(store);

            Assert.False(result);
        }

        [Theory]
        [InlineData(BigCommerceStoreFieldIndex.ApiUserName)]
        [InlineData(BigCommerceStoreFieldIndex.ApiToken)]
        public void ConnectionVerificationNeeded_ReturnsTrue_WhenBasicDataHasChanged(BigCommerceStoreFieldIndex changedField)
        {
            var store = CreateBigCommerceStore(username: "foo", token: "bar");
            store.Fields[(int) changedField].IsChanged = true;

            var testObject = mock.Create<BigCommerceBasicAuthenticationPersisitenceStrategy>();

            var result = testObject.ConnectionVerificationNeeded(store);

            Assert.True(result);
        }

        /// <summary>
        /// Create a BigCommerce store that doesn't look modified
        /// </summary>
        private BigCommerceStoreEntity CreateBigCommerceStore(string username, string token)
        {
            var store = new BigCommerceStoreEntity()
            {
                ApiUserName = username,
                ApiToken = token
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
