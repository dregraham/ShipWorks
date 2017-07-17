using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ChannelAdvisor
{
    public class ChannelAdvisorOrderLoaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ChannelAdvisorOrderEntity orderToSave;
        private readonly ChannelAdvisorOrder downloadedOrder;
        private readonly Mock<IOrderElementFactory> orderElementFactory;
        private readonly ChannelAdvisorOrderLoader testObject;
        private readonly Mock<IChannelAdvisorRestClient> channelAdvisorRestClient;


        public ChannelAdvisorOrderLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orderToSave = new ChannelAdvisorOrderEntity();
            downloadedOrder = new ChannelAdvisorOrder();
            downloadedOrder.Fulfillments = new List<ChannelAdvisorFulfillment>();
            downloadedOrder.Items = new List<ChannelAdvisorOrderItem>();
            orderElementFactory = mock.Mock<IOrderElementFactory>();
            channelAdvisorRestClient = mock.Mock<IChannelAdvisorRestClient>();
            testObject = mock.Create<ChannelAdvisorOrderLoader>();

            channelAdvisorRestClient.Setup(c => c.GetProduct(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new ChannelAdvisorProduct()
                {
                    Attributes = new List<ChannelAdvisorProductAttribute>(),
                    Images = new List<ChannelAdvisorProductImage>()
                });

            orderElementFactory.Setup(f => f.CreateItem(It.IsAny<OrderEntity>()))
                .Returns<OrderEntity>(order =>
                {
                    var item = new ChannelAdvisorOrderItemEntity();
                    order.OrderItems.Add(item);
                    return item;
                });

            orderElementFactory.Setup(f => f.CreateItemAttribute(It.IsAny<ChannelAdvisorOrderItemEntity>()))
                .Returns<ChannelAdvisorOrderItemEntity>(item =>
                {
                    var attribute = new OrderItemAttributeEntity();
                    item.OrderItemAttributes.Add(attribute);
                    return attribute;
                });
        }

        #region Order Level Tests
        [Fact]
        public void LoadOrder_OrderNumberIsSet()
        {
            downloadedOrder.ID = 123;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(123, orderToSave.OrderNumber);
        }

        [Fact]
        public void LoadOrder_OrderDateIsSet()
        {
            downloadedOrder.CreatedDateUtc = new DateTime(2017, 7, 7);

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(new DateTime(2017, 7, 7), orderToSave.OrderDate);
        }

        [Fact]
        public void LoadOrder_OnlineLastModifiedIsSet()
        {
            downloadedOrder.CreatedDateUtc = new DateTime(2017, 7, 7);

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(new DateTime(2017, 7, 7), orderToSave.OnlineLastModified);
        }

        [Fact]
        public void LoadOrder_CustomOrderIdentifierIsSet()
        {
            downloadedOrder.ID = 123;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(123, orderToSave.OrderNumber);
        }

        [Fact]
        public void LoadOrder_PaymentStatusIsSet()
        {
            downloadedOrder.PaymentStatus = EnumHelper.GetDescription(ChannelAdvisorRestPaymentStatus.Submitted);

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal((int) ChannelAdvisorRestPaymentStatus.Submitted, orderToSave.OnlinePaymentStatus);
        }

        [Fact]
        public void LoadOrder_CheckoutStatusIsSet()
        {
            downloadedOrder.PaymentStatus = EnumHelper.GetDescription(ChannelAdvisorRestCheckoutStatus.Completed);

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal((int)ChannelAdvisorRestCheckoutStatus.Completed, orderToSave.OnlineCheckoutStatus);
        }

        [Fact]
        public void LoadOrder_ShippingStatusIsSet()
        {
            downloadedOrder.PaymentStatus = EnumHelper.GetDescription(ChannelAdvisorRestShippingStatus.Shipped);

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal((int)ChannelAdvisorRestShippingStatus.Shipped, orderToSave.OnlineShippingStatus);
        }

        [Fact]
        public void LoadOrder_OrderFlagStyleIsSet()
        {
            downloadedOrder.FlagID = (int) ChannelAdvisorRestFlagType.BlueFlag;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(EnumHelper.GetDescription(ChannelAdvisorRestFlagType.BlueFlag), orderToSave.FlagStyle);
        }

        [Fact]
        public void LoadOrder_OrderFlagDescriptionIsSet()
        {
            downloadedOrder.FlagDescription = "flag";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("flag", orderToSave.FlagDescription);
        }

        [Fact]
        public void LoadOrder_OrderFlagTypeIsSet()
        {
            downloadedOrder.FlagID = (int) ChannelAdvisorRestFlagType.BlueFlag;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal((int) ChannelAdvisorRestFlagType.BlueFlag, orderToSave.FlagType);
        }

        #region ShippingAddressTests
        [Fact]
        public void LoadOrder_ShipFirstNameIsSet()
        {
            downloadedOrder.ShippingFirstName = "Foo";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Foo", orderToSave.ShipFirstName);
        }

        [Fact]
        public void LoadOrder_ShipLastNameIsSet()
        {
            downloadedOrder.ShippingLastName = "Bar";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Bar", orderToSave.ShipLastName);
        }

        [Fact]
        public void LoadOrder_ShipCompanyIsSet()
        {
            downloadedOrder.ShippingCompanyName = "Interapptive";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Interapptive", orderToSave.ShipCompany);
        }

        [Fact]
        public void LoadOrder_ShipStreet1IsSet()
        {
            downloadedOrder.ShippingAddressLine1 = "1 S Memorial Dr";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("1 S Memorial Dr", orderToSave.ShipStreet1);
        }

        [Fact]
        public void LoadOrder_ShipStreet2IsSet()
        {
            downloadedOrder.ShippingAddressLine2 = "Suite 2000";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Suite 2000", orderToSave.ShipStreet2);
        }

        [Fact]
        public void LoadOrder_ShipCityIsSet()
        {
            downloadedOrder.ShippingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("St Louis", orderToSave.ShipCity);
        }

        [Fact]
        public void LoadOrder_ShipStateIsSet()
        {
            downloadedOrder.ShippingStateOrProvince = "MO";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("MO", orderToSave.ShipStateProvCode);
        }

        [Fact]
        public void LoadOrder_ShipPostalCodeIsSet()
        {
            downloadedOrder.ShippingPostalCode = "63102";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("63102", orderToSave.ShipPostalCode);
        }

        [Fact]
        public void LoadOrder_ShipCountryIsSet()
        {
            downloadedOrder.ShippingCountry = "US";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("US", orderToSave.ShipCountryCode);
        }

        [Fact]
        public void LoadOrder_ShipPhoneIsSet()
        {
            downloadedOrder.ShippingDaytimePhone = "123-456-7890";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("123-456-7890", orderToSave.ShipPhone);
        }

        [Fact]
        public void LoadOrder_ShipEmailIsSet_WhenShippingAndBillingAddressAreTheSame()
        {
            downloadedOrder.BuyerEmailAddress = "support@shipworks.com";
            downloadedOrder.ShippingFirstName = "Foo";
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.ShippingLastName = "Bar";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.ShippingCity = "St Louis";
            downloadedOrder.BillingCity = "St Louis";
            downloadedOrder.ShippingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("support@shipworks.com", orderToSave.ShipEmail);
        }
        #endregion

        #region BillingAddressTests
        [Fact]
        public void LoadOrder_BillFirstNameIsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Foo", orderToSave.BillFirstName);
        }

        [Fact]
        public void LoadOrder_BillLastNameIsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Bar", orderToSave.BillLastName);
        }

        [Fact]
        public void LoadOrder_BillCompanyIsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BillingCompanyName = "Interapptive";
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Interapptive", orderToSave.BillCompany);
        }

        [Fact]
        public void LoadOrder_BillStreet1IsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("1 S Memorial Dr", orderToSave.BillStreet1);
        }

        [Fact]
        public void LoadOrder_BillStreet2IsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BillingAddressLine2 = "Suite 2000";
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Suite 2000", orderToSave.BillStreet2);
        }

        [Fact]
        public void LoadOrder_BillCityIsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("St Louis", orderToSave.BillCity);
        }

        [Fact]
        public void LoadOrder_BillStateIsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BillingStateOrProvince = "MO";
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("MO", orderToSave.BillStateProvCode);
        }

        [Fact]
        public void LoadOrder_BillPostalCodeIsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BillingPostalCode = "63102";
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("63102", orderToSave.BillPostalCode);
        }

        [Fact]
        public void LoadOrder_BillCountryIsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BillingCountry = "US";
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("US", orderToSave.BillCountryCode);
        }

        [Fact]
        public void LoadOrder_BillPhoneIsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BillingDaytimePhone = "123-456-7890";
            downloadedOrder.BillingFirstName = "Foo";
            downloadedOrder.BillingLastName = "Bar";
            downloadedOrder.BillingAddressLine1 = "1 S Memorial Dr";
            downloadedOrder.BillingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("123-456-7890", orderToSave.BillPhone);
        }

        [Fact]
        public void LoadOrder_BillEmailIsSet_WhenBillingFieldsAreNotNull()
        {
            downloadedOrder.BuyerEmailAddress = "support@Billworks.com";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("support@Billworks.com", orderToSave.BillEmail);
        }

        [Fact]
        public void LoadOrder_BillFirstNameIsSetToShippingFirstName_WhenBillingFieldsAreNull()
        {
            downloadedOrder.ShippingFirstName = "Foo";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Foo", orderToSave.BillFirstName);
        }

        [Fact]
        public void LoadOrder_BillLastNameIsSetToShippingLastName_WhenBillingFieldsAreNull()
        {
            downloadedOrder.ShippingLastName = "Bar";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Bar", orderToSave.BillLastName);
        }

        [Fact]
        public void LoadOrder_BillCompanyIsSetToShippingCompany_WhenBillingFieldsAreNull()
        {
            downloadedOrder.ShippingCompanyName = "Interapptive";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Interapptive", orderToSave.BillCompany);
        }

        [Fact]
        public void LoadOrder_BillStreet1IsSetToShippingStreet1_WhenBillingFieldsAreNull()
        {
            downloadedOrder.ShippingAddressLine1 = "1 S Memorial Dr";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("1 S Memorial Dr", orderToSave.BillStreet1);
        }

        [Fact]
        public void LoadOrder_BillStreet2IsSetToShippingStreet2_WhenBillingFieldsAreNull()
        {
            downloadedOrder.ShippingAddressLine2 = "Suite 2000";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Suite 2000", orderToSave.BillStreet2);
        }

        [Fact]
        public void LoadOrder_BillCityIsSetToShippingCity_WhenBillingFieldsAreNull()
        {
            downloadedOrder.ShippingCity = "St Louis";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("St Louis", orderToSave.BillCity);
        }

        [Fact]
        public void LoadOrder_BillStateIsSetToShippingState_WhenBillingFieldsAreNull()
        {
            downloadedOrder.ShippingStateOrProvince = "MO";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("MO", orderToSave.BillStateProvCode);
        }

        [Fact]
        public void LoadOrder_BillPostalCodeIsSetToShippingPostalCode_WhenBillingFieldsAreNull()
        {
            downloadedOrder.ShippingPostalCode = "63102";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("63102", orderToSave.BillPostalCode);
        }

        [Fact]
        public void LoadOrder_BillCountryIsSetToShippingCountry_WhenBillingFieldsAreNull()
        {
            downloadedOrder.ShippingCountry = "US";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("US", orderToSave.BillCountryCode);
        }

        [Fact]
        public void LoadOrder_BillPhoneIsSetToShippingPhone_WhenBillingFieldsAreNull()
        {
            downloadedOrder.ShippingDaytimePhone = "123-456-7890";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("123-456-7890", orderToSave.BillPhone);
        }

        [Fact]
        public void LoadOrder_BillEmailIsSet()
        {
            downloadedOrder.BuyerEmailAddress = "support@Billworks.com";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("support@Billworks.com", orderToSave.BillEmail);
        }

        #endregion












        #endregion

        #region Order Item Level Tests

        [Fact]
        public void LoadOrder_OrderItemNameIsSet()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() {Title = "My Title"});

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(downloadedOrder.Items[0].Title, orderToSave.OrderItems[0].Name);
        }

        [Fact]
        public void LoadOrder_OrderItemQuantityIsSet()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() { Quantity = 42 });

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(downloadedOrder.Items[0].Quantity, orderToSave.OrderItems[0].Quantity);
        }

        [Fact]
        public void LoadOrder_OrderItemUnitPriceIsSet()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() { UnitPrice = 10.23m });

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(downloadedOrder.Items[0].UnitPrice, orderToSave.OrderItems[0].UnitPrice);
        }

        [Fact]
        public void LoadOrder_OrderItemCodeIsSet()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() { Sku = "sku!" });

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(downloadedOrder.Items[0].Sku, orderToSave.OrderItems[0].Code);
        }

        [Fact]
        public void LoadOrder_OrderItemSkuIsSet()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() { Sku = "sku!" });

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(downloadedOrder.Items[0].Sku, orderToSave.OrderItems[0].SKU);
        }

        [Fact]
        public void LoadOrder_SiteOrderItemIDIsSet()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() { SiteOrderItemID = "SO1"});

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(downloadedOrder.Items[0].SiteOrderItemID, 
                ((ChannelAdvisorOrderItemEntity)orderToSave.OrderItems[0]).MarketplaceName);
        }

        [Fact]
        public void LoadOrder_BuyerUserIDIsSet()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            downloadedOrder.BuyerUserID = "Foo";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(downloadedOrder.BuyerUserID,
                ((ChannelAdvisorOrderItemEntity) orderToSave.OrderItems[0]).MarketplaceBuyerID);
        }

        [Fact]
        public void LoadOrder_OrderItemAttrbiuteNotCreated_WhenNoGiftNote()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);
        
            orderElementFactory.Verify(f=>f.CreateItemAttribute(It.IsAny<OrderItemEntity>()), Times.Never);
        }

        [Fact]
        public void LoadOrder_OrderItemAttributeCreated_WhenGiftNote()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() {GiftNotes = "For You!"});

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            var giftAttributeToSave = orderToSave.OrderItems.Single().OrderItemAttributes.Single();

            Assert.Equal("Gift Notes", giftAttributeToSave.Name);
            Assert.Equal("For You!", giftAttributeToSave.Description);
            Assert.Equal(0M, giftAttributeToSave.UnitPrice);
        }

        [Fact]
        public void LoadOrder_OrderItemAttributeCreated_WhenGiftMessage()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() {GiftMessage = "For you!"});

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            var giftAttributeToSave = orderToSave.OrderItems.Single().OrderItemAttributes.Single();

            Assert.Equal("Gift Message", giftAttributeToSave.Name);
            Assert.Equal("For you!", giftAttributeToSave.Description);
            Assert.Equal(0M, giftAttributeToSave.UnitPrice);
        }

        [Fact]
        public void LoadOrder_HasTwoAttributes_WhenGiftNoteAndGiftMessageSet()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem()
            {
                GiftMessage = "For you!",
                GiftNotes = "Some Note"
            });

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            orderElementFactory.Verify(f => f.CreateItemAttribute(It.IsAny<OrderItemEntity>()), Times.Exactly(2));
        }

        [Fact]
        public void LoadOrder_UsesAccessToken

        #endregion

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}