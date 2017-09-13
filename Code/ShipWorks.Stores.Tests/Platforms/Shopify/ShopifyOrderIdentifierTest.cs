using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Shopify
{
    public class ShopifyOrderIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ShopifyOrderEntity order;

        public ShopifyOrderIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new ShopifyOrderEntity();
        }

        [Fact]
        public void ToString_ReturnsShopifyOrderID()
        {
            var testObject = new ShopifyOrderIdentifier(123);
            Assert.Equal("ShopifyOrderID:123", testObject.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
