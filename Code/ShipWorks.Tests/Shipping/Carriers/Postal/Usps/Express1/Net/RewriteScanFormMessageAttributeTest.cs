using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Express1.Net
{
    [TestClass]
    public class RewriteScanFormMessageAttributeTest
    {
        [TestMethod]
        public void CheckNecessaryCodeIsInPlace_DoesNotThrowError()
        {
            RewriteScanFormMessageAttribute.CheckNecessaryCodeIsInPlace();
        }
    }
}
