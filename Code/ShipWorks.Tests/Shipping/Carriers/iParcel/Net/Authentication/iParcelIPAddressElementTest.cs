using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.iParcel.Net.Authentication;
using Moq;
using log4net;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.Net.Authentication
{
    [TestClass]
    public class iParcelIPAddressElementTest
    {
        private iParcelIPAddressElement testObject;

        private Mock<ILog> logger; 

        [TestInitialize]
        public void Initialize()
        {
            logger = new Mock<ILog>();
            logger.Setup(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()));

            testObject = new iParcelIPAddressElement();
        }

        [TestMethod]
        public void Build_AddsIPAddressElement_Test()
        {
            XElement element = testObject.Build();

            Assert.AreEqual("IPAddresses", element.Name);
        }

        [TestMethod]
        public void Build_IPAddress_Test()
        {
            XElement element = testObject.Build();
            
            // No guarantees that the IP address will always be the same, so just check that it's not an empty string
            Assert.IsNotNull(element.Value);
            Assert.AreNotEqual(string.Empty, element.Value);
        }
    }
}
