﻿using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.AccountSettings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce.AccountSettings
{
    public class BigCommerceAccountSettingsViewModelTest : IDisposable
    {
        readonly AutoMock mock;

        public BigCommerceAccountSettingsViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_SetsAuthenticationTypeToOauth()
        {
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            Assert.Equal(BigCommerceAuthenticationType.Oauth, testObject.AuthenticationType);
        }

        [Fact]
        public void LoadStore_SetsDataFromStore()
        {
            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            var store = mock.CreateMock<IBigCommerceStoreEntity>();
            store.SetupGet(x => x.ApiUrl).Returns("http://www.foo.com");
            store.SetupGet(x => x.BigCommerceAuthentication).Returns(BigCommerceAuthenticationType.Basic);
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store.Object);

            Assert.Equal("http://www.foo.com", testObject.ApiUrl);
            Assert.Equal(BigCommerceAuthenticationType.Basic, testObject.AuthenticationType);
        }

        [Fact]
        public void LoadStore_DelegatesToPersistenceStrategy()
        {
            var strategy = mock.CreateKeyedMockOf<IBigCommerceAuthenticationPersistenceStrategy>()
                .For(BigCommerceAuthenticationType.Oauth);

            var store = mock.CreateMock<IBigCommerceStoreEntity>();
            store.SetupGet(x => x.BigCommerceAuthentication).Returns(BigCommerceAuthenticationType.Oauth);
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store.Object);

            strategy.Verify(x => x.LoadStoreIntoViewModel(store.Object, testObject));
        }

        [Fact]
        public void LoadStore_ThrowsNullArgumentException_WhenStoreIsNull()
        {
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            Assert.Throws<ArgumentNullException>(() => testObject.LoadStore(null));
        }

        [Theory]
        [InlineData(null, "Please enter the API Path of your BigCommerce store.")]
        [InlineData("", "Please enter the API Path of your BigCommerce store.")]
        [InlineData("  ", "Please enter the API Path of your BigCommerce store.")]
        [InlineData("http://www.contoso.com/path???/file name", "The specified API Path is not a valid address.")]
        public async Task SaveToEntity_WithInvalidApiUrl_CausesError(string apiUrl, string errorMessage)
        {
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();
            testObject.ApiUrl = apiUrl;
            testObject.BasicUsername = "foo";
            testObject.BasicToken = "bar";

            var result = await testObject.SaveToEntity(new BigCommerceStoreEntity());
            Assert.False(result);
            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(errorMessage));
        }

        [Fact]
        public async Task SaveToEntity_DelegatesToPersistenceStrategy_WhenApiKeyIsValid()
        {
            var strategy = mock.CreateKeyedMockOf<IBigCommerceAuthenticationPersistenceStrategy>()
                .For(BigCommerceAuthenticationType.Basic);

            var store = new BigCommerceStoreEntity();
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = "http://www.example.com";

            await testObject.SaveToEntity(store);

            strategy.Verify(x => x.SaveDataToStoreFromViewModel(store, testObject));
        }

        [Fact]
        public async Task SaveToEntity_CausesError_WhenPersistenceStrategyFails()
        {
            mock.CreateKeyedMockOf<IBigCommerceAuthenticationPersistenceStrategy>()
                .For(BigCommerceAuthenticationType.Basic)
                .Setup(x => x.SaveDataToStoreFromViewModel(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<BigCommerceAccountSettingsViewModel>()))
                .Returns(GenericResult.FromError<BigCommerceStoreEntity>("Foo"));

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(new BigCommerceStoreEntity());
            testObject.ApiUrl = "http://www.example.com";

            var result = await testObject.SaveToEntity(new BigCommerceStoreEntity());

            Assert.False(result);
            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("Foo"));
        }

        [Theory]
        [InlineData(BigCommerceAuthenticationType.Oauth)]
        [InlineData(BigCommerceAuthenticationType.Basic)]
        public async Task SaveToEntity_DelegatesToIdentifier(BigCommerceAuthenticationType authenticationType)
        {
            CreateSuccessfulPersistenceStrategyFor(authenticationType);

            mock.Mock<IBigCommerceConnectionVerifier>()
                .Setup(x => x.Verify(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<IBigCommerceAuthenticationPersistenceStrategy>()))
                .ReturnsAsync(Result.FromSuccess());

            var store = new BigCommerceStoreEntity { BigCommerceAuthentication = authenticationType };
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.ApiUrl = "https://store-vplh1lw.mybigcommerce.com/api/v2/";

            await testObject.SaveToEntity(store).Recover(ex => false);

            mock.Mock<IBigCommerceIdentifier>().Verify(x => x.Set(store));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SaveToEntity_DelegatesToConnectionVerifier_AndReturnsResult(bool verifierResult)
        {
            var store = new BigCommerceStoreEntity();

            var strategy = CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            mock.Mock<IBigCommerceConnectionVerifier>()
                .Setup(x => x.Verify(store, strategy.Object))
                .ReturnsAsync(verifierResult ? GenericResult.FromSuccess(Unit.Default) : GenericResult.FromError<Unit>("Foo"));

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = "http://www.example.com";

            var result = await testObject.SaveToEntity(store);

            Assert.Equal(verifierResult, result);
        }

        [Fact]
        public async Task SaveToEntity_ShowsError_WhenVerifierFails()
        {
            var store = new BigCommerceStoreEntity();

            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            mock.Mock<IBigCommerceConnectionVerifier>()
                .Setup(x => x.Verify(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<IBigCommerceAuthenticationPersistenceStrategy>()))
                .ReturnsAsync(GenericResult.FromError<string>("Foo"));

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = "http://www.example.com";

            await testObject.SaveToEntity(store);

            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("Foo"));
        }

        [Fact]
        public async Task SaveToEntity_TogglesCursor_WhenVerifyingConnection()
        {
            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            mock.Mock<IBigCommerceConnectionVerifier>()
                .Setup(x => x.Verify(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<IBigCommerceAuthenticationPersistenceStrategy>()))
                .ReturnsAsync(GenericResult.FromSuccess(""));

            var disposable = mock.CreateMock<IDisposable>();
            mock.Mock<IMessageHelper>().Setup(x => x.SetCursor(Cursors.WaitCursor))
                .Returns(disposable);

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(new BigCommerceStoreEntity());
            testObject.ApiUrl = "http://www.example.com";

            await testObject.SaveToEntity(new BigCommerceStoreEntity());

            disposable.Verify(x => x.Dispose());
        }

        [Fact]
        public void MigrateToOauth_ChangesAuthenticationType_ToOauth()
        {
            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();
            testObject.LoadStore(new BigCommerceStoreEntity());

            testObject.MigrateToOauth.Execute(null);

            Assert.Equal(BigCommerceAuthenticationType.Oauth, testObject.AuthenticationType);
        }

        [Theory]
        [InlineData(" https://store-vplh1lw.mybigcommerce.com/api/v3/ ", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        [InlineData(" https://store-vplh1lw.mybigcommerce.com/api/v2/ ", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        [InlineData(" https://store-vplh1lw.mybigcommerce.com/api/v3 ", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        [InlineData(" https://store-vplh1lw.mybigcommerce.com/api/v2 ", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        [InlineData("http://store-vplh1lw.mybigcommerce.com/api/v2", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        public void MigrateToOauth_TranslatesApiUrl_WhenUrlIsBasicLegacy(string original, string expected)
        {
            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();
            testObject.LoadStore(new BigCommerceStoreEntity { ApiUrl = original });

            testObject.MigrateToOauth.Execute(null);

            Assert.Equal(expected, testObject.ApiUrl);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("http://www.example.com")]
        [InlineData("https://shipworks.mybigcommerce.com/api/v2/")]
        public void MigrateToOauth_SetsApiUrlToEmptyString_WhenUrlIsNotasicLegacy(string original)
        {
            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();
            testObject.LoadStore(new BigCommerceStoreEntity { ApiUrl = original });

            testObject.MigrateToOauth.Execute(null);

            Assert.Empty(testObject.ApiUrl);
        }

        [Theory]
        [InlineData("https://api.bigcommerce.com/stores/v3ixtef5a7/v3/", "https://api.bigcommerce.com/stores/v3ixtef5a7/v2/")]
        [InlineData("https://api.bigcommerce.com/stores/v3ixtef5a7/v3", "https://api.bigcommerce.com/stores/v3ixtef5a7/v2/")]
        public async Task SaveToEntity_SavesCorrectUrl(string original, string expected)
        {
            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Oauth);
            var store = new BigCommerceStoreEntity();
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();
            testObject.LoadStore(store);
            testObject.ApiUrl = original;

            await testObject.SaveToEntity(store);
            
            Assert.Equal(expected, store.ApiUrl);
        }

        [Fact]
        public async Task MigrateToOauth_DelegatesToOauthStrategy_AfterChange()
        {
            var builder = mock.CreateKeyedMockOf<IBigCommerceAuthenticationPersistenceStrategy>();
            var basicStrategy = builder.For(BigCommerceAuthenticationType.Basic);
            var oauthStrategy = builder.For(BigCommerceAuthenticationType.Oauth);

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();
            testObject.LoadStore(new BigCommerceStoreEntity());

            testObject.MigrateToOauth.Execute(null);
            testObject.ApiUrl = "https://api.bigcommerce.com/stores/vplh1lw/v3/";
            await testObject.SaveToEntity(new BigCommerceStoreEntity());

            basicStrategy.Verify(x => x.LoadStoreIntoViewModel(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<IBigCommerceAccountSettingsViewModel>()));
            oauthStrategy.Verify(x => x.LoadStoreIntoViewModel(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<IBigCommerceAccountSettingsViewModel>()), Times.Never);
            basicStrategy.Verify(x => x.SaveDataToStoreFromViewModel(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<BigCommerceAccountSettingsViewModel>()), Times.Never);
            oauthStrategy.Verify(x => x.SaveDataToStoreFromViewModel(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<BigCommerceAccountSettingsViewModel>()));
        }

        /// <summary>
        /// Create a persistence strategy mock for the given type
        /// </summary>
        private Mock<IBigCommerceAuthenticationPersistenceStrategy> CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType type)
        {
            var strategy = mock.CreateKeyedMockOf<IBigCommerceAuthenticationPersistenceStrategy>()
                .For(type);
            strategy.Setup(x => x.SaveDataToStoreFromViewModel(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<BigCommerceAccountSettingsViewModel>()))
                .Returns(GenericResult.FromSuccess(new BigCommerceStoreEntity()));
            return strategy;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
