using System;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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

        [Theory]
        [InlineData("Foo", "foo")]
        [InlineData("FOO", "foo")]
        [InlineData("foo", "foo")]
        public void LicenseIdentifier_ReturnsDecryptedLicense(string input, string expected)
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity
            {
                StoreTypeCode = StoreTypeCode.BigCommerce
            };

            mock.Mock<IBigCommerceIdentifier>()
                .Setup(x => x.Get(store))
                .Returns(input);

            var testObject = mock.Create<BigCommerceStoreType>(TypedParameter.From<StoreEntity>(store));

            var result = testObject.LicenseIdentifier;

            Assert.Equal(expected, result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
