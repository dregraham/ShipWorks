using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Xunit;
using Moq;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Tests.Shipping.ShipSense
{
    public class KnowledgebaseEntryChangeSetXmlWriterTest
    {
        private KnowledgebaseEntryChangeSetXmlWriter testObject;

        private Mock<IChangeSetXmlWriter> packageXmlWriter;
        private Mock<IChangeSetXmlWriter> customsXmlWriter;
        private KnowledgebaseEntry entry;

        public KnowledgebaseEntryChangeSetXmlWriterTest()
        {
            entry = new KnowledgebaseEntry();
            entry.AppliedCustoms = true;

            packageXmlWriter = new Mock<IChangeSetXmlWriter>();
            customsXmlWriter = new Mock<IChangeSetXmlWriter>();

            testObject = new KnowledgebaseEntryChangeSetXmlWriter(entry, packageXmlWriter.Object, customsXmlWriter.Object);
        }

        [Fact]
        public void AppendChangeSet_ThrowsShipSenseException_WhenArgumentIsNull_Test()
        {
            Assert.Throws<ShipSenseException>(() => testObject.WriteTo(null));
        }

        [Fact]
        public void ChangeSet_HasTimestamp_Test()
        {
            XElement changeSets = new XElement("ChangeSets");

            testObject.WriteTo(changeSets);

            Assert.NotNull(changeSets.Descendants("ChangeSet").First().Attribute("Timestamp"));
        }

        [Fact]
        public void ChangeSets_HaveTimestamps_Test()
        {
            XElement changeSets = new XElement("ChangeSets");

            testObject.WriteTo(changeSets);
            testObject.WriteTo(changeSets);

            IEnumerable<string> timestamps = changeSets.Descendants("ChangeSet").Select(cs => cs.Attribute("Timestamp").Value);

            Assert.Equal(2, timestamps.Count());
        }

        [Fact]
        public void AppendChangeSet_DelegatesToPackageXmlWriter_Test()
        {
            XElement changeSets = new XElement("ChangeSets");

            testObject.WriteTo(changeSets);

            packageXmlWriter.Verify(w => w.WriteTo(It.IsAny<XElement>()), Times.Once());
        }

        [Fact]
        public void AppendChangeSet_DelegatesToCustomsXmlWriter_WhenAppliedCustomsIsTrue_Test()
        {
            entry.AppliedCustoms = true;
            XElement changeSets = new XElement("ChangeSets");

            testObject.WriteTo(changeSets);

            customsXmlWriter.Verify(w => w.WriteTo(It.IsAny<XElement>()), Times.Once());
        }

        [Fact]
        public void AppendChangeSet_DoesNotDelegateToCustomsXmlWriter_WhenAppliedCustomsIsFalse_Test()
        {
            entry.AppliedCustoms = false;
            XElement changeSets = new XElement("ChangeSets");

            testObject.WriteTo(changeSets);

            customsXmlWriter.Verify(w => w.WriteTo(It.IsAny<XElement>()), Times.Never());
        }

        [Fact]
        public void AppendChangeSet_AppendsChangeSetNode_ToElementProvided_Test()
        {
            XElement changeSets = new XElement("ChangeSets");

            testObject.WriteTo(changeSets);

            Assert.Equal(1, changeSets.Descendants("ChangeSet").Count());
        }

    }
}
