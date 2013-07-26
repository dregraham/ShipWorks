using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.XPath;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;
using System.IO;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Object with builtin XML serialization features that are a little more overridable and better
    /// than builtin XML serialization.
    /// </summary>
    public class SerializableObject
    {
        /// <summary>
        /// Serialize the object to an XML string using the specified tag as the root
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected string SerializeXml(string rootTag)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.Unicode))
                {
                    xmlWriter.Formatting = Formatting.Indented;

                    xmlWriter.WriteStartDocument(true);
                    xmlWriter.WriteStartElement(rootTag);

                    SerializeXml(xmlWriter);

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();

                    xmlWriter.Flush();

                    // Move the stream to the beginning
                    stream.Seek(0, SeekOrigin.Begin);

                    // Read it back
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Save the instance as XML to the given writer
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void SerializeXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter == null)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            // Find all public properties
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                // Skip ignored ones
                if (IsIgnored(property))
                {
                    continue;
                }

                // Start a tag for the property
                xmlWriter.WriteStartElement(property.Name);

                object value = property.GetValue(this, null);

                // Only save non-null values
                if (value != null)
                {
                    // Its its a collection, save each item
                    IList list = value as IList;
                    if (list != null)
                    {
                        foreach (object item in list)
                        {
                            xmlWriter.WriteStartElement("Item");
                            InternalSerializeValue(xmlWriter, item, property, true);
                            xmlWriter.WriteEndElement();
                        }
                    }
                    else
                    {
                        InternalSerializeValue(xmlWriter, value, property, false);
                    }
                }

                // Close the property
                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Serialize the given value to the xml stream.  The content is already ensured to be within its own
        /// element, so an parent element is not required to be written.
        /// </summary>
        private void InternalSerializeValue(XmlTextWriter xmlWriter, object value, PropertyInfo property, bool isListItem)
        {
            if (value == null)
            {
                return;
            }

            SerializeValue(xmlWriter, value, property, isListItem);
        }

        /// <summary>
        /// Serialize the given value to the xml stream.  The content is already ensured to be within its own
        /// element, so an parent element is not required to be written.  The value will never be null.
        /// </summary>
        protected virtual void SerializeValue(XmlTextWriter xmlWriter, object value, PropertyInfo property, bool isListItem)
        {
            // If it's a list item it could be of any type, so we have to serialize the type.  If its not, then we can deserialize
            // it back just by using the type of the property.
            if (isListItem)
            {
                xmlWriter.WriteAttributeString("type", value.GetType().FullName);
            }

            xmlWriter.WriteAttributeString("value", SerializationUtility.SerializeValue(value));
        }

        /// <summary>
        /// Deserialize the given XML content
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void DeserializeXml(string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            XPathNavigator xpath = xmlDocument.CreateNavigator();
            xpath.MoveToFirstChild();

            DeserializeXml(xpath);
        }

        /// <summary>
        /// Load the object data from the given XPath 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void DeserializeXml(XPathNavigator xpath)
        {
            if (xpath == null)
            {
                throw new ArgumentNullException("xpath");
            }

            // Find all public properties
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                // Skip ignored ones
                if (IsIgnored(property))
                {
                    continue;
                }

                // Get the property node
                XPathNavigator xpathProperty = xpath.SelectSingleNode(property.Name);

                // If this property wasn't saved, just move on
                if (xpathProperty == null)
                {
                    continue;
                }

                IList list = property.GetValue(this, null) as IList;

                if (list != null)
                {
                    // Select all child elements, each one represents an item
                    XPathNodeIterator xpathItems = xpathProperty.SelectChildren(XPathNodeType.Element);
                    while (xpathItems.MoveNext())
                    {
                        XPathNavigator xpathItem = xpathItems.Current;

                        object value = InternalDeserializeValue(xpathItem, property, true);

                        list.Add(value);
                    }
                }
                else
                {
                    object value = InternalDeserializeValue(xpathProperty, property, false);

                    // Set the value
                    property.SetValue(this, value, null);
                }
            }
        }

        /// <summary>
        /// Deserialize a value of the given type from the given XML stream.
        /// </summary>
        private object InternalDeserializeValue(XPathNavigator xpathItem, PropertyInfo property, bool isListItem)
        {
            if (xpathItem.IsEmptyElement && !xpathItem.HasAttributes)
            {
                return null;
            }

            return DeserializeValue(xpathItem, property, isListItem);
        }

        /// <summary>
        /// Deserialize a value of the given type from the given XML stream.
        /// </summary>
        protected virtual object DeserializeValue(XPathNavigator xpathItem, PropertyInfo property, bool isListItem)
        {
            Type type = null;

            // If its not in a list, then the property itself is what we are deserializing to, so just use it's type.
            if (!isListItem)
            {
                type = property.PropertyType;
            }
            else
            {
                string typeString = XPathUtility.Evaluate(xpathItem, "@type", null);
                if (typeString == null)
                {
                    return null;
                }

                type = GetTypeByFullName(typeString);
            }

            string valueText = XPathUtility.Evaluate(xpathItem, "@value", null);
            if (valueText == null)
            {
                return null;
            }

            return SerializationUtility.DeserializeValue(valueText, type);
        }

        /// <summary>
        /// Gets a type from the specified type string
        /// </summary>
        /// <param name="value">Name and namespace of the type to get.</param>
        /// <returns>The actual type for the string, or null if it can't be found.</returns>
        /// <remarks>Subclasses should override this if they need the GetType method to
        /// look inside of their own assemblies.</remarks>
        protected virtual Type GetTypeByFullName(string value)
        {
            return Type.GetType(value);
        }

        /// <summary>
        /// Indicates if the property should be ignored for serialization purposes
        /// </summary>
        private bool IsIgnored(PropertyInfo property)
        {
            if (Attribute.GetCustomAttribute(property, typeof(XmlIgnoreAttribute), true) != null)
            {
                return true;
            }

            if (!property.CanRead)
            {
                return true;
            }

            if (!property.CanWrite)
            {
                // If its a list its ok - we aren't going to write to it, we are going to fill it.
                if (typeof(IList).IsAssignableFrom(property.PropertyType))
                {
                    return false;
                }

                // Generic we have to test too
                if (property.PropertyType.IsGenericType)
                {
                    if (typeof(IList<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition()))
                    {
                        return false;
                    }
                }

                // Perhaps its just object, so Reflection can know... if it's a non-null instance we can check further
                IList list = property.GetValue(this, null) as IList;
                if (list != null)
                {
                    return false;
                }

                // Unwritable, non-list - it's ignored
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
