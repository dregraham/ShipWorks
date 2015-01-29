using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Tests.Shipping.Carriers
{
    [TestClass]
    public class CarrierDescriptionTest
    {
        private readonly string[] upsNames = {"UPS", "ups", "u p s", "   UP s "};
        private readonly string[] uspsNames = { "USPS", "usps", "u s p s", "   UsP s " };
        private readonly string[] fedExNames = { "FedEx", "fedex", "f e d E X", "   Fed e x " };
        private readonly string[] dhlNames = { "DHL", "dhl", "d h l", "   DH l " };

        [TestMethod]
        public void OtherCarrier_AllAttributesAreFalse_WhenCarrierTypeIsUnknown()
        {
            ShipmentEntity shipment = CreateOtherShipment("Foo", "Bar");

            CarrierDescription description = new CarrierDescription(shipment);

            Assert.IsFalse(description.IsUSPS);
            Assert.IsFalse(description.IsUPS);
            Assert.IsFalse(description.IsFedEx);
            Assert.IsFalse(description.IsDHL);
        }

        [TestMethod]
        public void OtherCarrier_NameIsCarrier_WhenCarrierTypeIsUnknown()
        {
            ShipmentEntity shipment = CreateOtherShipment("Foo", "Bar");

            CarrierDescription description = new CarrierDescription(shipment);

            Assert.AreEqual("Foo", description.Name);
        }

        [TestMethod]
        public void Name_ReturnsUPS_WhenOtherCarrierContainsUPS()
        {
            foreach (string carrierName in upsNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.AreEqual("UPS", description.Name, string.Format("{0} should set Name to UPS", carrierName));
            }
        }

        [TestMethod]
        public void Name_ReturnsUsps_WhenOtherCarrierContainsUsps()
        {
            foreach (string carrierName in uspsNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.AreEqual("USPS", description.Name, string.Format("{0} should set Name to Usps", carrierName));
            }
        }

        [TestMethod]
        public void Name_ReturnsFedEx_WhenOtherCarrierContainsFedEx()
        {
            foreach (string carrierName in fedExNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.AreEqual("FedEx", description.Name, string.Format("{0} should set Name to FedEx", carrierName));
            }
        }

        [TestMethod]
        public void Name_ReturnsDHL_WhenOtherCarrierContainsDHL()
        {
            foreach (string carrierName in dhlNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.AreEqual("DHL", description.Name, string.Format("{0} should set Name to DHL", carrierName));
            }
        }

        [TestMethod]
        public void IsUps_ReturnsTrue_WhenOtherCarrierContainsUPS()
        {
            foreach (string carrierName in upsNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.IsTrue(description.IsUPS, string.Format("{0} should set IsUPS to true", carrierName));   
            }
        }

        [TestMethod]
        public void IsUsps_ReturnsTrue_WhenOtherCarrierContainsUSPS()
        {
            foreach (string carrierName in uspsNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Priority"));
                Assert.IsTrue(description.IsUSPS, string.Format("{0} should set IsUsps to true", carrierName));
            }
        }

        [TestMethod]
        public void IsFedEx_ReturnsTrue_WhenOtherCarrierContainsFedEx()
        {
            foreach (string carrierName in fedExNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.IsTrue(description.IsFedEx, string.Format("{0} should set IsFedEx to true", carrierName));
            }
        }

        [TestMethod]
        public void IsDHL_ReturnsTrue_WhenOtherCarrierContainsDHL()
        {
            foreach (string carrierName in dhlNames)
            {
                CarrierDescription description = new CarrierDescription(CreateOtherShipment(carrierName, "Ground"));
                Assert.IsTrue(description.IsDHL, string.Format("{0} should set IsDHL to true", carrierName));
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
