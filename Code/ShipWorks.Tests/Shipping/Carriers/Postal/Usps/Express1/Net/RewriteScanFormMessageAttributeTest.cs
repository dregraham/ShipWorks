using Xunit;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Express1.Net
{
    public class RewriteScanFormMessageAttributeTest
    {
        [Fact]
        public void CheckNecessaryCodeIsInPlace_DoesNotThrowError()
        {
            RewriteScanFormMessageAttribute.CheckNecessaryCodeIsInPlace();
        }
    }
}
