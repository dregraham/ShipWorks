using System.Linq;
using System.Xml.Linq;
using ShipWorks.Shipping.ShipSense.Customs;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// An implementation of the IChangeSetXmlWriter interface intended to write the 
    /// change set for a knowledge base entry to an existing XML node.
    /// </summary>
    public class KnowledgebaseEntryChangeSetXmlWriter : IChangeSetXmlWriter
    {
        private readonly IChangeSetXmlWriter packageXmlWriter;
        private readonly IChangeSetXmlWriter customsXmlWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseEntryChangeSetXmlWriter"/> class. This
        /// uses the KnowledgebasePackageChangeSetXmlWriter and the KnowledgebaseCustomsItemXmlWriter
        /// implementations to write the package and customs data.
        /// </summary>
        /// <param name="entry">The knowledge base entry containing the package and customs data to record a change set for.</param>
        public KnowledgebaseEntryChangeSetXmlWriter(KnowledgebaseEntry entry)
            : this(new KnowledgebasePackageChangeSetXmlWriter(entry.OriginalPackages, entry.Packages), new KnowledgebaseCustomsItemXmlWriter(entry.OriginalCustomsItems, entry.CustomsItems))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseEntryChangeSetXmlWriter"/> class.
        /// </summary>
        /// <param name="packageXmlWriter">The package XML writer.</param>
        /// <param name="customsXmlWriter">The customs XML writer.</param>
        public KnowledgebaseEntryChangeSetXmlWriter(IChangeSetXmlWriter packageXmlWriter, IChangeSetXmlWriter customsXmlWriter)
        {
            this.packageXmlWriter = packageXmlWriter;
            this.customsXmlWriter = customsXmlWriter;
        }

        /// <summary>
        /// Writes the XML representation of a knowledge base entry change set to the given XElement 
        /// in the format of <ChangeSet><Packages></Packages><CustomsItems></CustomsItems></ChangeSet>. 
        /// The change set for the knowledge base entry is written to the "<ChangeSets/>" node of the 
        /// XElement provided; if this node does not exist, one is created.
        /// </summary>
        /// <param name="element">The XElement being written to.</param>
        public void WriteTo(XElement element)
        {
            if (element == null)
            {
                throw new ShipSenseException("Cannot write change set to a null value.");
            }

            if (!element.Descendants("ChangeSets").Any())
            {
                element.Add(new XElement("ChangeSets"));
            }

            // Write the package and customs data to a new ChangeSet node and append it to the
            // group of change sets
            XElement changeSet = new XElement("ChangeSet");

            packageXmlWriter.WriteTo(changeSet);
            customsXmlWriter.WriteTo(changeSet);

            element.Add(changeSet);
        }
    }
}
