using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Tests.Shipping.ShipSense
{
    [TestClass]
    public class KnowledgebaseEntryChangeSetXmlWriterTest
    {
        private KnowledgebaseEntryChangeSetXmlWriter testObject;
        
        private Mock<IChangeSetXmlWriter> packageXmlWriter;
        private Mock<IChangeSetXmlWriter> customsXmlWriter;

        [TestInitialize]
        public void Initialize()
        {
            packageXmlWriter = new Mock<IChangeSetXmlWriter>();
            customsXmlWriter = new Mock<IChangeSetXmlWriter>();

            testObject = new KnowledgebaseEntryChangeSetXmlWriter(packageXmlWriter.Object, customsXmlWriter.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ShipSenseException))]
        public void AppendChangeSet_ThrowsShipSenseException_WhenArgumentIsNull_Test()
        {
            testObject.WriteTo(null);
        }

        [TestMethod]
        public void AppendChangeSet_CreatesChangeSetsNode_WhenItDoesNotExist_Test()
        {
            XElement changeSets = new XElement("Something");

            testObject.WriteTo(changeSets);

            Assert.AreEqual(1, changeSets.Descendants("ChangeSets").Count());
        }

        [TestMethod]
        public void AppendChangeSet_DoesNotCreateChangeSetsNode_WhenItExists_Test()
        {
            XElement changeSets = new XElement("ChangeSets");

            testObject.WriteTo(changeSets);

            Assert.AreEqual(1, changeSets.Descendants("ChangeSets").Count());
        }

        [TestMethod]
        public void AppendChangeSet_DelegatesToPackageXmlWriter_Test()
        {
            XElement changeSets = new XElement("ChangeSets");

            testObject.WriteTo(changeSets);

            packageXmlWriter.Verify(w => w.WriteTo(It.IsAny<XElement>()), Times.Once());
        }

        [TestMethod]
        public void AppendChangeSet_DelegatesToCustomsXmlWriter_Test()
        {
            XElement changeSets = new XElement("ChangeSets");

            testObject.WriteTo(changeSets);

            customsXmlWriter.Verify(w => w.WriteTo(It.IsAny<XElement>()), Times.Once());
        }

        [TestMethod]
        public void AppendChangeSet_AppendsChangeSetNode_ToElementProvided_Test()
        {
            XElement changeSets = new XElement("ChangeSets");

            testObject.WriteTo(changeSets);

            Assert.AreEqual(1, changeSets.Descendants("ChangeSet").Count());
        }

    }
}
