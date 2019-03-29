using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility methods for custom serializations
    /// </summary>
    public static class SerializationUtility
    {
        /// <summary>
        /// Convert the given value to string format for serialization
        /// </summary>
        public static string SerializeValue(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (value is Enum)
            {
                value = (int) value;
            }

            // For decimals, always make sure precision is to at least two places.  This helps to eliminate serialization mismatches (causing incorrect dirties)
            // when "0" goes to "0.00" at some point after user making edits.
            if (value is decimal)
            {
                value = 0.01m + (decimal) value - 0.01m;
            }

            var serializableObject = value as SerializableObject;
            if (serializableObject != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.ASCII);

                    serializableObject.SerializeXml(xmlTextWriter);

                    xmlTextWriter.Flush();

                    memoryStream.Position = 0;

                    var textReader = new StreamReader(memoryStream);

                    value = textReader.ReadToEnd();
                }
            }

            return string.Format("{0}", value);
        }

        /// <summary>
        /// Deserialize the value that had been serialized as a string
        /// </summary>
        public static object DeserializeValue(string value, Type type)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            // If it's Nullable<> get its underlying system version
            if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>)))
            {
                type = new NullableConverter(type).UnderlyingType;
            }

            Type enumType = null;

            if (type.IsEnum)
            {
                enumType = type;
                type = typeof(int);
            }

            object result;

            if (type == typeof(TimeSpan))
            {
                result = TimeSpan.Parse(value);
            }
            else if (type == typeof(Guid))
            {
                result = Guid.Parse(value);
            }
            else
            {
                result = Convert.ChangeType(value, type);
            }

            if (enumType != null)
            {
                result = Enum.ToObject(enumType, (int) result);
            }

            return result;
        }


        /// <summary>
        /// Serializes the given object to XML.
        /// </summary>
        /// <param name="value">The obj.</param>
        /// <returns>An XML representation of the object.</returns>
        public static string SerializeToXml(object value)
        {
            return SerializeToXml(value, false);
        }

        /// <summary>
        /// Serializes the given object to XML.
        /// </summary>
        /// <param name="value">The obj.</param>
        /// <param name="omitXmlDeclaration">Omit the xml declaration.</param>
        /// <returns>An XML representation of the object.</returns>
        public static string SerializeToXml(object value, bool omitXmlDeclaration)
        {
            string serializedXml = string.Empty;

            if (value != null)
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                XmlSerializer serializer = new XmlSerializer(value.GetType());

                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;

                if (omitXmlDeclaration)
                {
                    xmlWriterSettings.OmitXmlDeclaration = true;
                }

                using (StringWriter stringWriter = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
                    {
                        serializer.Serialize(writer, value, namespaces);
                        serializedXml = stringWriter.ToString();
                    }
                }
            }

            return serializedXml;
        }

        /// <summary>
        /// Deserializes the given XML into an object of the given type.
        /// </summary>
        /// <param name="serializedXml">The serialized XML.</param>
        /// <returns>An object representation of the XML.</returns>
        public static T DeserializeFromXml<T>(string serializedXml)
        {
            T obj;

            serializedXml = XmlUtility.StripInvalidXmlCharacters(serializedXml);

            // Check for common unescaped symbols
            serializedXml = serializedXml.Replace("®", "&#174;");
            serializedXml = serializedXml.Replace("©", "&#169;");
            serializedXml = serializedXml.Replace("™", "&#8482;");

            using (TextReader reader = new StringReader(serializedXml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                obj = (T) serializer.Deserialize(reader);
            }

            return obj;
        }
    }
}
