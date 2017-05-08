using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartOrderIdentifierTest
    {
        [Theory]
        [InlineData("55")]
        [InlineData("blah")]
        public void ToString_ReturnsCorrectString(string po)
        {
            var testObject = new WalmartOrderIdentifier(po);

            Assert.Equal($"WalmartPurchaseOrderID:{po}", testObject.ToString());
        }
        
        [Fact]
        public void ApplyTo_ThrowsInvalidOperationException_WhenGivenNonWalmartOrderEntity()
        {
            var testObject = new WalmartOrderIdentifier("1");
            Assert.Throws<ArgumentNullException>(() => testObject.ApplyTo(new OrderEntity()));
        }

        [Fact]
        public void ApplyTo_ThrowsInvalidOperationException_WhenPassedNullOrderEntity()
        {
            var testObject = new WalmartOrderIdentifier("1");
            Assert.Throws<ArgumentNullException>(() => testObject.ApplyTo((WalmartOrderEntity) null));
        }

        [Fact]
        public void ApplyTo_ThrowsArgumentNullException_WhenPassedNullDownloadDetailEntity()
        {
            var testObject = new WalmartOrderIdentifier("1");
            Assert.Throws<ArgumentNullException>(() => testObject.ApplyTo((DownloadDetailEntity) null));
        }

        [Fact]
        public void ApplyTo_SetsPurchaseOrderNumber()
        {
            var testObject = new WalmartOrderIdentifier("74");
            WalmartOrderEntity order = new WalmartOrderEntity();
            testObject.ApplyTo(order);
            Assert.Equal("74", order.PurchaseOrderID);
        }

        [Fact]
        public void ApplyTo_SetsStringData_WhenPassedDownloadDetail()
        {
            var testObject = new WalmartOrderIdentifier("74");
            DownloadDetailEntity downloadDetailEntity = new DownloadDetailEntity();

            testObject.ApplyTo(downloadDetailEntity);
            Assert.Equal("74", downloadDetailEntity.ExtraStringData1);
        }
    }
}