using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using System.Collections;
using Interapptive.Shared;
using System.Xml.XPath;
using System.ComponentModel;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Base class for all elements that make up a filter definition
    /// </summary>
    public abstract class ConditionElement : SerializableObject
    {
        /// <summary>
        /// Generate the SQL for the condition clement
        /// </summary>
        public abstract string GenerateSql(SqlGenerationContext context);

        /// <summary>
        /// Get the entity target that is in scope for this ConditionElement
        /// </summary>
        public abstract ConditionEntityTarget GetScopedEntityTarget();

        /// <summary>
        /// Providing debugging representation
        /// </summary>
        public override string ToString()
        {
            return ConditionElementFactory.GetDescriptor(GetType()).Identifier;
        }

        /// <summary>
        /// Serialize the given value to the xml stream.  The content is already ensured to be within its own
        /// element, so an parent element is not required to be written.  The value will never be null.
        /// </summary>
        protected sealed override void SerializeValue(XmlTextWriter xmlWriter, object value, PropertyInfo property, bool isListItem)
        {
            // If its a condition element, save itself
            ConditionElement element = value as ConditionElement;
            if (element != null)
            {
                xmlWriter.WriteAttributeString("identifier", ConditionElementFactory.GetDescriptor(element.GetType()).Identifier);

                element.SerializeXml(xmlWriter);
            }
            else
            {
                base.SerializeValue(xmlWriter, value, property, isListItem);
            }
        }

        /// <summary>
        /// Deserialize from the given xml stream a value of the specified type.
        /// </summary>
        protected sealed override object DeserializeValue(XPathNavigator xpathItem, PropertyInfo property, bool isListItem)
        {
            string identifier = XPathUtility.Evaluate(xpathItem, "@identifier", "");

            if (!string.IsNullOrEmpty(identifier))
            {
                ConditionElement element = ConditionElementFactory.CreateElement(identifier);

                element.DeserializeXml(xpathItem);

                return element;
            }
            else
            {
                return base.DeserializeValue(xpathItem, property, isListItem);
            }
        }
    }
}
