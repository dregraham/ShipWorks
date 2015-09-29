using Autofac.Extras.Moq;
using ShipWorks.Shipping.UI.Views.ShippingPanel.ValueConverters;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Views.ShippingPanel.ValueConverters
{
    public class ShipmentTypeToOriginAddressesConverterTest
    {
        [Fact]
        public void Convert_ReturnsEmptyList_WhenValueIsNotShipmentTypeCode()
        {
            //using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            //{
            //    var testObject = mock.Create<ShipmentTypeToOriginAddressesConverter>();
            //    var result = testObject.
            //}
        }
    }
}
