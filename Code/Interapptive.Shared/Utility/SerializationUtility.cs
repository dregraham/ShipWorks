using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

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
                value = 0.01m  + (decimal) value - 0.01m;
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
            string serializedXml = string.Empty;

            if (value != null)
            {
                XmlSerializer serializer = new XmlSerializer(value.GetType());

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);
                    serializer.Serialize(memoryStream, value, namespaces);

                    StreamReader reader = new StreamReader(memoryStream);
                    memoryStream.Position = 0;
                    serializedXml = reader.ReadToEnd();
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

            byte[] bytes = Encoding.UTF8.GetBytes(serializedXml);

            using (var memoryStream = new MemoryStream(bytes))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                obj = (T)serializer.Deserialize(memoryStream);
            }

            return obj;
        }
    }
}
