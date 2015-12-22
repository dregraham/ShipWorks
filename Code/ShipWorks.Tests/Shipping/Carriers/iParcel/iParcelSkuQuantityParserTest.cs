using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Net.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel
{
    public class iParcelSkuQuantityParserTest
    {
        private iParcelSkuQuantityParser testObject;

        private ShipmentEntity shipment;
        private Mock<ITokenProcessor> tokenProcessor;

        public iParcelSkuQuantityParserTest()
        {
            shipment = new ShipmentEntity()
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
                    Service = (int)iParcelServiceType.Preferred,
                    TrackByEmail = true,
                    TrackBySMS = true
                }
            };

            shipment.Order.OrderItems.Add(new OrderItemEntity { Description = "some description", Quantity = 2, Weight = 1.54, UnitPrice = 3.40M, SKU = "12345678" });
            shipment.Order.OrderItems.Add(new OrderItemEntity { Description = "another description", Quantity = 1, Weight = 5.54, UnitPrice = 4.90M, SKU = "12345678" });

            // Setup the token processor to just return the string that was passed in
            tokenProcessor = new Mock<ITokenProcessor>();
            tokenProcessor.Setup(t => t.Process(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns((string token, ShipmentEntity s) => token);

            testObject = new iParcelSkuQuantityParser(shipment, tokenProcessor.Object);
        }

        [Fact]
        public void Parse_SkuQuantity_Test()
        {
            Dictionary<string, int> items = testObject.Parse("ABC123, 45 | XYZ, 2343");

            Assert.Equal(2, items.Keys.Count);
            Assert.Equal("ABC123", items.Keys.First());
            Assert.Equal("XYZ", items.Keys.Last());

            Assert.Equal(45, items["ABC123"]);
            Assert.Equal(2343, items["XYZ"]);
        }

        [Fact]
        public void Parse_SkuQuantity_SingleItem_Test()
        {
            Dictionary<string, int> items = testObject.Parse("ABC123, 45");

            Assert.Equal(1, items.Count);
            Assert.True(items.ContainsKey("ABC123"));
            Assert.Equal(45, items["ABC123"]);

        }

        [Fact]
        public void Parse_SkuQuantity_SkuContainsPunctuation_Test()
        {
            Dictionary<string, int> items = testObject.Parse("ABC._~;123, 45 | XYZ, 2343");

            Assert.Equal(2, items.Count);

            Assert.True(items.ContainsKey("ABC._~;123"));
            Assert.True(items.ContainsKey("XYZ"));

            Assert.Equal(45, items["ABC._~;123"]);
            Assert.Equal(2343, items["XYZ"]);
        }

        [Fact]
        public void Parse_SkuQuantity_DanglingPipe_Test()
        {
            Dictionary<string, int> items = testObject.Parse("ABC123, 45 | XYZ, 2343|");

            Assert.Equal(2, items.Count);
            Assert.True(items.ContainsKey("ABC123"));
            Assert.True(items.ContainsKey("XYZ"));

            Assert.Equal(45, items["ABC123"]);
            Assert.Equal(2343, items["XYZ"]);
        }

        [Fact]
        public void Parse_SkuQuantity_DanglingPipeAndTrailingWhitespace_Test()
        {
            Dictionary<string, int> items = testObject.Parse("ABC123, 45 | XYZ, 2343|        ");

            Assert.Equal(2, items.Count);
            Assert.True(items.ContainsKey("ABC123"));
            Assert.True(items.ContainsKey("XYZ"));

            Assert.Equal(45, items["ABC123"]);
            Assert.Equal(2343, items["XYZ"]);
        }

        [Fact]
        public void Parse_DelegatesToTokenProcessor_Test()
        {
            testObject.Parse("000000, 5 | 111111, 6");

            // Check that the token processor was called for the sku/quantity string
            tokenProcessor.Verify(t => t.Process("000000, 5 | 111111, 6", shipment), Times.Once());
        }

        [Fact]
        public void Parse_ThrowsiParcelException_WhenSkuQuantityGroupingLengthIsNotTwo_Test()
        {

            // This will generate a skuQuantity array with only a single item (2343)
            Assert.Throws<iParcelException>(() => testObject.Parse("ABC123, 45 | 2343"));
        }

        [Fact]
        public void Parse_ThrowsiParcelException_WhenSkuQuantityGroupingIsMissingPipe_Test()
        {
            // Pipe is missing after the first quantity value
            Assert.Throws<iParcelException>(() => testObject.Parse("ABC123, 45 XYZ, 2343"));
        }

        [Fact]
        public void Parse_ThrowsiParcelException_WhenSkuQuantityListIsMissingSku_Test()
        {
            Assert.Throws<iParcelException>(() => testObject.Parse("123456789, 45 | , 2343"));
        }

        [Fact]
        public void Parse_ThrowsiParcelException_WhenQuantityIsString_Test()
        {
            Assert.Throws<iParcelException>(() => testObject.Parse("123456789, 45 | 123, ABC"));
        }

        [Fact]
        public void Parse_ThrowsiParcelException_WhenQuantityIsDecimal_Test()
        {
            Assert.Throws<iParcelException>(() => testObject.Parse("123456789, 45 | 123, 45.4"));
        }
    }
}
