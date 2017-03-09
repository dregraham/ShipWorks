using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartOrderLoaderTest
    {
        private readonly AutoMock mock;
        private readonly IWalmartOrderLoader testObject;

        public WalmartOrderLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<WalmartOrderLoader>();
        }

        [Fact]
        public void LoadOrder_LoadsItemFromWalmartOrder()
        {
            Order orderDto = new Order();
            WalmartOrderEntity orderEntity = new WalmartOrderEntity();
            orderLineType item = GenerateItem("item1", "name1", "sku1");

            orderDto.orderLines = new[] { item };
            orderDto.shippingInfo = new shippingInfoType
            {
                estimatedDeliveryDate = DateTime.UtcNow.AddDays(-12),
                postalAddress = new postalAddressType
                {
                    name = "foo",
                    address1 = "bar",
                    address2 = "baz",
                    state = "MO",
                    postalCode = "63040",
                    country = "US"
                }
            };

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderDto.orderLines.First().lineNumber, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().LineNumber);
            Assert.Equal(orderDto.orderLines.First().item.productName, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().Name);
            Assert.Equal(orderDto.orderLines.First().item.sku, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().SKU);
            Assert.Equal(orderDto.orderLines.First().orderLineStatuses.First().status.ToString(), orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().LocalStatus);
            Assert.Equal(double.Parse(orderDto.orderLines.First().orderLineStatuses.First().statusQuantity.amount), orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().Quantity);
            Assert.Equal(orderDto.orderLines.First().charges.First().chargeAmount.amount, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().UnitPrice);
        }

        /// <summary>
        /// Helper method to generate walmart items
        /// </summary>
        private static orderLineType GenerateItem(string lineNumber, string name, string sku, orderLineStatusValueType status = orderLineStatusValueType.Created)
        {
            return new orderLineType
            {
                lineNumber = lineNumber,
                item = new itemType
                {
                    productName = name,
                    sku = sku
                },
                orderLineStatuses = new[]
                {
                    new orderLineStatusType
                    {
                        status = status
                    }
                },
                charges = new[]
                {
                    new chargeType
                    {
                        chargeType1 = "PRODUCT",
                        chargeAmount = new moneyType
                        {
                            currency = currencyType.USD,
                            amount = 3.95M
                        }
                    }
                },
                orderLineQuantity = new quantityType
                {
                    unitOfMeasurement = "each",
                    amount = "123"
                }
            };
        }
    }
}
