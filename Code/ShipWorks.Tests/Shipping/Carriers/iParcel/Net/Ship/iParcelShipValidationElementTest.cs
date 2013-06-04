﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Net.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.Net.Ship
{
    [TestClass]
    public class iParcelShipValidationElementTest
    {
        private iParcelShipValidationElement testObject;

        private iParcelCredentials credentials;
        private Mock<IiParcelServiceGateway> gateway;

        [TestInitialize]
        public void Initialize()
        {
            gateway = new Mock<IiParcelServiceGateway>();
            gateway.Setup(g => g.IsValidUser(It.IsAny<iParcelCredentials>())).Returns(true);

            credentials = new iParcelCredentials("username", "password", false, gateway.Object);

            testObject = new iParcelShipValidationElement(credentials, true, true);
        }

        [TestMethod]
        public void Build_ValidationElement_ContainsRequestTypeElement_WhenNotUsedForRates_Test()
        {
            testObject = new iParcelShipValidationElement(credentials, true, false);

            XElement element = testObject.Build();
            XElement requestTypeElement = element.XPathSelectElement("/RequestType");

            Assert.IsNotNull(requestTypeElement);
            Assert.AreEqual("LIVE", requestTypeElement.Value);
        }

        [TestMethod]
        public void Build_ValidationElement_ContainsRequestTypeElement_WhenUsedForRates_Test()
        {
            testObject = new iParcelShipValidationElement(credentials, true, true);

            XElement element = testObject.Build();
            XElement requestTypeElement = element.XPathSelectElement("/RequestType");

            Assert.IsNotNull(requestTypeElement);
            Assert.AreEqual("TEST", requestTypeElement.Value);
        }

        [TestMethod]
        public void Build_ValidationElement_ContainsReturnInfoElement_Test()
        {
            XElement element = testObject.Build();
            XElement returnElement = element.XPathSelectElement("/ReturnInfo");

            Assert.IsNotNull(returnElement);
            Assert.AreEqual("ALL", returnElement.Value);
        }

        [TestMethod]
        public void Build_ValidationElement_ContainsDomesticShippingElement_Test()
        {
            XElement element = testObject.Build();
            XElement domesticElement = element.XPathSelectElement("/DomesticShipping");

            Assert.IsNotNull(domesticElement);
        }

        [TestMethod]
        public void Build_ValidationElement_DomesticShippingElementIsOne_WhenDomesticShippingIsTrue_Test()
        {
            XElement element = testObject.Build();
            XElement domesticElement = element.XPathSelectElement("/DomesticShipping");
            
            Assert.AreEqual("1", domesticElement.Value);
        }

        [TestMethod]
        public void Build_ValidationElement_ContainsDomesticShippingElementIsZero_WhenDomesticShippingIsFalse_Test()
        {
            testObject = new iParcelShipValidationElement(credentials, false, true);

            XElement element = testObject.Build();
            XElement domesticElement = element.XPathSelectElement("/DomesticShipping");

            Assert.AreEqual("0", domesticElement.Value);
        }
    }
}
