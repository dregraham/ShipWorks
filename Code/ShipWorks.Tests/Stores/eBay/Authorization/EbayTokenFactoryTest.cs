using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;
using System.Reflection;

using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Readers;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Repositories;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Writers;

namespace ShipWorks.Tests.Stores.eBay.Authorization
{
    [TestClass]
    public class EbayTokenFactoryTest
    {
        EbayTokenFactory factory;

        public EbayTokenFactoryTest()
        {
            factory = new EbayTokenFactory();
        }

        [TestMethod]
        public void CreateReader_ReturnsXmlTokenReader_Test()
        {
            ITokenReader reader = factory.CreateReader("<XmlString/>");
            Assert.IsInstanceOfType(reader, typeof(XmlTokenReader));

        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CreateReader_ThrowsEbayException_WhenGivenInvalidXml_Test()
        {
            ITokenReader reader = factory.CreateReader(string.Empty);
        }

        [TestMethod]
        [DeploymentItem(@"Stores\eBay\Artifacts\TokenFileWithoutUserId.tkn")]
        public void CreateReader_ReturnsSecureXmlFileTokenReader_Test()
        {
            ITokenReader reader = factory.CreateReader(new FileInfo("TokenFileWithoutUserId.tkn"));
            Assert.IsInstanceOfType(reader, typeof(SecureXmlFileTokenReader));
        }

        [TestMethod]
        public void CreateWriter_WithStream_ReturnsSecureXmlTokenWriter_Test()
        {
            MemoryStream stream = new MemoryStream();
            ITokenWriter writer = factory.CreateWriter(stream);

            Assert.IsInstanceOfType(writer, typeof(SecureXmlTokenWriter));
        }

        [TestMethod]
        [DeploymentItem(@"Stores\eBay\Artifacts\TokenFileWithUserId.tkn")]
        public void CreateWriter_WithFileInfo_ReturnsSecureXmlFileTokenWriter_Test()
        {
            FileInfo file = new FileInfo("TokenFileWithUserId.tkn");
            ITokenWriter writer = factory.CreateWriter(file);

            Assert.IsInstanceOfType(writer, typeof(SecureXmlFileTokenWriter));
        }

        [TestMethod]
        public void CreateReader_ReturnsEbayTokenRepository_Test()
        {
            ITokenRepository reader = factory.CreateRepository("my license");
            Assert.IsInstanceOfType(reader, typeof(EbayTokenRepository));
        }
    }
}
