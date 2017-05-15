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
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Stores.Platforms.ShopSite.AccountSettings;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Logon;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ShopSite
{
    public class ShopSiteAccountSettingsViewModelTest : IDisposable
    {
        readonly AutoMock mock;

        public ShopSiteAccountSettingsViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_SetsAuthenticationTypeToOauth()
        {
            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            Assert.Equal(ShopSiteAuthenticationType.Oauth, testObject.AuthenticationType);
        }

        [Fact]
        public void LoadStore_SetsDataFromStore()
        {
            CreateSuccessfulPersistenceStrategyFor(ShopSiteAuthenticationType.Basic);

            var store = mock.CreateMock<IShopSiteStoreEntity>();
            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            store.SetupGet(x => x.ApiUrl).Returns("http://www.something.com");
            store.SetupGet(x => x.ShopSiteAuthentication).Returns(ShopSiteAuthenticationType.Basic);

            testObject.LoadStore(store.Object);

            Assert.Equal("http://www.something.com", testObject.ApiUrl);
            Assert.Equal(ShopSiteAuthenticationType.Basic, testObject.AuthenticationType);
        }

        [Fact]
        public void LoadStore_DelegatesToPersistenceStrategy()
        {
            var strategy = mock.CreateKeyedMockOf<IShopSiteAuthenticationPersistenceStrategy>()
                .For(ShopSiteAuthenticationType.Oauth);

            var store = mock.CreateMock<IShopSiteStoreEntity>();
            store.SetupGet(x => x.ShopSiteAuthentication).Returns(ShopSiteAuthenticationType.Oauth);
            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            testObject.LoadStore(store.Object);

            strategy.Verify(x => x.LoadStoreIntoViewModel(store.Object, testObject));
        }

        [Fact]
        public void LoadStore_ThrowsNullArgumentException_WhenStoreIsNull()
        {
            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            Assert.Throws<ArgumentNullException>(() => testObject.LoadStore(null));
        }

        [Theory]
        [InlineData(null, "Please enter the path of your ShopSite store")]
        [InlineData("", "Please enter the path of your ShopSite store")]
        [InlineData("  ", "Please enter the path of your ShopSite store")]
        //[InlineData("http://www.something.com/path", "The specified URL is not a valid address")]
        public void SaveToEntity_WithInvalidApiUrl_CausesError(string apiUrl, string errorMessage)
        {
            mock.CreateKeyedMockOf<IShopSiteAuthenticationPersistenceStrategy>()
                .For(ShopSiteAuthenticationType.Oauth)
                .SetReturnsDefault(Result.FromSuccess());

            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();
            testObject.ApiUrl = apiUrl;

            var result = testObject.SaveToEntity(new ShopSiteStoreEntity());
            Assert.False(result);
            mock.Mock<IMessageHelper>().Verify(x => x.ShowError(errorMessage));
        }

        [Fact]
        public void SaveToEntity_WithInvalidApiUrl_DelegatesToPersistenceStrategy()
        {
            mock.CreateKeyedMockOf<IShopSiteAuthenticationPersistenceStrategy>()
                .For(ShopSiteAuthenticationType.Oauth)
                .Setup(x => x.ValidateApiUrl("http://www.something.com/path"))
                .Returns(Result.FromError("foo"));

            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();
            testObject.ApiUrl = "http://www.something.com/path";

            var result = testObject.SaveToEntity(new ShopSiteStoreEntity());
            Assert.False(result);
            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("foo"));
        }

        [Fact]
        public void SaveToEntity_DelegatesToPersistenceStrategy_WhenApiKeyIsValid()
        {
            var strategy = mock.CreateKeyedMockOf<IShopSiteAuthenticationPersistenceStrategy>()
                .For(ShopSiteAuthenticationType.Basic);

            var store = new ShopSiteStoreEntity();
            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = "http://www.example.com";
            testObject.SaveToEntity(store);

            strategy.Verify(x => x.SaveDataToStoreFromViewModel(store, testObject));
        }

        [Fact]
        public void SaveToEntity_CausesError_WhenPersistenceStrategyFails()
        {
            mock.CreateKeyedMockOf<IShopSiteAuthenticationPersistenceStrategy>()
                .For(ShopSiteAuthenticationType.Basic)
                .Setup(x => x.SaveDataToStoreFromViewModel(It.IsAny<ShopSiteStoreEntity>(), It.IsAny<ShopSiteAccountSettingsViewModel>()))
                .Returns(GenericResult.FromError<ShopSiteStoreEntity>("Foo"));

            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            testObject.LoadStore(new ShopSiteStoreEntity());
            testObject.ApiUrl = "http://www.example.com";

            var result = testObject.SaveToEntity(new ShopSiteStoreEntity());

            Assert.False(result);
            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("Foo"));
        }

        [Theory]
        [InlineData(ShopSiteAuthenticationType.Oauth)]
        [InlineData(ShopSiteAuthenticationType.Basic)]
        public void SaveToEntity_DelegatesToIdentifier(ShopSiteAuthenticationType authenticationType)
        {
            CreateSuccessfulPersistenceStrategyFor(authenticationType);

            var store = new ShopSiteStoreEntity();
            store.ShopSiteAuthentication = authenticationType;
            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            testObject.ApiUrl = "https://demo.shopsite.com/authorize.cgi";
            testObject.SaveToEntity(store);

            mock.Mock<IShopSiteIdentifier>().Verify(x => x.Set(store, testObject.ApiUrl));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SaveToEntity_DelegatesToConnectionVerifier_AndReturnsResult(bool verifierResult)
        {
            var store = new ShopSiteStoreEntity();
            var strategy = CreateSuccessfulPersistenceStrategyFor(ShopSiteAuthenticationType.Basic);

            mock.Mock<IShopSiteConnectionVerifier>()
                .Setup(x => x.Verify(store, strategy.Object))
                .Returns(verifierResult ? GenericResult.FromSuccess(Unit.Default) : GenericResult.FromError<Unit>("Foo"));

            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = "http://www.something.com";

            var result = testObject.SaveToEntity(store);

            Assert.Equal(verifierResult, result);
        }

        [Fact]
        public void SaveToEntity_ShowsError_WhenVerifierFails()
        {
            var store = new ShopSiteStoreEntity();

            CreateSuccessfulPersistenceStrategyFor(ShopSiteAuthenticationType.Basic);

            mock.Mock<IShopSiteConnectionVerifier>()
                .Setup(x => x.Verify(It.IsAny<ShopSiteStoreEntity>(), It.IsAny<IShopSiteAuthenticationPersistenceStrategy>()))
                .Returns(GenericResult.FromError<Unit>("Foo"));

            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            testObject.LoadStore(store);
            testObject.ApiUrl = "http://www.example.com";

            testObject.SaveToEntity(store);

            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("Foo"));
        }

        [Fact]
        public void SaveToEntity_TogglesCursor_WhenVerifyingConnection()
        {
            CreateSuccessfulPersistenceStrategyFor(ShopSiteAuthenticationType.Basic);

            mock.Mock<IShopSiteConnectionVerifier>()
                .Setup(x => x.Verify(It.IsAny<ShopSiteStoreEntity>(), It.IsAny<IShopSiteAuthenticationPersistenceStrategy>()))
                .Returns(GenericResult.FromSuccess(Unit.Default));

            var disposable = mock.CreateMock<IDisposable>();
            mock.Mock<IMessageHelper>().Setup(x => x.SetCursor(Cursors.WaitCursor))
                .Returns(disposable);

            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            testObject.LoadStore(new ShopSiteStoreEntity());
            testObject.ApiUrl = "http://www.something.com";

            testObject.SaveToEntity(new ShopSiteStoreEntity());

            disposable.Verify(x => x.Dispose());
        }

        [Fact]
        public void MigrateToOauth_ChangesAuthenticationType_ToOauth()
        {
            CreateSuccessfulPersistenceStrategyFor(ShopSiteAuthenticationType.Basic);

            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();
            var store = mock.CreateMock<ShopSiteStoreEntity>();

            store.SetupGet(x => x.Username).Returns("Username");
            store.SetupGet(x => x.Password).Returns("Password");

            testObject.LoadStore(store.Object);

            testObject.MigrateToOauth.Execute(null);

            Assert.Equal(ShopSiteAuthenticationType.Oauth, testObject.AuthenticationType);
        }

        private Mock<IShopSiteAuthenticationPersistenceStrategy> CreateSuccessfulPersistenceStrategyFor(ShopSiteAuthenticationType type)
        {
            var strategy = mock.CreateKeyedMockOf<IShopSiteAuthenticationPersistenceStrategy>().For(type);

            strategy.Setup(x => x.SaveDataToStoreFromViewModel(It.IsAny<ShopSiteStoreEntity>(), It.IsAny<ShopSiteAccountSettingsViewModel>()))
                .Returns(GenericResult.FromSuccess(new ShopSiteStoreEntity()));

            return strategy;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
