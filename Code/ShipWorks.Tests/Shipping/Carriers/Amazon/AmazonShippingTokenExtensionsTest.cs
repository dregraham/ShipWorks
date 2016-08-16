using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Stores.Platforms.Amazon;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon
{
    public class AmazonShippingTokenExtensionsTest
    {
        [Fact]
        public void FactMethodName()
        {
            var errorDate = new DateTime(2001, 1, 1);
            IAmazonCredentials store = new AmazonStoreEntity();
            store.SetShippingToken(new AmazonShippingToken() { ErrorDate = errorDate, ErrorReason = "" });

            Assert.True(!string.IsNullOrEmpty(store.ShippingToken));

            var amazonShippingToken = store.GetShippingToken();

            Assert.Equal(string.Empty, amazonShippingToken.ErrorReason);
            Assert.Equal(errorDate, amazonShippingToken.ErrorDate);
        }
    }
}
