using ShipWorks.Shipping.Carriers.Postal;
using System.Collections.Generic;
using Xunit;

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
                Assert.True(PostalUtility.IsMilitaryPostalCode(postalCode));
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
                Assert.False(PostalUtility.IsMilitaryPostalCode(postalCode));
            }
        }

        [Fact]
        public void IsGlobalPost_ReturnsTrue_WhenServiceTypeIsGlobalPost()
        {
            Assert.True(PostalUtility.IsGlobalPost(PostalServiceType.GlobalPostEconomyIntl));
        }

        [Fact]
        public void IsGlobalPost_ReturnsTrue_WhenServiceTypeIsGlobalPostSaverEconomy()
        {
            Assert.True(PostalUtility.IsGlobalPost(PostalServiceType.GlobalPostSmartSaverEconomyIntl));
        }

        [Fact]
        public void IsGlobalPost_ReturnsFalse_WhenServiceTypeIsNotGlobalPost()
        {
            Assert.False(PostalUtility.IsGlobalPost(PostalServiceType.AsendiaGeneric));
        }
    }
}
