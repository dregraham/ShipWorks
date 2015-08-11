using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Data.Model.EntityClasses;
using Moq;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel
{
    public class iParcelTokenSuggestionFactoryTest
    {
        private iParcelTokenSuggestionFactory testObject;

        private List<ShipmentEntity> shipments;
        private Mock<IiParcelRepository> repository;

        public iParcelTokenSuggestionFactoryTest()
        {
            repository = new Mock<IiParcelRepository>();
            repository.Setup(r => r.PopulateOrderDetails(It.IsAny<ShipmentEntity>()));

            shipments = new List<ShipmentEntity>
            {
                new ShipmentEntity()
                {
                    ShipCity = "St. Louis",
                    ShipCompany = "Initech",
                    ShipCountryCode = "US",
                    ShipEmail = "someone@nowhere.com",
                    ShipFirstName = "Peter",
                    ShipLastName = "Gibbons",
                    ShipPhone = "555-555-5555",
                    ShipPostalCode = "63102",
                    ShipStateProvCode = "MO",
                    ShipStreet1 = "1 Main Street",
                    ShipStreet2 = "Suite 500",

                    OriginFirstName = "Bill",
                    OriginLastName = "Lumbergh",
                    OriginStreet1 = "500 First Street",
                    OriginStreet2 = "Suite 200",
                    OriginCity = "Chicago",
                    OriginStateProvCode = "IL",
                    OriginPostalCode = "66666",
                    OriginCountryCode = "RU",

                    Order = new OrderEntity() { OrderTotal = 100.43M },

                    IParcel = new IParcelShipmentEntity
                    {
                        Reference = "reference-value",
                        Service = (int) iParcelServiceType.Preferred,
                        TrackByEmail = true,
                        TrackBySMS = true
                    }
                }
            };


            shipments[0].Order.OrderItems.Add(new OrderItemEntity { Description = "some description", Quantity = 2, Weight = 1.54, UnitPrice = 3.40M, SKU = "12345678" });
            shipments[0].Order.OrderItems.Add(new OrderItemEntity { Description = "another description", Quantity = 1, Weight = 5.54, UnitPrice = 4.90M, SKU = "987654321" });

            testObject = new iParcelTokenSuggestionFactory(shipments, repository.Object);
        }

        [Fact]
        public void GetSuggestions_AddsDefaultSuggestion_WhenShipmentCountIsOne_Test()
        {
            TokenSuggestion[] suggestions = testObject.GetSuggestions(TokenUsage.Generic);

            Assert.Equal(1, suggestions.Count(s => s.Xsl == "<xsl:for-each select=\"//Order/Item\"> {SKU}, {Quantity} <xsl:if test=\"position() !=  last()\">|</xsl:if></xsl:for-each>"));
        }

        [Fact]
        public void GetSuggestions_AddsDefaultSuggestion_WhenShipmentCountIsGreaterThanOne_Test()
        {
            shipments.Add(new ShipmentEntity());

            TokenSuggestion[] suggestions = testObject.GetSuggestions(TokenUsage.Generic);

            Assert.Equal(1, suggestions.Count(s => s.Xsl == "<xsl:for-each select=\"//Order/Item\"> {SKU}, {Quantity} <xsl:if test=\"position() !=  last()\">|</xsl:if></xsl:for-each>"));
        }

        [Fact]
        public void GetSuggestions_AddsDefaultSuggestion_WhenShipmentCountIsZero_Test()
        {
            shipments.RemoveAt(0);

            TokenSuggestion[] suggestions = testObject.GetSuggestions(TokenUsage.Generic);

            Assert.Equal(1, suggestions.Count(s => s.Xsl == "<xsl:for-each select=\"//Order/Item\"> {SKU}, {Quantity} <xsl:if test=\"position() !=  last()\">|</xsl:if></xsl:for-each>"));
        }

        [Fact]
        public void GetSuggestions_DelegatesToRepository_WhenShipmentCountIsOne_Test()
        {
            testObject.GetSuggestions(TokenUsage.Generic);

            repository.Verify(r => r.PopulateOrderDetails(shipments[0]), Times.Once());
        }

        [Fact]
        public void GetSuggestions_DoesNotDelegateToRepository_WhenShipmentCountIsGreaterThanOne_Test()
        {
            shipments.Add(new ShipmentEntity());

            testObject.GetSuggestions(TokenUsage.Generic);

            repository.Verify(r => r.PopulateOrderDetails(shipments[0]), Times.Never());
        }

        [Fact]
        public void GetSuggestions_DoesNotDelegateToRepository_WhenShipmentCountIsZero_Test()
        {
            shipments.RemoveAt(0);

            testObject.GetSuggestions(TokenUsage.Generic);

            repository.Verify(r => r.PopulateOrderDetails(It.IsAny<ShipmentEntity>()), Times.Never());
        }

        [Fact]
        public void GetSuggestions_AddsSuggestionForEachOrderItem_WhenShipmentCountIsOne_Test()
        {
            TokenSuggestion[] suggestions = testObject.GetSuggestions(TokenUsage.Generic);

            // Filter out the default suggestion
            List<TokenSuggestion> itemSpecificSuggestions = suggestions.Where(s => s.Xsl != "<xsl:for-each select=\"//Order/Item\"> {SKU}, {Quantity} <xsl:if test=\"position() !=  last()\">|</xsl:if></xsl:for-each>").ToList();

            Assert.Equal(2, itemSpecificSuggestions.Count);
        }

        [Fact]
        public void GetSuggestions_OrderItemSuggestionsIsCommaDelimited_WhenShipmentCountIsOne_Test()
        {
            TokenSuggestion[] suggestions = testObject.GetSuggestions(TokenUsage.Generic);

            // Filter out the default suggestion
            List<TokenSuggestion> itemSpecificSuggestions = suggestions.Where(s => s.Xsl != "<xsl:for-each select=\"//Order/Item\"> {SKU}, {Quantity} <xsl:if test=\"position() !=  last()\">|</xsl:if></xsl:for-each>").ToList();

            foreach (TokenSuggestion suggestion in itemSpecificSuggestions)
            {
                string[] commaSeparatedItem = suggestion.Xsl.Split(new char[] { ',' });
                Assert.Equal(2, commaSeparatedItem.Length);
            }
        }

        [Fact]
        public void GetSuggestions_OrderItemSuggestionsXSL_WhenShipmentCountIsOne_Test()
        {
            TokenSuggestion[] suggestions = testObject.GetSuggestions(TokenUsage.Generic);

            // Filter out the default suggestion
            List<TokenSuggestion> itemSpecificSuggestions = suggestions.Where(s => s.Xsl != "<xsl:for-each select=\"//Order/Item\"> {SKU}, {Quantity} <xsl:if test=\"position() !=  last()\">|</xsl:if></xsl:for-each>").ToList();

            foreach (OrderItemEntity orderItem in shipments[0].Order.OrderItems)
            {
                string expectedXsl = string.Format("{0}, {1} ", orderItem.SKU, orderItem.Quantity);
                Assert.Equal(1, itemSpecificSuggestions.Count(s => s.Xsl == expectedXsl));
            }
        }

        [Fact]
        public void GetSuggestions_ThrowsiParcelException_WhenExceptionOccurs_Test()
        {
            // Setup the repository to throw an exception to trigger the exception handling
            repository.Setup(r => r.PopulateOrderDetails(It.IsAny<ShipmentEntity>())).Throws(new NullReferenceException());

            Assert.Throws<iParcelException>(() => testObject.GetSuggestions(TokenUsage.Generic));
        }
    }
}
