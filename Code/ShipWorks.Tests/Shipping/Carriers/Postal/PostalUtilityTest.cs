using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Tests.Shipping.Carriers.Postal
{
    public class PostalUtilityTest
    {
        [Fact]
        public void IsMilitaryPostalCode_ReturnsTrue_WhenPostalCodeIsMilitary()
        {
            List<string> militaryPostalCodes = new List<string>
            {
                "09000",
                "09500",
                "09899",
                "34000",
                "34099",
                "96200",
                "96450",
                "96699",
            };

            foreach (string postalCode in militaryPostalCodes)
            {
                Assert.IsTrue(PostalUtility.IsMilitaryPostalCode(postalCode), "{0} should be a military postal code", postalCode);
            }
        }

        [Fact]
        public void IsMilitaryPostalCode_ReturnsFalse_WhenPostalCodeIsNotMilitary()
        {
            List<string> postalCodes = new List<string>
            {
                "08999",
                "09900",
                "33999",
                "34100",
                "96199",
                "96700",
            };

            foreach (string postalCode in postalCodes)
            {
                Assert.IsFalse(PostalUtility.IsMilitaryPostalCode(postalCode), "{0} should not be a military postal code", postalCode);
            }
        }
    }
}
