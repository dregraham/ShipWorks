﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OnLineTools.Api
{
    [TestClass]
    public class UpsApiCoreTest
    {

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void GetUspsEndorsementTypeCode_ReturnsCorrectValue_Test()
        {
            Dictionary<UspsEndorsementType, string> testList = new Dictionary<UspsEndorsementType, string>();
            testList.Add(UspsEndorsementType.ReturnServiceRequested, "1");
            testList.Add(UspsEndorsementType.ForwardingServiceRequested, "2");
            testList.Add(UspsEndorsementType.AddressServiceRequested, "3");
            testList.Add(UspsEndorsementType.ChangeServiceRequested, "4");

            foreach (var entry in testList)
            {
                string testCode = UpsApiCore.GetUspsEndorsementTypeCode(entry.Key);
                Assert.AreEqual(entry.Value, testCode, string.Format("Assert.AreEqual failed. Expected:{0}. Actual:{1}. ", entry.Value, testCode));
            }
        }

        protected XElement WritePackagesXml(UpsShipmentEntity shipment)
        {
            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
                {
                    UpsApiCore.WritePackagesXml(shipment, xmlWriter, false, new UpsPackageWeightElementWriter(xmlWriter), new UpsPackageServiceOptionsElementWriter(xmlWriter));

                    xmlWriter.Flush();
                    stream.Position = 0;
                    return XElement.Load(stream);
                }
            }
        }

        [TestMethod]
        public void WritePackagesXml_WritesAdditionalHandlingIndicator_Test()
        {
            var shipment = new UpsShipmentEntity { Shipment = new ShipmentEntity() };
            var package = shipment.Packages.AddNew();

            package.AdditionalHandlingEnabled = true;

            XElement element = WritePackagesXml(shipment);

            var additionalHandlings =
                ((IEnumerable)element.XPathEvaluate("/AdditionalHandling"))
                    .Cast<XElement>().ToList();

            Assert.AreEqual(1, additionalHandlings.Count);
        }

        [TestMethod]
        public void WritePackagesXml_DoesNotWriteAdditionalHandlingIndicator_WhenNotSelectedOnPackage_Test()
        {
            var shipment = new UpsShipmentEntity { Shipment = new ShipmentEntity() };
            var package = shipment.Packages.AddNew();

            package.AdditionalHandlingEnabled = false;

            XElement element = WritePackagesXml(shipment);

            var additionalHandlings =
                ((IEnumerable)element.XPathEvaluate("/AdditionalHandling"))
                    .Cast<XElement>().ToList();

            Assert.AreEqual(0, additionalHandlings.Count);
        }

        [TestMethod]
        public void IsDomesticUsOrPuertoRico_ReturnsTrue_WhenBothCountriesArePuertoRico()
        {
            var shipment = new ShipmentEntity {OriginCountryCode = "PR", ShipCountryCode = "PR"};
            var result = UpsApiCore.IsDomesticUnitedStatesOrPuertoRico(shipment);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDomesticUsOrPuertoRico_ReturnsTrue_WhenBothCountriesAreUS()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "US", ShipCountryCode = "US" };
            var result = UpsApiCore.IsDomesticUnitedStatesOrPuertoRico(shipment);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDomesticUsOrPuertoRico_ReturnsTrue_WhenBothCountriesAreUSAndBothStatesArePuertoRico()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "US", OriginStateProvCode = "PR", ShipCountryCode = "US", ShipStateProvCode = "PR" };
            var result = UpsApiCore.IsDomesticUnitedStatesOrPuertoRico(shipment);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDomesticUsOrPuertoRico_ReturnsTrue_WhenBothCountriesAreUSAndBothStatesAreContinental()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "US", OriginStateProvCode = "MO", ShipCountryCode = "US", ShipStateProvCode = "IL" };
            var result = UpsApiCore.IsDomesticUnitedStatesOrPuertoRico(shipment);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDomesticUsOrPuertoRico_ReturnsFalse_WhenGoingFromPuertoRicoToUS()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "PR", ShipCountryCode = "US" };
            var result = UpsApiCore.IsDomesticUnitedStatesOrPuertoRico(shipment);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDomesticUsOrPuertoRico_ReturnsFalse_WhenGoingFromUSToPuertoRico()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "US", ShipCountryCode = "PR" };
            var result = UpsApiCore.IsDomesticUnitedStatesOrPuertoRico(shipment);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDomesticUsOrPuertoRico_ReturnsFalse_WhenBothCountriesAreUSButOriginStateIsPuertoRico()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "US", OriginStateProvCode = "PR", ShipCountryCode = "US" };
            var result = UpsApiCore.IsDomesticUnitedStatesOrPuertoRico(shipment);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDomesticUsOrPuertoRico_ReturnsFalse_WhenBothCountriesAreUSButDestinationStateIsPuertoRico()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "US", ShipCountryCode = "US", ShipStateProvCode = "PR" };
            var result = UpsApiCore.IsDomesticUnitedStatesOrPuertoRico(shipment);
            Assert.IsFalse(result);
        }
    }
}
