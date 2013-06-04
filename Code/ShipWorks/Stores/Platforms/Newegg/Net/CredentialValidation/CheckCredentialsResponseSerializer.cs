using System;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation
{
    /// <summary>
    /// An implementation of the INeweggSerializer that serializes and deserializes 
    /// a CheckCredentialsResult object.
    /// </summary>
    public class CheckCredentialsResponseSerializer : INeweggSerializer
    {
        /// <summary>
        /// Deserializes the specified XML. If an object is unable to be
        /// serialized from the XML, a null value is returned.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>A CheckCredentialsResult object.</returns>
        public object Deserialize(string xml)
        {
            CheckCredentialsResult credentialsResult = SerializationUtility.DeserializeFromXml<CheckCredentialsResult>(xml) as CheckCredentialsResult;
            return credentialsResult;
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
