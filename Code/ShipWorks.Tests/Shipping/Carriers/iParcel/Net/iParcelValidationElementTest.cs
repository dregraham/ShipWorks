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

        [TestInitialize]
        public void Initialize()
        {
            gateway = new Mock<IiParcelServiceGateway>();
            gateway.Setup(g => g.IsValidUser(It.IsAny<iParcelCredentials>())).Returns(true);

            credentials = new iParcelCredentials("username", "password", false, gateway.Object);

            testObject = new iParcelValidationElement(credentials);
        }

        [Fact]
        public void Build_AddsValidationElement_Test()
        {
            XElement element = testObject.Build();

            Assert.AreEqual("Validation", element.Name);
        }

        [Fact]
        public void Build_ValidationElement_ContainsUsernameElement_Test()
        {
            XElement element = testObject.Build();
            XElement usernameElement = element.XPathSelectElement("/UserName");

            Assert.IsNotNull(usernameElement);
            Assert.AreEqual(credentials.Username, usernameElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsPasswordElement_Test()
        {
            XElement element = testObject.Build();
            XElement passwordElement = element.XPathSelectElement("/Password");

            Assert.IsNotNull(passwordElement);
            Assert.AreEqual(credentials.DecryptedPassword, passwordElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsAgreeTermsElement_Test()
        {
            XElement element = testObject.Build();
            XElement termsElement = element.XPathSelectElement("/AgreeTerms");

            Assert.IsNotNull(termsElement);
            Assert.AreEqual("1", termsElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsSDNDPLCheckedElement_Test()
        {
            XElement element = testObject.Build();
            XElement checkedElement = element.XPathSelectElement("/SDNDPLChecked");

            Assert.IsNotNull(checkedElement);
            Assert.AreEqual("1", checkedElement.Value);
        }

        [Fact]
        public void Build_ValidationElement_ContainsExportLicenseCheckedElement_Test()
        {
            XElement element = testObject.Build();
            XElement checkedElement = element.XPathSelectElement("/ExportLicenseChecked");

            Assert.IsNotNull(checkedElement);
            Assert.AreEqual("1", checkedElement.Value);
        }
    }
}
