using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Amazon;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    [Trait("Carrier", "Amazon")]
    public class AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactoryTest
    {
        [Fact]
        public void Constructor_WithNullCarrierNames_ThrowsNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(null));
        }

        [Fact]
        public void ShipmentType_ReturnsAmazon()
        {
            AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory testObject = new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(new List<string>());

            Assert.Equal(ShipmentTypeCode.Amazon, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void AllowedForBestRate_IsFalse()
        {
            List<string> carrierNames = new List<string>() { "asdf", "xxx" };

            AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory testObject = new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(carrierNames);

            Assert.False(testObject.AllowedForBestRate);
        }
    }
}
