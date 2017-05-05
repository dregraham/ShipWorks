using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Tests.Shared;
using System;
using System.Linq;
using Interapptive.Shared.Business;
using Moq;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartOrderLoaderTest
    {
        private readonly AutoMock mock;
        private readonly IWalmartOrderLoader testObject;
        private readonly WalmartOrderEntity orderEntity;
        private readonly Order orderDto;

        public WalmartOrderLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<WalmartOrderLoader>();

            orderDto = new Order();
            orderEntity = new WalmartOrderEntity();
            orderLineType item = GenerateItem("1", "name1", "sku1");

            orderDto.purchaseOrderId = "123";
            orderDto.customerOrderId = "456";
            orderDto.orderLines = new[] { item };
            orderDto.shippingInfo = new shippingInfoType
            {
                estimatedDeliveryDate = DateTime.UtcNow.AddDays(-12),
                postalAddress = new postalAddressType
                {
                    name = "foo bar",
                    address1 = "bar",
                    address2 = "baz",
                    state = "MO",
                    postalCode = "63040",
                    country = "USA"
                }
            };
        }

        [Fact]
        public void LoadOrder_ExitingLineItemDoesNotChange_WhenNotInDownloadedOrder()
        {
            orderEntity.IsNew = false;
            orderEntity.OrderItems.Add(new WalmartOrderItemEntity() {LineNumber = "15", Quantity = 15});
            Assert.Equal(1, orderEntity.OrderItems.Count);

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(15, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().Single(i=>i.LineNumber == "15").Quantity);
        }

        [Fact]
        public void LoadOrder_LineItemAdded_WhenNoMatchingLineItemFound()
        {
            orderEntity.IsNew = false;
            orderEntity.OrderItems.Add(new WalmartOrderItemEntity() { LineNumber = "15", Quantity = 15 });

            orderDto.orderLines.Single().lineNumber = "2";
            orderDto.orderLines.Single().orderLineQuantity.amount = "1";

            Assert.Equal(1, orderEntity.OrderItems.Count);

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(2, orderEntity.OrderItems.Count);
            Assert.Equal(1, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().Single(i=>i.LineNumber == "2").Quantity);
        }

        [Fact]
        public void LoadOrder_LineItemUpdated_WhenMatchingLineItemFound()
        {
            orderEntity.IsNew = false;
            orderEntity.OrderItems.Add(new WalmartOrderItemEntity() { LineNumber = "15", Quantity = 42 });

            orderDto.orderLines.Single().lineNumber = "15";
            orderDto.orderLines.Single().orderLineQuantity.amount = "6";

            Assert.Equal(1, orderEntity.OrderItems.Count);

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(1, orderEntity.OrderItems.Count);
            Assert.Equal(6, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().Single(i => i.LineNumber == "15").Quantity);
        }


        [Fact]
        public void LoadOrder_LoadsItemFromWalmartOrder()
        {
            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderDto.orderLines.Single().lineNumber, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().Single().LineNumber);
            Assert.Equal(orderDto.orderLines.Single().item.productName, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().Single().Name);
            Assert.Equal(orderDto.orderLines.Single().item.sku, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().Single().SKU);
            Assert.Equal(orderDto.orderLines.Single().orderLineStatuses.Single().status.ToString(), orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().Single().OnlineStatus);
            Assert.Equal(double.Parse(orderDto.orderLines.Single().orderLineQuantity.amount), orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().Single().Quantity);
            Assert.Equal(orderDto.orderLines.Single().charges.Single(c => c.chargeType1 == "PRODUCT").chargeAmount.amount, orderEntity.OrderItems.Cast<WalmartOrderItemEntity>().Single().UnitPrice);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_SetsCustomerOrderID_IfOrderIsNew(bool isNew)
        {
            const string previousValue = "100";
            const string downloadedValue = "200";

            string expectedValue = isNew ? downloadedValue : previousValue;
            orderEntity.IsNew = isNew;

            orderEntity.CustomerOrderID = previousValue;
            orderDto.customerOrderId = downloadedValue;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(expectedValue, orderEntity.CustomerOrderID);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_SetsOrderNumberToPONumber_IfOrderIsNew(bool isNew)
        {
            const long previousValue = 10;
            const long downloadedValue = 42;

            long expectedValue = isNew ? downloadedValue : previousValue;
            orderEntity.IsNew = isNew;

            orderDto.purchaseOrderId = downloadedValue.ToString();
            orderEntity.OrderNumber = previousValue;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(expectedValue, orderEntity.OrderNumber);
        }

        [Fact]
        public void LoadOrder_ThrowsWhenCannotParsePONumber_WhenOrderIsNew()
        {
            orderEntity.IsNew = true;
            orderDto.purchaseOrderId = "42A";

            WalmartException walmartException = Assert.Throws<WalmartException>(() => testObject.LoadOrder(orderDto, orderEntity));
            Assert.Equal("PurchaseOrderId '42A' could not be converted to an number",
                walmartException.Message);
        }

        [Fact]
        public void LoadOrder_LoadsAddressesFromWalmartOrder_WhenIsNew()
        {
            orderEntity.IsNew = true;
            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderDto.shippingInfo.postalAddress.name, orderEntity.BillUnparsedName);
            Assert.Equal("foo", orderEntity.BillFirstName);
            Assert.Equal("MO", orderEntity.BillStateProvCode);
            Assert.Equal("US", orderEntity.BillCountryCode);
        }

        [Fact]
        public void LoadOrder_SkipsAddressesFromWalmartOrder_WhenNotNew()
        {
            orderEntity.IsNew = false;
            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Null(orderEntity.BillUnparsedName);
            Assert.Null(orderEntity.ShipUnparsedName);
        }

        [Fact]
        public void LoadOrder_ShippingAndBillingAddressMatch()
        {
            orderEntity.IsNew = true;
            testObject.LoadOrder(orderDto, orderEntity);

            var billingAddress = new AddressAdapter(orderEntity, "Bill");
            var shippingAddress = new AddressAdapter(orderEntity, "Ship");

            Assert.Equal(billingAddress, shippingAddress);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_SetsOrderDate_IfOrderIsNew(bool isNew)
        {
            DateTime previousValue = DateTime.UtcNow.AddDays(-12);
            DateTime downloadValue = DateTime.UtcNow;

            DateTime expectedValue = isNew ? downloadValue : previousValue;
            orderEntity.IsNew = isNew;

            orderEntity.OrderDate = previousValue;
            orderDto.orderDate = downloadValue;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(expectedValue, orderEntity.OrderDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_SetsEstimatedDeliveryDate_IfOrderIsNew(bool isNew)
        {
            DateTime previousValue = DateTime.UtcNow.AddDays(-12);
            DateTime downloadValue = DateTime.UtcNow;

            DateTime expectedValue = isNew ? downloadValue : previousValue;
            orderEntity.IsNew = isNew;

            orderEntity.EstimatedDeliveryDate = previousValue;
            orderDto.shippingInfo.estimatedDeliveryDate = downloadValue;
            
            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(expectedValue, orderEntity.EstimatedDeliveryDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_SetsEstimatedShipDate_IfOrderIsNew(bool isNew)
        {
            DateTime previousValue = DateTime.UtcNow.AddDays(-12);
            DateTime downloadValue = DateTime.UtcNow;
            DateTime expectedValue = isNew ? downloadValue : previousValue;

            orderEntity.IsNew = isNew;
            orderEntity.EstimatedShipDate = previousValue;

            orderDto.shippingInfo.estimatedShipDate = downloadValue;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(expectedValue, orderEntity.EstimatedShipDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_SetsRequestedShipping_IfOrderIsNew(bool isNew)
        {
            const shippingMethodCodeType previousValue = shippingMethodCodeType.Express;
            const shippingMethodCodeType downloadValue = shippingMethodCodeType.WhiteGlove;
            shippingMethodCodeType expectedValue = isNew ? downloadValue : previousValue;

            orderEntity.IsNew = isNew;
            orderEntity.RequestedShipping = previousValue.ToString();

            orderDto.shippingInfo.methodCode = downloadValue;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(expectedValue.ToString(), orderEntity.RequestedShipping);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_SetsRequestedShippingMethodCode_IfOrderIsNew(bool isNew)
        {
            const shippingMethodCodeType previousValue = shippingMethodCodeType.Express;
            const shippingMethodCodeType downloadValue = shippingMethodCodeType.WhiteGlove;
            shippingMethodCodeType expectedValue = isNew ? downloadValue : previousValue;

            orderEntity.IsNew = isNew;
            orderEntity.RequestedShippingMethodCode = previousValue.ToString();

            orderDto.shippingInfo.methodCode = downloadValue;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(expectedValue.ToString(), orderEntity.RequestedShippingMethodCode);
        }

        [Fact]
        public void LoadOrder_PopulatesOrderDetailsFromOrderRepository_WhenOrderIsNotNew()
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_LoadsOrderCharges(bool isNew)
        {
            orderEntity.IsNew = isNew;
            orderEntity.OrderCharges.Add(new OrderChargeEntity()
            {
                Amount = 99
            });

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderEntity.OrderCharges.Single(c => c.Type == "Random Charge").Amount, 123.121M);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_LoadsTax(bool isNew)
        {
            orderEntity.IsNew = isNew;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderEntity.OrderCharges.Single(c => c.Type == "Tax").Amount, 4.99M);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_LoadsRefunds(bool isNew)
        {
            orderEntity.IsNew = isNew;

            testObject.LoadOrder(orderDto, orderEntity);

            Assert.Equal(orderEntity.OrderCharges.Single(c => c.Type == "Refund").Amount, -7M);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadOrder_DelegatesToCalculateTotal(bool isNew)
        {
            orderEntity.IsNew = isNew;
            mock.Mock<IOrderChargeCalculator>()
                .Setup(c => c.CalculateTotal(orderEntity))
                .Returns(3.50M);
            orderEntity.OrderTotal = 0;

            testObject.LoadOrder(orderDto, orderEntity);

            mock.Mock<IOrderChargeCalculator>()
                .Verify(c => c.CalculateTotal(orderEntity), Times.Once);

            Assert.Equal(3.50M, orderEntity.OrderTotal);
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
