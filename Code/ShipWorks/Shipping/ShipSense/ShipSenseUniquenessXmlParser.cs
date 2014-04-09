using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ShipWorks.Shipping.ShipSense
{
    public class ShipSenseUniquenessXmlParser
    {
        /// <summary>
        /// Returns a list of item property/field names used to determine the uniqueness string.  Only the 
        /// names in this list are used when creating the uniqueness string.
        /// </summary>
        /// <param name="shipSenseUniquenessXml">The ShipSense uniqueness settings XML as a string.</param>
        public List<string> GetItemProperties(string shipSenseUniquenessXml)
        {
            List<string> propertiesToInclude = new List<string>();

            if (!string.IsNullOrWhiteSpace(shipSenseUniquenessXml))
            {
                try
                {
                    XElement shipSenseUniquenessXElement = XElement.Parse(shipSenseUniquenessXml);
                    propertiesToInclude = shipSenseUniquenessXElement
                        .Descendants("ItemProperty")
                        .Descendants("Name")
                        .Select(n => n.Value)
                        .OrderBy(n => n).ToList();
                }
                catch (InvalidOperationException ex)
                {
                    throw new ShipSenseException("ShipSense was unable to determine its property settings.", ex);
                }
            }

            return propertiesToInclude;
        }

        /// <summary>
        /// Returns a list of names used to determine uniqueness string.  Only the names in this list are used when
        /// creating the uniqueness string.
        /// </summary>
        /// <param name="shipSenseUniquenessXml">The ShipSense uniqueness settings XML as a string.</param>
        public List<string> GetItemAttributes(string shipSenseUniquenessXml)
        {
            List<string> attributeNamesToInclude = new List<string>();

            if (!string.IsNullOrWhiteSpace(shipSenseUniquenessXml))
            {
                try
                {
                    XElement shipSenseUniquenessXElement = XElement.Parse(shipSenseUniquenessXml);
                    attributeNamesToInclude = shipSenseUniquenessXElement
                        .Descendants("ItemAttribute")
                        .Descendants("Name")
                        .Select(n => n.Value.ToUpperInvariant())
                        .OrderBy(n => n).ToList();
                }
                catch (InvalidOperationException ex)
                {
                    throw new ShipSenseException("ShipSense was unable to determine its attribute settings.", ex);
                }
            }

            return attributeNamesToInclude;
        }
    }
}