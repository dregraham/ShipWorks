using System;
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

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class BigCommerceAccountSettingsViewModelTest : IDisposable
    {
        readonly AutoMock mock;

        public BigCommerceAccountSettingsViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void LoadStore_SetsDataFromStore()
        {
            mock.CreateKeyedMockOf<IBigCommerceAuthenticationPersistenceStrategy>()
                .For(BigCommerceAuthenticationType.Basic);

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
        [InlineData("www.example.com")]
        [InlineData("https://www.example.com")]
        [InlineData(" www.example.com")]
        [InlineData("www.example.com ")]
        [InlineData(" www.example.com ")]
        public void SaveToEntity_SetsStoreApi_WhenDataIsValid(string newUrl)
        {
            mock.CreateKeyedMockOf<IBigCommerceAuthenticationPersistenceStrategy>()
                .For(BigCommerceAuthenticationType.Basic)
                .Setup(x => x.SaveDataToStoreFromViewModel(It.IsAny<BigCommerceStoreEntity>(), It.IsAny<BigCommerceAccountSettingsViewModel>()))
                .Returns(GenericResult.FromSuccess(new BigCommerceStoreEntity()));

            var store = new BigCommerceStoreEntity();
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(new BigCommerceStoreEntity());
            testObject.ApiUrl = newUrl;

            testObject.SaveToEntity(store);

            Assert.Equal("https://www.example.com", store.ApiUrl);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
