using System;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download
{
    /// <summary>
    /// An implementation of the INeweggSerializer that serializes and deserializes 
    /// a DownloadResult object.
    /// </summary>
    public class DownloadResponseSerializer : INeweggSerializer
    {
        /// <summary>
        /// Deserializes the specified XML. If an object is unable to be
        /// serialized from the XML, a null value is returned.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>An OrdersResult object.</returns>
        public object Deserialize(string xml)
        {
            DownloadResult ordersResult = SerializationUtility.DeserializeFromXml<DownloadResult>(xml) as DownloadResult;
            return ordersResult;
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
