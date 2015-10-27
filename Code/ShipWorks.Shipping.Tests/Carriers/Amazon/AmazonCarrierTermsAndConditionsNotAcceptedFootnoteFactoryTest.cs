using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Amazon;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    [Trait("Carrier", "Amazon")]
    public class AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactoryTest
    {
        [Fact]
        public void Constructor_WithNullShipmentType_ThrowsNullArgumentException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Assert.Throws<ArgumentNullException>(() => new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(null, new List<string>()));
            }
        }

        [Fact]
        public void Constructor_WithNullCarrierNames_ThrowsNullArgumentException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Assert.Throws<ArgumentNullException>(() => new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(new AmazonShipmentType(null, null, null), null));
            }
        }

        [Fact]
        public void ShipmentType_MatchesConstructorParam()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentType shipmentType = new AmazonShipmentType(null, null, null);
                AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory testObject = new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(shipmentType, new List<string>());

                Assert.Equal(shipmentType, testObject.ShipmentType);
            }
        }

        [Fact]
        public void AllowedForBestRate_IsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentType shipmentType = new AmazonShipmentType(null, null, null);
                List<string> carrierNames = new List<string>() {"asdf", "xxx"};

                AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory testObject = new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(shipmentType, carrierNames);

                Assert.False(testObject.AllowedForBestRate);
            }
        }
    }
}
