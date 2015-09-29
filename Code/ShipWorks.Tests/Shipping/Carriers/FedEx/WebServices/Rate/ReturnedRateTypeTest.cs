using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.WebServices.Rate
{
    class ReturnedRateTypeTest
    {
        [Fact]
        public void ReturnedRateType_ContainsPayorRetialPackage()
        {
            ReturnedRateType result;
            Assert.True(Enum.TryParse<ReturnedRateType>("PAYOR_RETAIL_PACKAGE",false, out result),
                "For Counter Rates, FedEx was returning PAYOR_RETAIL_PACKAGE, but it wasn't in the WSDL. We added it manually and this test is here to make sure we add it again when updating the WSDL next year.");
        }
    }
}
