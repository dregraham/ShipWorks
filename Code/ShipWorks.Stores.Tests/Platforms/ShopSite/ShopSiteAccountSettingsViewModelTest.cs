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
        public void LoadStore_ThrowsNullArgumentException_WhenStoreIsNull()
        {
            var testObject = mock.Create<ShopSiteAccountSettingsViewModel>();

            Assert.Throws<ArgumentNullException>(() => testObject.LoadStore(null));
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
