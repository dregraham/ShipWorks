using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetOrderIdentifierTest
    {
        [Fact]
        public void ApplyToOrder_AppliesMerchantOrderId()
        {
            var testObject = new JetOrderIdentifier("merchID");

            var jetOrderEntity = new JetOrderEntity();

            testObject.ApplyTo(jetOrderEntity);

            Assert.Equal("merchID", jetOrderEntity.MerchantOrderId);
        }

        [Fact]
        public void ApplyToOrder_ThrowsInvalidOperation_WhenGivenNonJetOrder()
        {
            var testObject = new JetOrderIdentifier("merchID");

            var jetOrderEntity = new OrderEntity();

            Assert.Throws<InvalidOperationException>(() => testObject.ApplyTo(jetOrderEntity));
        }

        [Fact]
        public void ApplyToDownloadDetailEntity_SetsMerchantId()
        {
            var testObject = new JetOrderIdentifier("merchID");
            var downloadDetailEntity = new DownloadDetailEntity();

            testObject.ApplyTo(downloadDetailEntity);

            Assert.Equal("merchID", downloadDetailEntity.ExtraStringData1);
        }

        [Fact]
        public void ToString_ReturnsCorrectString()
        {
            var testObject = new JetOrderIdentifier("merchID");
            Assert.Equal("JetMerchantOrderId:merchID", testObject.ToString());
        }
    }
}