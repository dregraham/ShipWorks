using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Editions
{
    /// <summary>
    /// A class to encapsulate of any functionality available to shipment types that may be restricted. See the
    /// <see cref="ShipmentTypeRestrictionType"/> for a list of functionality that can be restricted.
    /// </summary>
    public class ShipmentTypeFunctionality
    {
        private readonly Dictionary<ShipmentTypeCode, List<ShipmentTypeRestrictionType>> shipmentTypeRestrictions;
        private XElement originalFunctionalitySource;

        /// <summary>
        /// Prevents a default instance of the <see cref="ShipmentTypeFunctionality"/> class from being created.
        /// </summary>
        private ShipmentTypeFunctionality()
        {
            shipmentTypeRestrictions = new Dictionary<ShipmentTypeCode, List<ShipmentTypeRestrictionType>>();
        }
        
        /// <summary>
        /// Deserializes the XML in the specified path looking for data in the ShipmentTypeFunctionality node.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>An instance of ShipmentTypeFunctionality.</returns>
        public static ShipmentTypeFunctionality Deserialize(long storeId, XPathNavigator path)
        {
            XElement document = null;

            if (path != null)
            {
                document = XElement.Parse(path.OuterXml);
            }

            return Deserialize(storeId, document);
        }

        /// <summary>
        /// Deserializes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static ShipmentTypeFunctionality Deserialize(long storeId, XElement source)
        {
            return Deserialize(storeId, source, ShippingPolicies.Load);
        }

        /// <summary>
        /// Deserializes the XML in the specified XElement for data in the ShipmentTypeFunctionality node; no restrictions
        /// are configured if the node is not found.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="storePolicyConfigurationAction">Action to store policy configuration.</param>
        /// <returns>
        /// An instance of ShipmentTypeFunctionality.
        /// </returns>
        public static ShipmentTypeFunctionality Deserialize(long storeId, XElement source, Action<long, List<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>>> storePolicyConfigurationAction)
        {
            ShipmentTypeFunctionality functionality = new ShipmentTypeFunctionality();

            // Extract appropriate node with functionality set (or create an empty one if needed)
            XElement sourcedElement = new XElement("ShipmentTypeFunctionality");
            if (source != null)
            {
                // Set the sourced element to an empty ShipmentTypeFunctionality element if it's
                // missing; this is useful, so we don't have to check for null in other methods 
                // (e.g. ToXElement, ToString)
                sourcedElement = source.Element("ShipmentTypeFunctionality") ?? new XElement("ShipmentTypeFunctionality");
            }

            List<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> policyConfiguration = new List<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>>();
            IEnumerable<XElement> typeElements = sourcedElement.Elements("ShipmentType");
            foreach (XElement type in typeElements)
            {
                ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode)(int)type.Attribute("TypeCode");
                IEnumerable<ShipmentTypeRestrictionType> restrictions = type.Elements("Restriction").Select(e => EnumHelper.GetEnumByApiValue<ShipmentTypeRestrictionType>(e.Value));

                functionality.AddShipmentTypeRestriction(shipmentTypeCode, restrictions.Distinct().ToList());

                List<XElement> features = type.Elements("Feature").ToList();
                if (features.Any())
                {
                    policyConfiguration.Add(new KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>(shipmentTypeCode, features));                    
                }
            }

            // Record the raw data that was used to populate the object; we'll use this later
            // for serialization purposes. 
            functionality.originalFunctionalitySource = sourcedElement;

            storePolicyConfigurationAction(storeId, policyConfiguration);
            
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return originalFunctionalitySource.ToString();
        }

        /// <summary>
        /// Returns the functionality in the form of an XElement.
        /// </summary>
        /// <returns>A <see cref="System.Xml.Linq.XElement" /> that represents this instance.</returns>
        public XElement ToXElement()
        {
            return originalFunctionalitySource;
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
