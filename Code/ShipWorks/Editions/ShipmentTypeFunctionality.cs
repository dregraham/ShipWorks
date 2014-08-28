using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using ShipWorks.Shipping;

namespace ShipWorks.Editions
{
    /// <summary>
    /// A class to encapsulate of any functionality available to shipment types that may be restricted. See the
    /// <see cref="ShipmentTypeRestrictionType"/> for a list of functionality that can be restricted.
    /// </summary>
    public class ShipmentTypeFunctionality
    {
        private readonly Dictionary<ShipmentTypeCode, List<ShipmentTypeRestrictionType>> shipmentTypeRestrictions;

        /// <summary>
        /// Prevents a default instance of the <see cref="ShipmentTypeFunctionality"/> class from being created.
        /// </summary>
        private ShipmentTypeFunctionality()
        {
            shipmentTypeRestrictions = new Dictionary<ShipmentTypeCode, List<ShipmentTypeRestrictionType>>();
        }

        /// <summary>
        /// Parses the XML in the specified path looking for data in the ShipmentTypeFunctionality node.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>An instance of ShipmentTypeFunctionality.</returns>
        public static ShipmentTypeFunctionality Parse(XPathNavigator path)
        {
            ShipmentTypeFunctionality functionality = new ShipmentTypeFunctionality();

            XElement document = XElement.Parse(path.OuterXml);
            XElement functionalityElement = document.Element("ShipmentTypeFunctionality");

            if (functionalityElement != null)
            {
                IEnumerable<XElement> typeElements = functionalityElement.Elements("ShipmentType");
                foreach (XElement type in typeElements)
                {
                    ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) (int) type.Attribute("TypeCode");
                    IEnumerable<ShipmentTypeRestrictionType> restrictions = type.Elements("Restriction").Select(e => Enum.Parse(typeof(ShipmentTypeRestrictionType), e.Value, true)).Cast<ShipmentTypeRestrictionType>();

                    functionality.AddShipmentTypeRestriction(shipmentTypeCode, restrictions.Distinct().ToList());
                }
            }

            return functionality;
        }

        /// <summary>
        /// Gets the <see cref="IEnumerable{ShipmentTypeRestrictionType}"/> with the specified key.
        /// </summary>
        public IEnumerable<ShipmentTypeRestrictionType> this[ShipmentTypeCode key]
        {
            get { return shipmentTypeRestrictions.ContainsKey(key) ? shipmentTypeRestrictions[key] : new List<ShipmentTypeRestrictionType>(); }
        }

        /// <summary>
        /// Adds the list of the shipment type restrictions to the dictionary for the given shipment type code.
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code.</param>
        /// <param name="restrictions">The restrictions.</param>
        private void AddShipmentTypeRestriction(ShipmentTypeCode shipmentTypeCode, List<ShipmentTypeRestrictionType> restrictions)
        {
            if (shipmentTypeRestrictions.ContainsKey(shipmentTypeCode))
            {
                // The key already exists in the dictionary, so just add restrictions 
                // and remove any duplicates
                shipmentTypeRestrictions[shipmentTypeCode].AddRange(restrictions);
                shipmentTypeRestrictions[shipmentTypeCode] = shipmentTypeRestrictions[shipmentTypeCode].Distinct().ToList();
            }
            else
            {
                // No restrictions for this key yet, so add them
                shipmentTypeRestrictions.Add(shipmentTypeCode, restrictions);
            }
            
        }

    }
}
