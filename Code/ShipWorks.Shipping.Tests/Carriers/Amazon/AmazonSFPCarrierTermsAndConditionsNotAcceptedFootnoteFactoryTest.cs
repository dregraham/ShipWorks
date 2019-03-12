using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    [Trait("Carrier", "Amazon")]
    public class AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactoryTest
    {
        [Fact]
        public void Constructor_WithNullCarrierNames_ThrowsNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory(null));
        }

        [Fact]
        public void ShipmentType_ReturnsAmazon()
        {
            AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory testObject = new AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory(new List<string>());

            Assert.Equal(ShipmentTypeCode.AmazonSFP, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void AllowedForBestRate_IsFalse()
        {
            List<string> carrierNames = new List<string>() { "asdf", "xxx" };

            AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory testObject = new AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory(carrierNames);

            Assert.False(testObject.AllowedForBestRate);
        }
    }
}
