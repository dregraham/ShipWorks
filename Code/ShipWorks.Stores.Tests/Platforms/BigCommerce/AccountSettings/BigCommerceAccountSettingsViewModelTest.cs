using System;
using System.Reactive;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Enums;
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
        public void SaveToEntity_WithInvalidApiUrl_CausesError(string apiUrl, string errorMessage)
        {
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();
            testObject.ApiUrl = apiUrl;
            testObject.BasicUsername = "foo";
            testObject.BasicToken = "bar";

            var result = testObject.SaveToEntity(new BigCommerceStoreEntity());
            Assert.False(result);
            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(errorMessage));
        }

        [Fact]
        public void SaveToEntity_DelegatesToPersistenceStrategy_WhenApiKeyIsValid()
        {
            var strategy = mock.CreateKeyedMockOf<IBigCommerceAuthenticationPersistenceStrategy>()
                .For(BigCommerceAuthenticationType.Basic);

            var store = new BigCommerceStoreEntity();
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = "http://www.example.com";

            testObject.SaveToEntity(store);

            strategy.Verify(x => x.SaveDataToStoreFromViewModel(store, testObject));
        }

        [Fact]
        public void SaveToEntity_CausesError_WhenPersistenceStrategyFails()
        {
            mock.CreateKeyedMockOf<IBigCommerceAuthenticationPersistenceStrategy>()
                .For(BigCommerceAuthenticationType.Basic)
                .Setup(x => x.SaveDataToStoreFromViewModel(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<BigCommerceAccountSettingsViewModel>()))
                .Returns(GenericResult.FromError<BigCommerceStoreEntity>("Foo"));

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(new BigCommerceStoreEntity());
            testObject.ApiUrl = "http://www.example.com";

            var result = testObject.SaveToEntity(new BigCommerceStoreEntity());

            Assert.False(result);
            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("Foo"));
        }

        [Theory]
        [InlineData("www.example.com", "https://www.example.com")]
        [InlineData("https://www.example.com", "https://www.example.com")]
        [InlineData(" www.example.com", "https://www.example.com")]
        [InlineData("www.example.com ", "https://www.example.com")]
        [InlineData(" www.example.com ", "https://www.example.com")]
        [InlineData("https://api.bigcommerce.com/stores/vplh1lw/v3/", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        [InlineData("https://store-vplh1lw.mybigcommerce.com/api/v2/", "https://store-vplh1lw.mybigcommerce.com/api/v2/")]
        public void SaveToEntity_SetsStoreApi_WhenDataIsValidForBasic(string newUrl, string expectedUrl)
        {
            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            var store = new BigCommerceStoreEntity();
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = newUrl;

            testObject.SaveToEntity(store);

            Assert.Equal(expectedUrl, store.ApiUrl);
        }

        [Theory]
        [InlineData(" https://api.bigcommerce.com/stores/vplh1lw/v3/ ", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        [InlineData(" https://store-vplh1lw.mybigcommerce.com/api/v3/ ", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        [InlineData(" https://store-vplh1lw.mybigcommerce.com/api/v2/ ", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        [InlineData(" https://api.bigcommerce.com/stores/vplh1lw/v3 ", "https://api.bigcommerce.com/stores/vplh1lw/v2")]
        [InlineData(" https://store-vplh1lw.mybigcommerce.com/api/v3 ", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        [InlineData(" https://store-vplh1lw.mybigcommerce.com/api/v2 ", "https://api.bigcommerce.com/stores/vplh1lw/v2/")]
        public void SaveToEntity_SetsStoreApi_WhenDataIsValidForOauth(string newUrl, string expectedUrl)
        {
            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Oauth);

            var store = new BigCommerceStoreEntity();
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = newUrl;

            testObject.SaveToEntity(store);

            Assert.Equal(expectedUrl, store.ApiUrl);
        }

        [Theory]
        [InlineData("www.example.com", "www.example.com")]
        [InlineData("https://www.example.com", "https://www.example.com")]
        [InlineData(" www.example.com", " www.example.com")]
        [InlineData("www.example.com ", "www.example.com ")]
        [InlineData(" www.example.com ", " www.example.com ")]
        public void SaveToEntity_SetsStoreApi_WhenDataIsNotValidForOauth(string newUrl, string expectedUrl)
        {
            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Oauth);

            var store = new BigCommerceStoreEntity();
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = newUrl;

            testObject.SaveToEntity(store);

            Assert.Equal(expectedUrl, testObject.ApiUrl);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SaveToEntity_DelegatesToConnectionVerifier_AndReturnsResult(bool verifierResult)
        {
            var store = new BigCommerceStoreEntity();

            var strategy = CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            mock.Mock<IBigCommerceConnectionVerifier>()
                .Setup(x => x.Verify(store, strategy.Object))
                .Returns(verifierResult ? GenericResult.FromSuccess(Unit.Default) : GenericResult.FromError<Unit>("Foo"));

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = "http://www.example.com";

            var result = testObject.SaveToEntity(store);

            Assert.Equal(verifierResult, result);
        }

        [Fact]
        public void SaveToEntity_ShowsError_WhenVerifierFails()
        {
            var store = new BigCommerceStoreEntity();

            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            mock.Mock<IBigCommerceConnectionVerifier>()
                .Setup(x => x.Verify(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<IBigCommerceAuthenticationPersistenceStrategy>()))
                .Returns(GenericResult.FromError<Unit>("Foo"));

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = "http://www.example.com";

            testObject.SaveToEntity(store);

            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("Foo"));
        }

        [Fact]
        public void SaveToEntity_TogglesCursor_WhenVerifyingConnection()
        {
            CreateSuccessfulPersistenceStrategyFor(BigCommerceAuthenticationType.Basic);

            mock.Mock<IBigCommerceConnectionVerifier>()
                .Setup(x => x.Verify(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<IBigCommerceAuthenticationPersistenceStrategy>()))
                .Returns(GenericResult.FromSuccess(Unit.Default));

            var disposable = mock.CreateMock<IDisposable>();
            mock.Mock<IMessageHelper>().Setup(x => x.SetCursor(Cursors.WaitCursor))
                .Returns(disposable);

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(new BigCommerceStoreEntity());
            testObject.ApiUrl = "http://www.example.com";

            testObject.SaveToEntity(new BigCommerceStoreEntity());

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

        [Fact]
        public void MigrateToOauth_DelegatesToOauthStrategy_AfterChange()
        {
            var builder = mock.CreateKeyedMockOf<IBigCommerceAuthenticationPersistenceStrategy>();
            var basicStrategy = builder.For(BigCommerceAuthenticationType.Basic);
            var oauthStrategy = builder.For(BigCommerceAuthenticationType.Oauth);

            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();
            testObject.LoadStore(new BigCommerceStoreEntity { ApiUrl = "http://www.foo.com" });

            testObject.MigrateToOauth.Execute(null);
            testObject.SaveToEntity(new BigCommerceStoreEntity());

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
