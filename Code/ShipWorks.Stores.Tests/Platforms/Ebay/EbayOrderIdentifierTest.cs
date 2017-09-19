using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Ebay
{
    public class EbayOrderIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly EbayOrderEntity order;

        public EbayOrderIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new EbayOrderEntity();
        }

        [Fact]
        public void ToString_ReturnsEbayOrderID()
        {
            var testObject = new EbayOrderIdentifier(123, 456, 789);
            Assert.Equal("eBay:456 (123:789)", testObject.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
