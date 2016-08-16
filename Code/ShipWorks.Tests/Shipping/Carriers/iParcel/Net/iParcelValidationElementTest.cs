using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Net;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.Net
{
    public class iParcelValidationElementTest
    {
        private iParcelValidationElement testObject;

        private iParcelCredentials credentials;
        private Mock<IiParcelServiceGateway> gateway;

        public iParcelValidationElementTest()
        {
            gateway = new Mock<IiParcelServiceGateway>();
            gateway.Setup(g => g.IsValidUser(It.IsAny<iParcelCredentials>())).Returns(true);

            credentials = new iParcelCredentials("username", "password", false, gateway.Object);

            testObject = new iParcelValidationElement(credentials);
        }

        [Fact]
        public void Build_AddsValidationElement()
        {
            XElement element = testObject.Build();

            Assert.Equal("Validation", element.Name);
        }

        [Fact]
        public void Build_ValidationElement_ContainsUsernameElement()
        {
            XElement element = testObject.Build();
            XElement usernameElement = element.XPathSelectElement("/UserName");

            Assert.NotNull(usernameElement);
            Assert.Equal(credentials.Username, usernameElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsPasswordElement()
        {
            XElement element = testObject.Build();
            XElement passwordElement = element.XPathSelectElement("/Password");

            Assert.NotNull(passwordElement);
            Assert.Equal(credentials.DecryptedPassword, passwordElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsAgreeTermsElement()
        {
            XElement element = testObject.Build();
            XElement termsElement = element.XPathSelectElement("/AgreeTerms");

            Assert.NotNull(termsElement);
            Assert.Equal("1", termsElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsSDNDPLCheckedElement()
        {
            XElement element = testObject.Build();
            XElement checkedElement = element.XPathSelectElement("/SDNDPLChecked");

            Assert.NotNull(checkedElement);
            Assert.Equal("1", checkedElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsExportLicenseCheckedElement()
        {
            XElement element = testObject.Build();
            XElement checkedElement = element.XPathSelectElement("/ExportLicenseChecked");

            Assert.NotNull(checkedElement);
            Assert.Equal("1", checkedElement.Value);
        }
    }
}
