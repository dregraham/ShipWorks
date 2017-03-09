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

        private WalmartOrderEntity orderEntity;
        private Order orderDto;

        public WalmartOrderLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<WalmartOrderLoader>();

            orderDto = new Order();
            orderEntity = new WalmartOrderEntity();
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
        }

        [Fact]
        public void LoadOrder_LoadsItemFromWalmartOrder()
        {
            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderDto.orderLines.First().lineNumber, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().LineNumber);
            Assert.Equal(orderDto.orderLines.First().item.productName, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().Name);
            Assert.Equal(orderDto.orderLines.First().item.sku, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().SKU);
            Assert.Equal(orderDto.orderLines.First().orderLineStatuses.First().status.ToString(), orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().LocalStatus);
            Assert.Equal(double.Parse(orderDto.orderLines.First().orderLineQuantity.amount), orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().Quantity);
            Assert.Equal(orderDto.orderLines.First().charges.First().chargeAmount.amount, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().First().UnitPrice);
        }

        [Fact]
        public void LoadOrder_SetsCustomerOrderID_WhenOrderIsNew()
        {
            orderEntity.IsNew = true;
            orderDto.customerOrderId = "123abcd";

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderDto.customerOrderId, orderEntity.CustomerOrderID);
        }

        [Fact]
        public void LoadOrder_SetsPurchaseOrderID_WhenOrderIsNew()
        {
            orderEntity.IsNew = true;
            orderDto.purchaseOrderId = "PO123abcd";

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderDto.purchaseOrderId, orderEntity.PurchaseOrderID);
        }

        [Fact]
        public void LoadOrder_SetsOrderDate_WhenOrderIsNew()
        {
            orderEntity.IsNew = true;
            orderDto.orderDate = DateTime.UtcNow.AddDays(-12);

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderDto.orderDate, orderEntity.OrderDate);
        }

        [Fact]
        public void LoadOrder_SetsEstimatedDeliveryDate_WhenOrderIsNew()
        {
            orderEntity.IsNew = true;
            orderDto.shippingInfo.estimatedDeliveryDate = DateTime.UtcNow.AddDays(-12);

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderDto.shippingInfo.estimatedDeliveryDate, orderEntity.EstimatedDeliveryDate);
        }

        [Fact]
        public void LoadOrder_SetsEstimatedShipDate_WhenOrderIsNew()
        {
            orderEntity.IsNew = true;
            orderDto.shippingInfo.estimatedShipDate = DateTime.UtcNow.AddDays(-12);

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderDto.shippingInfo.estimatedShipDate, orderEntity.EstimatedShipDate);
        }

        [Fact]
        public void LoadOrder_SetsRequestedShipping_WhenOrderIsNew()
        {
            orderEntity.IsNew = true;
            orderDto.shippingInfo.methodCode = shippingMethodCodeType.WhiteGlove;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderDto.shippingInfo.methodCode.ToString(), orderEntity.RequestedShipping);
        }

        [Fact]
        public void LoadOrder_DelegatesToOrderRepositoryPopulateOrderDetails_WhenOrderIsNotNew()
        {
            orderEntity.IsNew = false;
            testObject.LoadOrder(orderDto, orderEntity);

            mock.Mock<IOrderRepository>().Verify(r => r.PopulateOrderDetails(orderEntity));
        }

        [Fact]
        public void LoadOrder_ResetsOrderCharges_WhenOrderIsNotNew()
        {
            orderEntity.IsNew = false;
            orderEntity.OrderCharges.Add(new OrderChargeEntity()
            {
                Amount = 99
            });

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.NotEqual(orderEntity.OrderCharges.FirstOrDefault().Amount, 99);
        }

        [Fact]
        public void LoadOrder_LoadsOrderCharges()
        {
            orderEntity.IsNew = false;
            orderEntity.OrderCharges.Add(new OrderChargeEntity()
            {
                Amount = 99
            });

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderEntity.OrderCharges.FirstOrDefault(c => c.Type == "Random Charge").Amount, 123.121M);
        }

        [Fact]
        public void LoadOrder_LoadsTax()
        {
            orderEntity.IsNew = false;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderEntity.OrderCharges.FirstOrDefault(c => c.Type == "Tax").Amount, 4.99M);
        }

        [Fact]
        public void LoadOrder_LoadsRefunds()
        {
            orderEntity.IsNew = false;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderEntity.OrderCharges.FirstOrDefault(c => c.Type == "Refund").Amount, -7M);
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
                refund = new refundType
                {
                    refundCharges = new []
                    {
                        new refundChargeType
                        {
                            refundReason = reasonCodesType.BillingError,
                            charge = new chargeType
                            {
                                chargeType1 = "refund",
                                chargeAmount = new moneyType
                                {
                                    currency = currencyType.USD,
                                    amount = -7M
                                }
                            }
                        }
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
                    },
                    new chargeType
                    {
                        chargeType1 = "Random Charge",
                        chargeAmount = new moneyType
                        {
                            currency = currencyType.USD,
                            amount = 123.121M
                        }
                    },
                    new chargeType
                    {
                        chargeType1 = "Random Tax",
                        chargeAmount = new moneyType
                        {
                            currency = currencyType.USD,
                            amount = 123.121M
                        },
                        tax = new taxType
                        {
                            taxName = "tax blah blah",
                            taxAmount = new moneyType
                            {
                                currency = currencyType.USD,
                                amount = 4.99M
                            }
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
