using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps.Express1
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
