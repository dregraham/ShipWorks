using System;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class BigCommerceStoreTypeTest : IDisposable
    {
        readonly AutoMock mock;

        public BigCommerceStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void LicenseIdentifier_ReturnsLegacyURLAsIs_WhenLegacyURLDoesNotEndInSlash()
        {
            StoreEntity store = new BigCommerceStoreEntity
            {
                BigCommerceAuthentication = BigCommerceAuthenticationType.Basic,
                ApiUrl = "https://store-cee1f.mybigcommerce.com",
                StoreTypeCode = StoreTypeCode.BigCommerce
            };
            var testObject = mock.Create<BigCommerceStoreType>(TypedParameter.From(store));

            var license = testObject.LicenseIdentifier;

            Assert.Equal("store-cee1f.mybigcommerce.com", license);
        }

        [Theory]
        [InlineData("https://store-cee1f.mybigcommerce.com/")]
        [InlineData("https://store-cee1f.mybigcommerce.com/api/v2/")]
        [InlineData("https://store-cee1f.mybigcommerce.com/admin")]
        [InlineData("https://store-cee1f.mybigcommerce.com/shipworks.php")]
        [InlineData("https://store-cee1f.mybigcommerce.com/admin/shipworks.php")]
        public void LicenseIdentifier_ReturnsLegacyLicense_ForLegacyURLs(string url)
        {
            StoreEntity store = new BigCommerceStoreEntity
            {
                BigCommerceAuthentication = BigCommerceAuthenticationType.Basic,
                ApiUrl = url,
                StoreTypeCode = StoreTypeCode.BigCommerce
            };
            var testObject = mock.Create<BigCommerceStoreType>(TypedParameter.From(store));

            var license = testObject.LicenseIdentifier;

            Assert.Equal("store-cee1f.mybigcommerce.com/", license);
        }

        [Theory]
        [InlineData("http://api.bigcommerce.com/stores/cee1f/v2/")]
        [InlineData("https://api.bigcommerce.com/stores/cee1f/v2/")]
        [InlineData("https://api.bigcommerce.com/stores/cee1f/v2/some/thing?else=foo")]
        [InlineData("https://api.bigcommerce.com/stores/cee1f/v3/")]
        public void LicenseIdentifier_ReturnsLegacyLicense_ForOAuthURLs(string url)
        {
            StoreEntity store = new BigCommerceStoreEntity
            {
                BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth,
                ApiUrl = url,
                StoreTypeCode = StoreTypeCode.BigCommerce
            };
            var testObject = mock.Create<BigCommerceStoreType>(TypedParameter.From(store));

            var license = testObject.LicenseIdentifier;

            Assert.Equal("store-cee1f.mybigcommerce.com/", license);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
