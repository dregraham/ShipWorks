using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ShipWorks.Shipping.ShipSense.Customs
{
    /// <summary>
    /// Xml write for customs items
    /// </summary>
    public class KnowledgebaseCustomsItemXmlWriter : IChangeSetXmlWriter
    {
        private IEnumerable<KnowledgebaseCustomsItem> before;
        private IEnumerable<KnowledgebaseCustomsItem> after;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="before">List of kb customs items before any changes.</param>
        /// <param name="after">List of kb customs items after changes applied.</param>
        public KnowledgebaseCustomsItemXmlWriter(IEnumerable<KnowledgebaseCustomsItem> before, IEnumerable<KnowledgebaseCustomsItem> after)
        {
            this.before = before;
            this.after = after;
        }

        /// <summary>
        /// This method appends the XML representation of the ChangeSet to the given XElement.
        /// </summary>
        public void Write(XElement changeSetXElement)
        {
            // Create the before customs items
            XElement beforeCustomsElements = new XElement("Before",
                from kbciBefore in before
                select CreateCustomsItem(kbciBefore));

            // Create the after customs items
            XElement afterCustomsElements = new XElement("After",
                from kbciBefore in after
                select CreateCustomsItem(kbciBefore));

            // Create the before CustomsItems element with the before and after customs
            XElement customsItems = new XElement("CustomsItems", beforeCustomsElements, afterCustomsElements);

            changeSetXElement.Add(customsItems);
        }

        /// <summary>
        /// Creates an XElement based on the given KnowledgebaseCustomsItem
        /// </summary>
        private static XElement CreateCustomsItem(KnowledgebaseCustomsItem knowledgebaseCustomsItem)
        {
            return new XElement("CustomsItem",
                new XElement("Description", knowledgebaseCustomsItem.Description),
                new XElement("CountryOfOrigin", knowledgebaseCustomsItem.CountryOfOrigin),
                new XElement("HarmonizedCode", knowledgebaseCustomsItem.HarmonizedCode),
                new XElement("NumberOfPieces", knowledgebaseCustomsItem.NumberOfPieces),
                new XElement("Quantity", knowledgebaseCustomsItem.Quantity),
                new XElement("UnitPriceAmount", knowledgebaseCustomsItem.UnitPriceAmount),
                new XElement("UnitValue", knowledgebaseCustomsItem.UnitValue),
                new XElement("Weight", knowledgebaseCustomsItem.Weight)
                );
        }
    }
}
