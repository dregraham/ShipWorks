using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Net.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.Net.Ship
{
    public class iParcelShipValidationElementTest
    {
        private iParcelShipValidationElement testObject;

        private iParcelCredentials credentials;
        private Mock<IiParcelServiceGateway> gateway;

        public iParcelShipValidationElementTest()
        {
            gateway = new Mock<IiParcelServiceGateway>();
            gateway.Setup(g => g.IsValidUser(It.IsAny<iParcelCredentials>())).Returns(true);

            credentials = new iParcelCredentials("username", "password", false, gateway.Object);

            testObject = new iParcelShipValidationElement(credentials, true, true);
        }

        [Fact]
        public void Build_ValidationElement_ContainsRequestTypeElement_WhenNotUsedForRates()
        {
            testObject = new iParcelShipValidationElement(credentials, true, false);

            XElement element = testObject.Build();
            XElement requestTypeElement = element.XPathSelectElement("/RequestType");

            Assert.NotNull(requestTypeElement);
            Assert.Equal("LIVE", requestTypeElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsRequestTypeElement_WhenUsedForRates()
        {
            testObject = new iParcelShipValidationElement(credentials, true, true);

            XElement element = testObject.Build();
            XElement requestTypeElement = element.XPathSelectElement("/RequestType");

            Assert.NotNull(requestTypeElement);
            Assert.Equal("TEST", requestTypeElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsReturnInfoElement()
        {
            XElement element = testObject.Build();
            XElement returnElement = element.XPathSelectElement("/ReturnInfo");

            Assert.NotNull(returnElement);
            Assert.Equal("ALL", returnElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsDomesticShippingElement()
        {
            XElement element = testObject.Build();
            XElement domesticElement = element.XPathSelectElement("/DomesticShipping");

            Assert.NotNull(domesticElement);
        }

        [Fact]
        public void Build_ValidationElement_DomesticShippingElementIsOne_WhenDomesticShippingIsTrue()
        {
            XElement element = testObject.Build();
            XElement domesticElement = element.XPathSelectElement("/DomesticShipping");
            
            Assert.Equal("1", domesticElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsDomesticShippingElementIsZero_WhenDomesticShippingIsFalse()
        {
            testObject = new iParcelShipValidationElement(credentials, false, true);

            XElement element = testObject.Build();
            XElement domesticElement = element.XPathSelectElement("/DomesticShipping");

            Assert.Equal("0", domesticElement.Value);
        }
    }
}
