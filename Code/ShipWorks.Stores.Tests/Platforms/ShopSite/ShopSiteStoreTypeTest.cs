using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ShopSite
{
    public class ShopSiteStoreTypeTest : IDisposable
    {
        readonly AutoMock mock;

        public ShopSiteStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData("Foo", "foo")]
        [InlineData("FOO", "foo")]
        [InlineData("foo", "foo")]
        public void LicenseIdentifier_ReturnsDecryptedLicense(string input, string expected)
        {
            ShopSiteStoreEntity store = new ShopSiteStoreEntity
            {
                StoreTypeCode = StoreTypeCode.ShopSite
            };

            mock.Mock<IShopSiteIdentifier>()
                .Setup(x => x.Get(store))
                .Returns(input);

            var testObject = mock.Create<ShopSiteStoreType>(TypedParameter.From<StoreEntity>(store));

            var result = testObject.LicenseIdentifier;

            Assert.Equal(expected, result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
