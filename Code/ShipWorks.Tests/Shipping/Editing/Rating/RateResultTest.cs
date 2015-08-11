using System.Collections.Generic;
using Xunit;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Editing.Rating
{
    public class RateResultTest
    {
        [Fact]
        public void MaskDescription_DoesNotMaskDescription()
        {
            RateResult testObject = new RateResult("Test Description", "3");
            testObject.MaskDescription(new List<RateResult>());
            Assert.AreEqual("Test Description", testObject.Description);
        }
    }
}
