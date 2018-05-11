using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Overstock;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Overstock
{
    public class OverstockOrderIdentifierTest
    {
        private const long testOverstockOrderID = 299;

        [Fact]
        public void ApplyToOrder_AppliesMerchantOrderId()
        {
            var testObject = new OverstockOrderIdentifier(testOverstockOrderID);

            var overstockOrderEntity = new OverstockOrderEntity();

            testObject.ApplyTo(overstockOrderEntity);

            Assert.Equal(testOverstockOrderID, overstockOrderEntity.OverstockOrderID);
        }

        [Fact]
        public void ApplyToOrder_ThrowsInvalidOperation_WhenGivenNonOverstockOrder()
        {
            var testObject = new OverstockOrderIdentifier(testOverstockOrderID);

            var overstockOrderEntity = new OrderEntity();

            Assert.Throws<InvalidOperationException>(() => testObject.ApplyTo(overstockOrderEntity));
        }

        [Fact]
        public void ApplyToDownloadDetailEntity_SetsMerchantId()
        {
            var testObject = new OverstockOrderIdentifier(testOverstockOrderID);
            var downloadDetailEntity = new DownloadDetailEntity();

            testObject.ApplyTo(downloadDetailEntity);

            Assert.Equal(testOverstockOrderID.ToString(), downloadDetailEntity.ExtraStringData1);
        }

        [Fact]
        public void ToString_ReturnsCorrectString()
        {
            var testObject = new OverstockOrderIdentifier(testOverstockOrderID);
            Assert.Equal($"OverstockOrderId:{testOverstockOrderID}", testObject.ToString());
        }
    }
}