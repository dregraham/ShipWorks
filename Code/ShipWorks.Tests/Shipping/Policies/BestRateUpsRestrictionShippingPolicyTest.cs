using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;
using ShipWorks.Shipping.Policies;
using Xunit;

namespace ShipWorks.Tests.Shipping.Policies
{
    public class BestRateUpsRestrictionShippingPolicyTest
    {
        private BestRateUpsRestrictionShippingPolicy testObject;

        public BestRateUpsRestrictionShippingPolicyTest()
        {
            testObject = new BestRateUpsRestrictionShippingPolicy();
        }

        [Fact]
        public void Configure_ThrowsArgumentException_WhenValueIsNotTrueOrFalse()
        {
            var ex = Assert.Throws<ArgumentException>(() => testObject.Configure("nottrueorfalse"));
            Assert.Equal("Unknown configuration value 'nottrueorfalse.' Expected 'true' or 'false.'\r\nParameter name: configuration", ex.Message);
        }


    }
}
