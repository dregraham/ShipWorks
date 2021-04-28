using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay.Warehouse;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.Orders.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Ebay
{
    public class EbayWarehouseOrderFactoryTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly EbayOrderEntity order;

        public EbayWarehouseOrderFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new EbayOrderEntity();
        }

        [Theory]
        [InlineData("789", "123", "456", "789", "123", "456")]
        [InlineData("123-456", "123", "456", "456", "123", "456")]
        [InlineData("123-456-xxx", "123", "456", "456", "123", "456")]
        public void ToString_ReturnsEbayOrderID(
            string orderId, string itemId, string transactionId,
            string expectedOrderId, string expectedItemId, string expectedTranId)
        {
            var ebayItem = new EbayWarehouseItem()
            {
                EbayItemID = long.Parse(itemId),
                EbayTransactionID = long.Parse(transactionId),
            };

            var warehouseOrder = new WarehouseOrder()
            {
                OrderNumber = orderId,
                Items = new List<WarehouseOrderItem>()
                {
                    new WarehouseOrderItem()
                    {
                        AdditionalData = new Dictionary<string, JToken>()
                        {
                            {"ebay", JToken.FromObject(ebayItem) }
                        }
                    }
                }
            };

            var ebayWarehouseOrder = new EbayWarehouseOrder()
            {
                EbayOrderID = orderId
            };

            var ebayUniqueIdentifier = EbayWarehouseOrderFactory.GetEbayOrderIdentifier(warehouseOrder, ebayWarehouseOrder);

            Assert.Equal(expectedOrderId, ebayUniqueIdentifier.EbayOrderID.ToString());
            Assert.Equal(expectedTranId, ebayUniqueIdentifier.TransactionID.ToString());
            Assert.Equal(expectedItemId, ebayUniqueIdentifier.EbayItemID.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
