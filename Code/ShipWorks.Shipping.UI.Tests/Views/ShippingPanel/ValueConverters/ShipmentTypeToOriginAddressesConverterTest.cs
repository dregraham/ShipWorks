using Autofac.Extras.Moq;
using ShipWorks.Tests.Shared;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.UI.ShippingPanel.ValueConverters;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Views.ShippingPanel.ValueConverters
{
    public class ShipmentTypeToOriginAddressesConverterTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("foo")]
        public void Convert_ReturnsEmptyList_WhenValueIsNotShipmentTypeCode(object value)
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var testObject = mock.Create<ShipmentTypeToOriginAddressesConverter>();
                var result = testObject.Convert("foo", typeof(object), null, null) as IEnumerable<KeyValuePair<string, long>>;
                Assert.Empty(result);
            }
        }

        [Fact]
        public void Convert_GetsShipmentTypeFromFactory_WhenValueIsShipmentTypeCode()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var testObject = mock.Create<ShipmentTypeToOriginAddressesConverter>();
                testObject.Convert(ShipmentTypeCode.Usps, typeof(object), null, null);
                mock.Mock<IShipmentTypeFactory>()
                    .Verify(x => x.Get(ShipmentTypeCode.Usps));
            }
        }

        [Fact]
        public void Convert_ReturnsListFromShipmentType_WhenValueIsShipmentTypeCode()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                KeyValuePair<string, long> item1 = new KeyValuePair<string, long>("Foo", 1);
                KeyValuePair<string, long> item2 = new KeyValuePair<string, long>("Bar", 2);

                mock.WithShipmentTypeFromFactory(type =>
                {
                    type.Setup(x => x.GetOrigins())
                        .Returns(new List<KeyValuePair<string, long>> { item1, item2 });
                });

                var testObject = mock.Create<ShipmentTypeToOriginAddressesConverter>();
                var result = testObject.Convert(ShipmentTypeCode.Usps, typeof(object), null, null) as IEnumerable<KeyValuePair<string, long>>;

                Assert.Equal(2, result.Count());
                Assert.Contains(item1, result);
                Assert.Contains(item2, result);
            }
        }
    }
}
