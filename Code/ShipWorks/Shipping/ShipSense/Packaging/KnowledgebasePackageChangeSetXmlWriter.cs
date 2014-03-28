using System.Collections.Generic;
using System.Xml.Linq;

namespace ShipWorks.Shipping.ShipSense.Packaging
{
    public class KnowledgebasePackageChangeSetXmlWriter : IChangeSetXmlWriter
    {
        private readonly IEnumerable<KnowledgebasePackage> before;
        private readonly IEnumerable<KnowledgebasePackage> after;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebasePackageChangeSetXmlWriter"/> class.
        /// </summary>
        /// <param name="before">The before.</param>
        /// <param name="after">The after.</param>
        public KnowledgebasePackageChangeSetXmlWriter(IEnumerable<KnowledgebasePackage> before, IEnumerable<KnowledgebasePackage> after)
        {
            this.before = before;
            this.after = after;
        }

        /// <summary>
        /// This method appends the XML representation of the ChangeSet to the given XElement.
        /// in the format of <Packages><Before><Package>....</Package></Before><After><Package>....</Package></After></Packages>
        /// </summary>
        /// <param name="changeSetXElement"></param>
        public void Write(XElement changeSetXElement)
        {
            // Build up the XML in the format of <Packages><Before><Package>....</Package></Before><After><Package>....</Package></After></Packages>
            // and append add it to the change set XML provided
            XElement beforeElement = new XElement("Before");
            foreach (KnowledgebasePackage package in before)
            {
                beforeElement.Add(BuildPackageXml(package));
            }

            XElement afterElement = new XElement("After");
            foreach (KnowledgebasePackage package in after)
            {
                afterElement.Add(BuildPackageXml(package));
            }

            XElement packagesElement = new XElement("Packages",
                beforeElement,
                afterElement);

            changeSetXElement.Add(packagesElement);
        }

        /// <summary>
        /// Builds the XML based on the package 
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>An XElement in form of <Package><Weight>XXX</Weight>...</Package></returns>
        private XElement BuildPackageXml(KnowledgebasePackage package)
        {
            XElement packageXml = new XElement("Package",
                new XElement("Weight", package.Weight),
                new XElement("Height", package.Height),
                new XElement("Width", package.Width),
                new XElement("Length", package.Length),
                new XElement("AdditionalWeight", package.AdditionalWeight),
                new XElement("ApplyAdditionalWeight", package.ApplyAdditionalWeight)
                );

            return packageXml;
        }
    }
}
