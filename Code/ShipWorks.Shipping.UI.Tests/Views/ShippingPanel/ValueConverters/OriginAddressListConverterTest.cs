using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.UI.ShippingPanel.ValueConverters;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Views.ShippingPanel.ValueConverters
{
    public class OriginAddressListConverterTest
    {
        [Fact]
        public void Convert_ReturnsEmptyList_WhenValuesIsNull()
        {
            var testObject = new OriginAddressListConverter();
            var results = testObject.Convert(null, typeof(object), null, null) as IEnumerable<KeyValuePair<string, long>>;
            Assert.Empty(results);
        }

        [Fact]
        public void Convert_ReturnsEmptyList_WhenValuesContainsEmptyArray()
        {
            var testObject = new OriginAddressListConverter();
            var results = testObject.Convert(new object[0], typeof(object), null, null) as IEnumerable<KeyValuePair<string, long>>;
            Assert.Empty(results);
        }

        [Fact]
        public void Convert_ReturnsEmptyList_WhenValuesDoesNotContainListOrLong()
        {
            var testObject = new OriginAddressListConverter();
            var results = testObject.Convert(new object[] { "foo", "bar" }, typeof(object), null, null) as IEnumerable<KeyValuePair<string, long>>;
            Assert.Empty(results);
        }

        [Fact]
        public void Convert_ReturnsList_WhenValuesContainsValidList()
        {
            KeyValuePair<string, long> item1 = new KeyValuePair<string, long>("Foo", 1);
            KeyValuePair<string, long> item2 = new KeyValuePair<string, long>("Bar", 2);

            var testObject = new OriginAddressListConverter();
            var results = testObject.Convert(new object[] { new[] { item1, item2 } }, typeof(object), null, null) as IEnumerable<KeyValuePair<string, long>>;

            Assert.Equal(2, results.Count());
            Assert.Contains(item1, results);
            Assert.Contains(item2, results);
        }

        [Fact]
        public void Convert_ReturnsListWithNoDuplicates_WhenLongValueIsInList()
        {
            KeyValuePair<string, long> item1 = new KeyValuePair<string, long>("Foo", 1);
            KeyValuePair<string, long> item2 = new KeyValuePair<string, long>("Bar", 2);

            var testObject = new OriginAddressListConverter();
            var results = testObject.Convert(new object[] { new[] { item1, item2 }, (long)1 }, typeof(object), null, null) as IEnumerable<KeyValuePair<string, long>>;

            Assert.Equal(2, results.Count());
            Assert.Contains(item1, results);
            Assert.Contains(item2, results);
        }

        [Fact]
        public void Convert_ReturnsListWithDeletedAddress_WhenLongValueIsNotInList()
        {
            KeyValuePair<string, long> item1 = new KeyValuePair<string, long>("Foo", 1);
            KeyValuePair<string, long> item2 = new KeyValuePair<string, long>("Bar", 2);

            var testObject = new OriginAddressListConverter();
            var results = testObject.Convert(new object[] { new[] { item1, item2 }, (long)3 }, typeof(object), null, null) as IEnumerable<KeyValuePair<string, long>>;

            Assert.Equal(3, results.Count());
            Assert.Contains(new KeyValuePair<string, long>("(deleted)", 3), results);
        }

        [Fact]
        public void Convert_ReturnsDeletedAddress_WhenValuesHasLongButNoList()
        {
            var testObject = new OriginAddressListConverter();
            var results = testObject.Convert(new object[] { (long)3 }, typeof(object), null, null) as IEnumerable<KeyValuePair<string, long>>;

            Assert.Equal(1, results.Count());
            Assert.Contains(new KeyValuePair<string, long>("(deleted)", 3), results);
        }
    }
}
