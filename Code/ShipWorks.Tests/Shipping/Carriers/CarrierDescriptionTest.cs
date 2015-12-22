using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Tests.Shipping.Carriers
{
    public class CarrierDescriptionTest
    {
        private readonly string[] upsNames = {"UPS", "ups", "u p s", "   UP s "};
        private readonly string[] uspsNames = { "USPS", "usps", "u s p s", "   UsP s " };
        private readonly string[] fedExNames = { "FedEx", "fedex", "f e d E X", "   Fed e x " };
        private readonly string[] dhlNames = { "DHL", "dhl", "d h l", "   DH l " };

        [Fact]
        public void OtherCarrier_AllAttributesAreFalse_WhenCarrierTypeIsUnknown()
        {
            ShipmentEntity shipment = CreateOtherShipment("Foo", "Bar");

            CarrierDescription description = new CarrierDescription(shipment);

            Assert.False(description.IsUSPS);
            Assert.False(description.IsUPS);
            Assert.False(description.IsFedEx);
            Assert.False(description.IsDHL);
        }

        [Fact]
        public void OtherCarrier_NameIsCarrier_WhenCarrierTypeIsUnknown()
        {
            ShipmentEntity shipment = CreateOtherShipment("Foo", "Bar");

            CarrierDescription description = new CarrierDescription(shipment);

            Assert.Equal("Foo", description.Name);
        }

        [Fact]
        public void Name_ReturnsUPS_WhenOtherCarrierContainsUPS()
        {
            foreach (string carrierName in upsNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.Equal("UPS", description.Name);
            }
        }

        [Fact]
        public void Name_ReturnsUsps_WhenOtherCarrierContainsUsps()
        {
            foreach (string carrierName in uspsNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.Equal("USPS", description.Name);
            }
        }

        [Fact]
        public void Name_ReturnsFedEx_WhenOtherCarrierContainsFedEx()
        {
            foreach (string carrierName in fedExNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.Equal("FedEx", description.Name);
            }
        }

        [Fact]
        public void Name_ReturnsDHL_WhenOtherCarrierContainsDHL()
        {
            foreach (string carrierName in dhlNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.Equal("DHL", description.Name);
            }
        }

        [Fact]
        public void IsUps_ReturnsTrue_WhenOtherCarrierContainsUPS()
        {
            foreach (string carrierName in upsNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.True(description.IsUPS, $"{carrierName} should set IsUPS to true");
            }
        }

        [Fact]
        public void IsUsps_ReturnsTrue_WhenOtherCarrierContainsUSPS()
        {
            foreach (string carrierName in uspsNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Priority"));
                Assert.True(description.IsUSPS, string.Format("{0} should set IsUsps to true", carrierName));
            }
        }

        [Fact]
        public void IsFedEx_ReturnsTrue_WhenOtherCarrierContainsFedEx()
        {
            foreach (string carrierName in fedExNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.True(description.IsFedEx, string.Format("{0} should set IsFedEx to true", carrierName));
            }
        }

        [Fact]
        public void IsDHL_ReturnsTrue_WhenOtherCarrierContainsDHL()
        {
            foreach (string carrierName in dhlNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.True(description.IsDHL, string.Format("{0} should set IsDHL to true", carrierName));
            }
        }

        private static ShipmentEntity CreateOtherShipment(string carrier, string service)
        {
            return new ShipmentEntity
            {
                ShipmentType = (int)ShipmentTypeCode.Other,
                Other = new OtherShipmentEntity
                {
                    Carrier = carrier,
                    Service = service
                }
            };
        }
    }
}
