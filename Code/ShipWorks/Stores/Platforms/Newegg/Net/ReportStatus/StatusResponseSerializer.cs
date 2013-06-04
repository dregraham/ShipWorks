using System;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus
{
    /// <summary>
    /// An implementation of the INeweggSerializer that serializes and deserializes 
    /// a StatusResult object.
    /// </summary>
    public class StatusResponseSerializer : INeweggSerializer
    {
        /// <summary>
        /// Deserializes the specified XML. If an object is unable to be
        /// serialized from the XML, a null value is returned.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>An StatusResult object.</returns>
        public object Deserialize(string xml)
        {
            StatusResult statusResult = SerializationUtility.DeserializeFromXml<StatusResult>(xml) as StatusResult;
            return statusResult;
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
