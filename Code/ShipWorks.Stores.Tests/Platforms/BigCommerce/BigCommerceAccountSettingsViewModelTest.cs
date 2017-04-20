using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.BigCommerce;
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
        public void LoadStore_SetsProperties_FromStore()
        {
            var store = mock.CreateMock<IBigCommerceStoreEntity>();
            store.SetupGet(x => x.ApiUrl).Returns("http://www.foo.com");
            store.SetupGet(x => x.ApiUserName).Returns("foo");
            store.SetupGet(x => x.ApiToken).Returns("bar");
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            testObject.LoadStore(store.Object);

            Assert.Equal("http://www.foo.com", testObject.ApiUrl);
            Assert.Equal("foo", testObject.ApiUsername);
            Assert.Equal("bar", testObject.ApiToken);
        }

        [Fact]
        public void LoadStore_ThrowsNullArgumentException_WhenStoreIsNull()
        {
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();

            Assert.Throws<ArgumentNullException>(() => testObject.LoadStore(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveToEntity_WithInvalidApiUserName_CausesError(string apiUsername)
        {
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();
            testObject.ApiUrl = "http://www.foo.com";
            testObject.ApiUsername = apiUsername;
            testObject.ApiToken = "foo";

            var result = testObject.SaveToEntity(new BigCommerceStoreEntity());
            Assert.False(result);
            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError("Please enter the API Username for your BigCommerce store."));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveToEntity_WithInvalidApiToken_CausesError(string apiToken)
        {
            var testObject = mock.Create<BigCommerceAccountSettingsViewModel>();
            testObject.ApiUrl = "http://www.foo.com";
            testObject.ApiUsername = "foo";
            testObject.ApiToken = apiToken;

            var result = testObject.SaveToEntity(new BigCommerceStoreEntity());
            Assert.False(result);
            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError("Please enter an API Token."));
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
            testObject.ApiUsername = "foo";
            testObject.ApiToken = "bar";

            var result = testObject.SaveToEntity(new BigCommerceStoreEntity());
            Assert.False(result);
            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(errorMessage));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
