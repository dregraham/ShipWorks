using System;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Errors
{
    /// <summary>
    /// An implementation of the INeweggSerializer that serializes and deserializes 
    /// an Error object.
    /// </summary>
    public class ErrorResponseSerializer : INeweggSerializer
    {
        /// <summary>
        /// Deserializes the specified XML. If an object is unable to be
        /// serialized from the XML, a null value is returned.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>An ErrorResult object.</returns>
        public object Deserialize(string xml)
        {
            ErrorResult errorResult = SerializationUtility.DeserializeFromXml<ErrorResult>(xml) as ErrorResult;
            return errorResult;
        }

        /// <summary>
        /// Serializes the specified object into XML.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An XML representation of the given object.
        /// </returns>
        public string Serialize(object value)
        {
            return SerializationUtility.SerializeToXml(value);
        }
    }
}
