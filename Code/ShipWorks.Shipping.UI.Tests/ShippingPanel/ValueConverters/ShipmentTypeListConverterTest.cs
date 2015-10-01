using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.UI.ShippingPanel.ValueConverters;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ValueConverters
{
    public class ShipmentTypeListConverterTest
    {
        [Fact]
        public void Convert_WithNullValues_ReturnsEmptyList()
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(null, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Empty(results);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Other)]
        [InlineData(ShipmentTypeCode.None)]
        [InlineData(ShipmentTypeCode.Usps)]
        public void Convert_ReturnsListWithTypeCode_WithSingleShipmentTypeCode(ShipmentTypeCode type)
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(new object[] { type }, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Contains(type, results.Select(x => x.Value));
        }

        [Fact]
        public void Convert_ReturnsEmptyList_WithSingleNullableShipmentTypeCodeThatIsNull()
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(new object[] { null }, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Empty(results);
        }

        [Fact]
        public void Convert_ReturnsEmptyList_WithSingleString()
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(new object[] { "Foo" }, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Empty(results);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Other, ShipmentTypeCode.FedEx)]
        [InlineData(ShipmentTypeCode.None, ShipmentTypeCode.UpsOnLineTools)]
        [InlineData(ShipmentTypeCode.Usps, ShipmentTypeCode.OnTrac)]
        public void Convert_ReturnsListWithTypeCodes_WithTwoSingleShipmentTypeCode(ShipmentTypeCode type1, ShipmentTypeCode type2)
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(new object[] { type1, type2 }, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Contains(type1, results.Select(x => x.Value));
            Assert.Contains(type2, results.Select(x => x.Value));
        }

        [Fact]
        public void Convert_ReturnsEmptyList_WithTwoSingleBadValues()
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(new object[] { "Foo", 1 }, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Empty(results);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Other, ShipmentTypeCode.FedEx)]
        [InlineData(ShipmentTypeCode.None, ShipmentTypeCode.UpsOnLineTools)]
        [InlineData(ShipmentTypeCode.Usps, ShipmentTypeCode.OnTrac)]
        public void Convert_ReturnsListWithTypeCodes_WithSingleListOfShipmentTypeCodes(ShipmentTypeCode type1, ShipmentTypeCode type2)
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(new object[] { new List<ShipmentTypeCode> { type1, type2 } }, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Contains(type1, results.Select(x => x.Value));
            Assert.Contains(type2, results.Select(x => x.Value));
        }

        [Fact]
        public void Convert_ReturnsEmptyList_WithSingleListOfInvalidItems()
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(new object[] { new List<object> { "foo", 3 } }, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Empty(results);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Other, ShipmentTypeCode.FedEx)]
        [InlineData(ShipmentTypeCode.None, ShipmentTypeCode.UpsOnLineTools)]
        [InlineData(ShipmentTypeCode.Usps, ShipmentTypeCode.OnTrac)]
        public void Convert_ReturnsListWithTypeCodes_WithTwoListsOfShipmentTypeCodes(ShipmentTypeCode type1, ShipmentTypeCode type2)
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(new object[] { new List<ShipmentTypeCode> { type1 }, new List<ShipmentTypeCode> { type2 } }, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Contains(type1, results.Select(x => x.Value));
            Assert.Contains(type2, results.Select(x => x.Value));
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Other, ShipmentTypeCode.Other)]
        [InlineData(ShipmentTypeCode.None, ShipmentTypeCode.None)]
        public void Convert_ReturnsListWithSingleItem_WithDuplicateItems(ShipmentTypeCode type1, ShipmentTypeCode type2)
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(new object[] { type1, new List<ShipmentTypeCode> { type2 } }, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Contains(type1, results.Select(x => x.Value));
            Assert.Equal(1, results.Count());
        }

        [Fact]
        public void Convert_ReturnsTypesInOrder_WhenTypesAreOutOfOrder()
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => "foo");
            object[] allTypes = new object[]
            {
                ShipmentTypeCode.iParcel,
                ShipmentTypeCode.Other,
                ShipmentTypeCode.Usps,
                ShipmentTypeCode.PostalWebTools,
                ShipmentTypeCode.OnTrac,
                ShipmentTypeCode.Endicia,
                ShipmentTypeCode.Express1Usps,
                ShipmentTypeCode.BestRate,
                ShipmentTypeCode.Express1Endicia,
                ShipmentTypeCode.None,
                ShipmentTypeCode.UpsWorldShip,
                ShipmentTypeCode.UpsOnLineTools,
                ShipmentTypeCode.FedEx
            };

            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(allTypes, null, null, null) as IEnumerable<ShipmentTypeListItem>;

            Assert.Equal(ShipmentTypeCode.BestRate, results.ElementAt(0).Value);
            Assert.Equal(ShipmentTypeCode.Usps, results.ElementAt(1).Value);
            Assert.Equal(ShipmentTypeCode.PostalWebTools, results.ElementAt(2).Value);
            Assert.Equal(ShipmentTypeCode.Express1Usps, results.ElementAt(3).Value);
            Assert.Equal(ShipmentTypeCode.FedEx, results.ElementAt(4).Value);
            Assert.Equal(ShipmentTypeCode.UpsOnLineTools, results.ElementAt(5).Value);
            Assert.Equal(ShipmentTypeCode.UpsWorldShip, results.ElementAt(6).Value);
            Assert.Equal(ShipmentTypeCode.Endicia, results.ElementAt(7).Value);
            Assert.Equal(ShipmentTypeCode.Express1Endicia, results.ElementAt(8).Value);
            Assert.Equal(ShipmentTypeCode.OnTrac, results.ElementAt(9).Value);
            Assert.Equal(ShipmentTypeCode.iParcel, results.ElementAt(10).Value);
            Assert.Equal(ShipmentTypeCode.Other, results.ElementAt(11).Value);
            Assert.Equal(ShipmentTypeCode.None, results.ElementAt(12).Value);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Other, "Other")]
        [InlineData(ShipmentTypeCode.None, "None")]
        [InlineData(ShipmentTypeCode.Usps, "USPS")]
        public void Convert_SetsDescription_FromEnumDescription(ShipmentTypeCode type, string description)
        {
            ShipmentTypeListConverter testObject = new ShipmentTypeListConverter(x => description);

            IEnumerable<ShipmentTypeListItem> results = testObject.Convert(new object[] { type }, null, null, null) as IEnumerable<ShipmentTypeListItem>;
            Assert.Contains(description, results.Select(x => x.Description));
        }
    }
}
