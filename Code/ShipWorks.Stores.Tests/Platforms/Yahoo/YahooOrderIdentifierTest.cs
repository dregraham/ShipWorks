using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo
{
    public class YahooOrderIdentifierTest
    {
        readonly YahooOrderIdentifier testObject = new YahooOrderIdentifier("1");

        [Fact]
        public void ApplyTo_ThrowsYahooException_WhenGivenNonYahooOrder()
        {
            Assert.Throws<YahooException>(() => testObject.ApplyTo(new OrderEntity()));
        }

        [Fact]
        public void ApplyTo_ThrowsYahooException_WhenGivenNullOrder()
        {
            Assert.Throws<YahooException>(() => testObject.ApplyTo((OrderEntity) null));
        }

        [Fact]
        public void ApplyTo_ThrowsArgumentNullException_WhenGivenNullDownloadDetail()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.ApplyTo((DownloadDetailEntity) null));
        }

        [Fact]
        public void ToString_ReturnsCorrectString()
        {
            Assert.Equal("YahooOrderID:1", testObject.ToString());
        }
    }
}
