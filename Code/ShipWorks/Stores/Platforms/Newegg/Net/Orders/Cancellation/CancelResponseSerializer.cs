using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation
{
    /// <summary>
    /// An implementation of the INeweggSerializer that serializes and deserializes 
    /// a CancelResult object.
    /// </summary>
    public class CancelResponseSerializer : INeweggSerializer
    {
        /// <summary>
        /// Deserializes the specified XML. If an object is unable to be
        /// serialized from the XML, a null value is returned.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>An OrdersResult object.</returns>
        public object Deserialize(string xml)
        {
            CancellationResult cancelResult = SerializationUtility.DeserializeFromXml<CancellationResult>(xml) as CancellationResult;

            return cancelResult;
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
