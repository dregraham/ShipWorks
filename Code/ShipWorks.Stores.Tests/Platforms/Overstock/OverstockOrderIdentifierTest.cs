using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Overstock;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Overstock
{
    public class OverstockOrderIdentifierTest
    {
        private const string salesChannelOrderNumber = "299";

        [Fact]
        public void ApplyToOrder_AppliesMerchantOrderId()
        {
            var testObject = new AlphaNumericOrderIdentifier(salesChannelOrderNumber);

            var overstockOrderEntity = new OverstockOrderEntity();

            testObject.ApplyTo(overstockOrderEntity);

            Assert.Equal(salesChannelOrderNumber, overstockOrderEntity.OrderNumberComplete);
        }

        [Fact]
        public void ApplyToDownloadDetailEntity_SetsMerchantId()
        {
            var testObject = new AlphaNumericOrderIdentifier(salesChannelOrderNumber);
            var downloadDetailEntity = new DownloadDetailEntity();

            testObject.ApplyTo(downloadDetailEntity);

            Assert.Equal(salesChannelOrderNumber.ToString(), downloadDetailEntity.ExtraStringData1);
        }
    }
}