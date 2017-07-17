using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
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
        private readonly ChannelAdvisorProduct downloadedProduct;

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

            downloadedProduct = new ChannelAdvisorProduct()
            {
                Attributes = new List<ChannelAdvisorProductAttribute>(),
                Images = new List<ChannelAdvisorProductImage>()
            };

            channelAdvisorRestClient.Setup(c => c.GetProduct(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(downloadedProduct);

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
            downloadedOrder.CheckoutStatus = EnumHelper.GetDescription(ChannelAdvisorRestCheckoutStatus.Completed);

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal((int)ChannelAdvisorRestCheckoutStatus.Completed, orderToSave.OnlineCheckoutStatus);
        }

        [Fact]
        public void LoadOrder_ShippingStatusIsSet()
        {
            downloadedOrder.ShippingStatus = EnumHelper.GetDescription(ChannelAdvisorRestShippingStatus.Shipped);

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

        [Fact]
        public void LoadOrder_TotalCalculated_WhenOrderIsNew()
        {
            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);
            mock.Mock<IOrderChargeCalculator>().Verify(c=>c.CalculateTotal(orderToSave), Times.Once);
        }

        [Fact]
        public void LoadOrder_TotalNotCalculated_WhenOrderIsNotNew()
        {
            orderToSave.IsNew = false;
            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);
            mock.Mock<IOrderChargeCalculator>().Verify(c => c.CalculateTotal(orderToSave), Times.Never);
        }

        [Fact]
        public void LoadOrder_OrderTotalSet_WhenOrderIsNew()
        {
            orderToSave.OrderTotal = 12.23M;
            mock.Mock<IOrderChargeCalculator>()
                .Setup(c => c.CalculateTotal(orderToSave))
                .Returns(45.73M);

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);
            
            Assert.Equal(45.73M, orderToSave.OrderTotal);
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

        [Fact]
        public void LoadOrder_RequestedShippingIsSet()
        {
            downloadedOrder.Fulfillments = new List<ChannelAdvisorFulfillment>() {new ChannelAdvisorFulfillment() {ShippingCarrier = "UPS", ShippingClass = "Ground"} };

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("UPS - Ground", orderToSave.RequestedShipping);
        }

        [Fact]
        public void LoadOrder_IsPrimeIsSetToYes_WhenShippingCarrierIsAmazonAndShippingClassIsPrime()
        {
            downloadedOrder.Fulfillments = new List<ChannelAdvisorFulfillment>() { new ChannelAdvisorFulfillment() { ShippingCarrier = "Amazon", ShippingClass = "Prime" } };

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal((int) ChannelAdvisorIsAmazonPrime.Yes, orderToSave.IsPrime);
        }

        [Fact]
        public void LoadOrder_IsPrimeIsSetToNo_WhenShippingCarrierIsNotAmazon()
        {
            downloadedOrder.Fulfillments = new List<ChannelAdvisorFulfillment>() { new ChannelAdvisorFulfillment() { ShippingCarrier = "UPS", ShippingClass = "Ground" } };

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal((int)ChannelAdvisorIsAmazonPrime.No, orderToSave.IsPrime);
        }

        [Fact]
        public void LoadOrder_IsPrimeIsSetToUnknown_WhenShippingCarrierIsNotSet()
        {
            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal((int)ChannelAdvisorIsAmazonPrime.Unknown, orderToSave.IsPrime);
        }

        [Fact]
        public void LoadOrder_ResellerIDIsSet_WhenOrderIsNew()
        {
            downloadedOrder.ResellerID = "ID";
            orderToSave.IsNew = true;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("ID", orderToSave.ResellerID);
        }

        [Fact]
        public void LoadOrder_ResellerIDIsNotSet_WhenOrderIsNotNew()
        {
            downloadedOrder.ResellerID = "ID";
            orderToSave.IsNew = false;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.True(string.IsNullOrWhiteSpace(orderToSave.ResellerID));
        }

        [Fact]
        public void LoadOrder_MarketplaceNamesIsSet_WhenOrderIsNew()
        {
            downloadedOrder.SiteName = "Amazon";
            orderToSave.IsNew = true;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal("Amazon", orderToSave.MarketplaceNames);
        }

        [Fact]
        public void LoadOrder_MarketplaceNamesIsSet_WhenOrderIsNotNew()
        {
            downloadedOrder.ResellerID = "Amazon";
            orderToSave.IsNew = false;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.True(string.IsNullOrWhiteSpace(orderToSave.ResellerID));
        }

        [Fact]
        public void LoadOrder_CreatesNote_WhenPublicNotesFieldIsNotNullAndOrderIsNew()
        {
            downloadedOrder.PublicNotes = "public notes";
            downloadedOrder.CreatedDateUtc = new DateTime(2017, 7, 7);
            orderToSave.IsNew = true;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            orderElementFactory.Verify(f => f.CreateNote(It.IsAny<OrderEntity>(), "public notes", new DateTime(2017, 7, 7), NoteVisibility.Public), Times.Once);
        }

        [Fact]
        public void LoadOrder_CreatesNote_WhenSpecialInstructionsFieldIsNotNullAndOrderIsNew()
        {
            downloadedOrder.SpecialInstructions = "special instructions";
            downloadedOrder.CreatedDateUtc = new DateTime(2017, 7, 7);
            orderToSave.IsNew = true;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            orderElementFactory.Verify(f => f.CreateNote(It.IsAny<OrderEntity>(), "special instructions", new DateTime(2017, 7, 7), NoteVisibility.Public), Times.Once);
        }

        [Fact]
        public void LoadOrder_CreatesNote_WhenPrivateNotesFieldIsNotNullAndOrderIsNew()
        {
            downloadedOrder.PrivateNotes = "private notes";
            downloadedOrder.CreatedDateUtc = new DateTime(2017, 7, 7);
            orderToSave.IsNew = true;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            orderElementFactory.Verify(f => f.CreateNote(It.IsAny<OrderEntity>(), "private notes", new DateTime(2017, 7, 7), NoteVisibility.Internal), Times.Once);
        }

        [Fact]
        public void LoadOrder_DoesNotCreatesNote_WhenOrderIsNotNew()
        {
            downloadedOrder.PrivateNotes = "private notes";
            downloadedOrder.CreatedDateUtc = new DateTime(2017, 7, 7);
            orderToSave.IsNew = false;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            orderElementFactory.Verify(f => f.CreateNote(It.IsAny<OrderEntity>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<NoteVisibility>()), Times.Never);
        }
        #region LoadCharges

        [Fact]
        public void LoadOrder_NoChargesCreated_WhenNoChargesToAdd()
        {
            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            orderElementFactory.Verify(f => f.CreateCharge(It.IsAny<OrderEntity>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void LoadOrder_NoChargesCreated_WhenChargesToAnd_AndNotNew()
        {
            orderToSave.IsNew = false;
            downloadedOrder.TotalTaxPrice = 5.25M;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            orderElementFactory.Verify(f => f.CreateCharge(It.IsAny<OrderEntity>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void LoadOrder_ChargeCreatedForSalesTax()
        {
            downloadedOrder.TotalTaxPrice = 5.25M;
            TestCharge("Sales Tax", "TAX", 5.25M);
        }

        [Fact]
        public void LoadOrder_ChargeCreatedForShipping()
        {
            downloadedOrder.TotalShippingPrice = 11.73M;
            TestCharge("Shipping", "SHIPPING", 11.73M);
        }

        [Fact]
        public void LoadOrder_ChargeCreatedForInsurance()
        {
            downloadedOrder.TotalInsurancePrice = 1.73M;
            TestCharge("Shipping Insurance", "INSURANCE", 1.73M);
        }

        [Fact]
        public void LoadOrder_ChargeCreatedForGiftWrap()
        {
            downloadedOrder.TotalGiftOptionPrice = 112.73M;
            TestCharge("Gift Wrap", "GIFT WRAP", 112.73M);
        }

        [Fact]
        public void LoadOrder_ChargeCreatedForAdditionalCostOrDiscount()
        {
            downloadedOrder.AdditionalCostOrDiscount = -11.73M;
            TestCharge("Additional Cost or Discount", "ADDITIONAL COST OR DISCOUNT", -11.73M);
        }

        private void TestCharge(string type, string description, decimal amount)
        {
            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);
            orderElementFactory.Verify(f => f.CreateCharge(orderToSave, type, description, amount), Times.Once);
            orderElementFactory.Verify(f => f.CreateCharge(It.IsAny<ChannelAdvisorOrderEntity>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);
        }
        #endregion

        #region LoadPayments
        [Fact]
        public void LoadOrder_PaymentDetailNotAdded_WhenOrderIsNotNew()
        {
            orderToSave.IsNew = false;
            downloadedOrder.PaymentMethod = "Blah";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            orderElementFactory.Verify(f => f.CreatePaymentDetail(orderToSave, It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void LoadOrder_PaymentDetailNotAdded_WhenNoPaymentInformation()
        {
            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            orderElementFactory.Verify(f => f.CreatePaymentDetail(orderToSave, It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void LoadOrder_PaymentTypeAdded()
        {
            downloadedOrder.PaymentMethod = "Blah";
            VerifyPaymentDetail("Payment Type", "Blah");
        }

        [Fact]
        public void LoadOrder_CardNumber()
        {
            downloadedOrder.PaymentCreditCardLast4 = "1234";
            VerifyPaymentDetail("Card Number", "1234");
        }

        [Fact]
        public void LoadOrder_ReferenceAdded()
        {
            downloadedOrder.PaymentMerchantReferenceNumber = "ref";
            VerifyPaymentDetail("Reference", "ref");
        }

        [Fact]
        public void LoadOrder_TransactionIdAdded()
        {
            downloadedOrder.PaymentTransactionID = "trans";
            VerifyPaymentDetail("TransactionID", "trans");
        }

        private void VerifyPaymentDetail(string label, string value)
        {
            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            orderElementFactory.Verify(f => f.CreatePaymentDetail(orderToSave, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            orderElementFactory.Verify(f => f.CreatePaymentDetail(orderToSave, label, value), Times.Once);
        }

        #endregion
        #endregion

        #region Order Item Level Tests

        [Fact]
        public void LoadOrder_OrderItemNotAdded_WhenOrderNotNew()
        {
            orderToSave.IsNew = false;
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() { Title = "My Title" });

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Empty(orderToSave.OrderItems);
        }

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
        public void LoadOrder_CallsGetProduct()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() {ProductID = 123});

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");

            channelAdvisorRestClient.Verify(c => c.GetProduct(123, "accessToken"), Times.Once);
        }

        [Fact]
        public void LoadOrder_CallsGetProductTwice_WhenDownloadedOrderHasTwoItems()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() {ProductID = 123});
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() {ProductID = 555});

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");

            channelAdvisorRestClient.Verify(c => c.GetProduct(123, "accessToken"), Times.Once);
            channelAdvisorRestClient.Verify(c => c.GetProduct(555, "accessToken"), Times.Once);
            channelAdvisorRestClient.Verify(c => c.GetProduct(It.IsAny<int>(), "accessToken"), Times.Exactly(2));
        }

        [Fact]
        public void LoadOrder_SetsProductWeight()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            downloadedProduct.Weight = 11.2m;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");
            Assert.Equal(11.2, orderToSave.OrderItems.Single().Weight);
        }

        [Fact]
        public void LoadOrder_SetsLocation()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            downloadedProduct.WarehouseLocation = "In the back";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");
            Assert.Equal("In the back", orderToSave.OrderItems.Single().Location);
        }

        [Fact]
        public void LoadOrder_SetsClassification()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            downloadedProduct.Classification = "First Class";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");
            Assert.Equal("First Class", ((ChannelAdvisorOrderItemEntity) orderToSave.OrderItems.Single()).Classification);
        }

        [Fact]
        public void LoadOrder_SetsUnitCost()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            downloadedProduct.Cost = 12.2m;

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");
            Assert.Equal(12.2m, orderToSave.OrderItems.Single().UnitCost);
        }

        [Fact]
        public void LoadOrder_SetsHarmonizedCode()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            downloadedProduct.HarmonizedCode = "code";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");
            Assert.Equal("code", ((ChannelAdvisorOrderItemEntity) orderToSave.OrderItems.Single()).HarmonizedCode);
        }

        [Fact]
        public void LoadOrder_SetsISBN()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            downloadedProduct.ISBN = "Isbn";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");
            Assert.Equal("Isbn", orderToSave.OrderItems.Single().ISBN);
        }

        [Fact]
        public void LoadOrder_SetsUPC()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            downloadedProduct.UPC = "upc";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");
            Assert.Equal("upc", orderToSave.OrderItems.Single().UPC);
        }

        [Fact]
        public void LoadOrder_SetsMPN()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            downloadedProduct.MPN = "mpn";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");
            Assert.Equal("mpn", ((ChannelAdvisorOrderItemEntity) orderToSave.OrderItems.Single()).MPN);
        }

        [Fact]
        public void LoadOrder_SetsDescription()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            downloadedProduct.Description = "In the back";

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");
            Assert.Equal("In the back", orderToSave.OrderItems.Single().Description);
        }

        [Fact]
        public void LoadOrder_CreatesItemAttrbiute_WhenProductHasAttribute()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            var downloadedAttribute = new ChannelAdvisorProductAttribute()
            {
                Name = "Joe Attribute",
                Value = "the value"
            };
            downloadedProduct.Attributes = new[] {downloadedAttribute};

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");

            orderElementFactory.Verify(
                f => f.CreateItemAttribute(orderToSave.OrderItems.Single(), "Joe Attribute", "the value", 0, false),
                Times.Once);
        }

        [Fact]
        public void LoadOrder_CreatesItemTwoAttrbiutes_WhenProductHasTwoAttributes()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());
            var downloadedAttribute = new ChannelAdvisorProductAttribute()
            {
                Name = "Joe Attribute",
                Value = "the value"
            };
            downloadedProduct.Attributes = new[] { downloadedAttribute, downloadedAttribute };

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");

            orderElementFactory.Verify(
                f => f.CreateItemAttribute(orderToSave.OrderItems.Single(), "Joe Attribute", "the value", 0, false),
                Times.Exactly(2));
        }

        [Fact]
        public void LoadOrder_ImageSaved()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem());

            string url = "http://www.shipworks.com/giffy.gif";
            downloadedProduct.Images = new[]
            {
                new ChannelAdvisorProductImage()
                {
                    Url = url
                }
            };

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, "accessToken");

            Assert.Equal(url, orderToSave.OrderItems.Single().Image);
            Assert.Equal(url, orderToSave.OrderItems.Single().Thumbnail);
        }

        #endregion

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}