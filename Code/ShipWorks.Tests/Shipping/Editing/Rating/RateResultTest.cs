using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Editing.Rating
{
    [TestClass]
    public class RateResultTest
    {
        [TestMethod]
        public void MaskDescription_DoesNotMaskDescription()
        {
            RateResult testObject = new RateResult("Test Description", "3");
            testObject.MaskDescription(new List<RateResult>());
            Assert.AreEqual("Test Description", testObject.Description);
        }
    }
}
